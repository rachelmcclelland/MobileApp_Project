using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pong
{
    public partial class MainPage : ContentPage
    {
        private double _startTranslationX, _startTranslationY;
        public MainPage()
        {
            InitializeComponent();

            addBallToGame();
        }

        private void addBallToGame()
        {
            int x, y;
            x = 30;
            y = 30;
            Random random = new Random();

            String ballPath = "Assets/Images/ball.png";

            Image ball = new Image();
            ball.Source = ballPath;
            ball.TranslationX = random.Next(0, 500);
            ball.TranslationY = random.Next(0, 500);

            board.Children.Add(ball);

      //      ball.TranslateTo(0, -30, 5000);

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
