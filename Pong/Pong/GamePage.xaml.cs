using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using CocosSharp;
using System.Reflection;
using System.IO;

namespace Pong
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        private double _startTranslationX, _startTranslationY;
        private double windowHeight = Application.Current.MainPage.Height;
    //    private double windowWidth = 
        private SKBitmap ball;

        public GamePage()
        {
            InitializeComponent();
    //        setDefaultSettings();
            addPaddle();

            Device.StartTimer(TimeSpan.FromSeconds(1f/60), () =>
            {
                canvasView.InvalidateSurface();

                return true;
            });
        }

        private void setDefaultSettings()
        {
            Image background = new Image();

            var assembly = typeof(MainPage);
            string filename = "Pong.Assets.Images.background.jpg";
            background.Source = ImageSource.FromResource(filename, assembly);
            background.Aspect = Aspect.AspectFill;
            background.BackgroundColor = Color.Blue;



            //gridBoard.Children.Add(background);

        }

        private void addPaddle()
        {

            Image paddle = new Image();

            var assembly = typeof(MainPage);
            string filename = "Pong.Assets.Images.paddle.png";
            paddle.Source = ImageSource.FromResource(filename, assembly);

           // paddle.TranslationX = 100;
            paddle.TranslationY = (windowHeight - 200) ;


            var panGestureRecognizer = new PanGestureRecognizer();
            panGestureRecognizer.PanUpdated += PanGestureRecognizer_PanUpdated;

            paddle.GestureRecognizers.Add(panGestureRecognizer);

            //board.Children.Add(paddle);

        }

        //speed the ball is going
        int speedX = 3;
        int speedY = 3;

        //starting position of the ball
        int x = 650;
        int y = 100;

        //boolean variables for detecting the movement of the ball
        Boolean moveLeft = false;
        Boolean moveRight = true;
        Boolean moveUp = false;
        Boolean moveDown = true;

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.Gray);

            int width = e.Info.Width;
            int height = e.Info.Height;

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Black.ToSKColor(),
                StrokeWidth = 10
            };

            canvas.Save();
            canvas.DrawCircle(x, y, 15, paint);

            paint.Style = SKPaintStyle.Fill;
            paint.Color = SKColors.Blue;
            canvas.DrawCircle(x, y, 15, paint);

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


            //string resourceID = "Pong.Assets.Images.ball.png";
            //Assembly assembly = GetType().GetTypeInfo().Assembly;

            //using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            //{
            //    ball = SKBitmap.Decode(stream);

            //}
            //canvas.DrawBitmap(ball, x, y);

            //x += 40;
            //y += 40;

            //canvasView.InvalidateSurface();
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var paddle = (Image)sender;

            if (e.StatusType == GestureStatus.Started)
            {
                _startTranslationX = paddle.TranslationX;
                _startTranslationY = paddle.TranslationY;
            }
            else if (e.StatusType == GestureStatus.Running)
            {
                paddle.TranslationX = _startTranslationX + e.TotalX;
                paddle.TranslationY = _startTranslationY + e.TotalY;
            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                paddle.TranslationX = _startTranslationX + e.TotalX;
                paddle.TranslationY = _startTranslationY + e.TotalY;
            }
        }

    }
}