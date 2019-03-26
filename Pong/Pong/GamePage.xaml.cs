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

namespace Pong
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        private double windowHeight = Application.Current.MainPage.Height;

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
        int x = 650;
        int y = 100;

        //boolean variables for detecting the movement of the ball
        Boolean moveLeft = false;
        Boolean moveRight = true;
        Boolean moveUp = false;
        Boolean moveDown = true;
        private SKBitmap paddle;

        SKMatrix matrix = SKMatrix.MakeIdentity();

        private float paddleX;
        private float paddleY;

        float padX = (float)Application.Current.MainPage.Height - 200;
        float padY = (float)Application.Current.MainPage.Width / 2;
        int score = 0;
        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            BackgroundImage = "Assets/Images/background.jpg";
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            //background colour
            canvas.Clear();

            string str = "Score: " + score;

            // Create an SKPaint object to display the text
            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.White
            };

            textPaint.TextSize = 35;

            // And draw the text
            canvas.DrawText(str, 50, 50, textPaint);

            //width and height of the window
            int width = e.Info.Width;
            int height = e.Info.Height;

            #region ball
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

            canvas.Restore();

            #endregion

            //CODE FOR PADDLE
            string resourceID = "Pong.Assets.Images.paddle.png";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                paddle = SKBitmap.Decode(stream);
            }
            paddleX = width / 2;
            paddleY = height - 100;
            canvas.SetMatrix(matrix);
            canvas.DrawBitmap(paddle, paddleX, paddleY);

            // rect = matrix.MapRect(rect);
            //System.Diagnostics.Debug.WriteLine(paddleX + " " + paddleY);

//            if (x > paddleX && paddleX < padX + paddle.Width && y > paddleY && paddleY < padY + paddle.Height)
            if(x > paddleX && x < paddleX + paddle.Width && y > paddleY && y < paddleY + paddle.Height)
            {
                //var assembly2 = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
                //Stream audioStream = assembly2.GetManifestResourceStream("Pong.Assets.Sounds.bounce.wav");

                //var bounce = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                //bounce.Load(audioStream);

//                if (moveDown)
  //              {
                    score++;
                    moveUp = true;
                    moveDown = false;
                    y -= speedY;

                  //  bounce.Play();
                //}
                //else
                //{
                //    moveUp = false;
                //    moveDown = true;
                //    y += speedY;
                //}
            }

        }

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
                    }
                    break;

                case SKTouchAction.Released:
                case SKTouchAction.Cancelled:
                    touchId = -1;
                    break;
            }// switch statement

            //  paddleX = previousPoint.X + point.X;
            // paddleY = previousPoint.Y + point.Y;
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