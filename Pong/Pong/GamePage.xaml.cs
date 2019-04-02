using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace Pong
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        private bool _beginGame = true;

        //boolean variables for detecting the movement of the ball
        Boolean moveLeft = false;
        Boolean moveRight = true;
        Boolean moveUp = false;
        Boolean moveDown = true;

        public GamePage()
        {
            InitializeComponent();
            
            Device.StartTimer(TimeSpan.FromSeconds(1f/60), () =>
            {
                canvasView.InvalidateSurface();

                return true;
            });
        }

        //speed the ball is going
        int speedX = 2;
        int speedY = 2;

        //starting position of the ball
        int x ;
        int y ;

        private SKBitmap paddle;

        SKMatrix matrix = SKMatrix.MakeIdentity();

        private float paddleX;
        private float paddleY;

        int score = 0;
        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            BackgroundImage = "Assets/Images/background.jpg";
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            //background colour
            canvas.Clear();

            //output the score at the top of the game
            string str = "Score: " + score;

            // Create an SKPaint object to display the text
            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.White
            };

            textPaint.TextSize = 35;

            // And draw the text onto the window
            canvas.DrawText(str, 50, 50, textPaint);

            //width and height of the window
            int width = e.Info.Width;
            int height = e.Info.Height;

            if(_beginGame)
            {
                Random random = new Random();

                paddleX = width / 2;
                paddleY = height - 100;

                x = random.Next(0, width);
                y = 100;

                _beginGame = false;
            }

            #region code repainting and movement of the ball
            //CODE FOR BALL
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Black.ToSKColor(),
                StrokeWidth = 8
            };

            canvas.Save();
            canvas.DrawCircle(x, y, 12, paint);

            paint.Style = SKPaintStyle.Fill;
            paint.Color = SKColors.Blue;
            canvas.DrawCircle(x, y, 12, paint);

            // movement of the ball
            MoveBallVertically(height);
            MoveBallHorizontally(width);

            canvas.Restore();

            #endregion

            //CODE FOR PADDLE
            string resourceID = "Pong.Assets.Images.paddle.png";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                paddle = SKBitmap.Decode(stream);
            }
            //paddleX = width / 2;
            //paddleY = height - 100;
            canvas.SetMatrix(matrix);
            canvas.DrawBitmap(paddle, paddleX, paddleY);

            CheckForCollision();
 
        }// CanvasView_PaintSurface

        private void MoveBallVertically(int height)
        {
            if (moveDown)
            {
                if (y >= height)
                {
                    y -= speedY;
                    moveDown = false;
                    moveUp = true;
                }
                else
                {
                    y += speedY;
                }
            }

            if (moveUp)
            {
                if (y <= 0)
                {
                    y += speedY;
                    moveUp = false;
                    moveDown = true;
                }
                else
                {
                    y -= speedY;
                }
            }
        } //MoveBallVertically

        private void MoveBallHorizontally(int width)
        {
            if (moveLeft)
            {
                if (x <= 0)
                {
                    x += speedX;
                    moveLeft = false;
                    moveRight = true;
                }
                else
                {
                    x -= speedX;
                }
            }


            if (moveRight)
            {
                if (x >= width)
                {
                    x -= speedX;
                    moveRight = false;
                    moveLeft = true;
                }
                else
                {
                    x += speedX;
                }
            }
        }// MoveBallHorizontally

        private void CheckForCollision()
        {
            if (moveDown)
            {
                // check is the ball collides with the paddle and if so,bounce back & increment score by 1
                if (y >= paddleY && y <= paddleY + paddle.Height && x >= paddleX && x <= paddleX + paddle.Width)
                //if(y >= paddleY && x >= paddleX && x <= paddleX + 170)
                {

                    var assembly2 = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
                    Stream audioStream = assembly2.GetManifestResourceStream("Pong.Assets.Sounds.bounce.wav");

                    var bounce = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                    bounce.Load(audioStream);

                    score++;
                    moveUp = true;
                    moveDown = false;
                    y -= speedY;

                    bounce.Play();
                }
            }
        }// CheckForCollision


        // Touch information
        long touchId = -1;
        SKPoint previousPoint;
        SKRect rect;

        private void CanvasView_Touch(object sender, SKTouchEventArgs e)
        {
            SKPoint point =
                new SKPoint((float)(canvasView.CanvasSize.Width * e.Location.X / canvasView.Width),
                            (float)(canvasView.CanvasSize.Height * e.Location.Y / canvasView.Height));
            switch(e.ActionType)
            {
                case SKTouchAction.Pressed:

                    rect = new SKRect(paddleX, paddleY, paddleX + paddle.Width, paddleY + paddle.Height);
                    rect = matrix.MapRect(rect);
                   
                    // Determine if the touch was within that rectangle
                    if ((e.Location.X > rect.Left && e.Location.X < rect.Right)
                        && (e.Location.Y > rect.Top && e.Location.Y < rect.Bottom))
                    {
                        touchId = e.Id;
                        previousPoint = point;
                    }                   
                    break;
                case SKTouchAction.Moved:
                    if (touchId == e.Id)
                    {
                        // Adjust the matrix for the new position
                        matrix.TransX += point.X - previousPoint.X;
                        previousPoint = point;
                       // paddleX = matrix.TransX;
                        //paddleY = matrix.TransY;
                    }
                    break;

                case SKTouchAction.Released:
                case SKTouchAction.Cancelled:
                    touchId = -1;
                    break;
            }// switch statement
        }

        private void MoveLeftBtn_Clicked(object sender, EventArgs e)
        {
            matrix.TransX -= 20;
            paddleX -= 20;
        }

        private void MoveRightBtn_Clicked(object sender, EventArgs e)
        {
            matrix.TransX += 20;
            paddleX += 20;
        }

        //private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        //{
        //    var paddle = (Image)sender;

        //    if (e.StatusType == GestureStatus.Started)
        //    {
        //        _startTranslationX = paddle.TranslationX;
        //        _startTranslationY = paddle.TranslationY;
        //    }
        //    else if (e.StatusType == GestureStatus.Running)
        //    {
        //        paddle.TranslationX = _startTranslationX + e.TotalX;
        //        paddle.TranslationY = _startTranslationY + e.TotalY;
        //    }
        //    else if (e.StatusType == GestureStatus.Completed)
        //    {
        //        paddle.TranslationX = _startTranslationX + e.TotalX;
        //        paddle.TranslationY = _startTranslationY + e.TotalY;
        //    }
        //}

    }
}