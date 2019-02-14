using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pong
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        private double _startTranslationX, _startTranslationY;
        private double windowHeight = Application.Current.MainPage.Height;
        private double windowWidth = Application.Current.MainPage.Width;

        public GamePage()
        {
            InitializeComponent();
    //        setDefaultSettings();
            addPaddle();
            addBallToGameAsync();

      //     DisplayAlert("width", " " + windowWidth, "Cancel");
        }

        private void addPaddle()
        {

            Image paddle = new Image();

            var assembly = typeof(MainPage);
            string filename = "Pong.Assets.Images.paddle.png";
            paddle.Source = ImageSource.FromResource(filename, assembly);

            paddle.TranslationY = (windowHeight - 200) ;


            var panGestureRecognizer = new PanGestureRecognizer();
            panGestureRecognizer.PanUpdated += PanGestureRecognizer_PanUpdated;

            paddle.GestureRecognizers.Add(panGestureRecognizer);

            board.Children.Add(paddle);

        }

        private void setDefaultSettings()
        {
            Image background = new Image();

            var assembly = typeof(MainPage);
            string filename = "Pong.Assets.Images.background.jpg";
            background.Source = ImageSource.FromResource(filename, assembly);
            background.Aspect = Aspect.AspectFill;
            background.BackgroundColor = Color.Blue;



            gridBoard.Children.Add(background);

        }

        private void addBallToGameAsync()
        {

            Random random = new Random();

  //          String ballPath = "Assets/Images/ball.png";

            Image ball = new Image();

            var assembly = typeof(MainPage);
            string filename = "Pong.Assets.Images.ball.png";
            ball.Source = ImageSource.FromResource(filename, assembly);
       //     ball.Source = ballPath;
            //          ball.TranslationX = random.Next(0, 500);
            //        ball.TranslationY = random.Next(0, 500);
            //putting the image in the middle of the page with 0, 0 ot 400
        //    ball.TranslationX = 200;
         //   ball.TranslationY = 5;
            

            board.Children.Add(ball);

            int x = 40;
            int y = 40;
            double ballX;
//            DisplayAlert("ball", "x = " + ball.TranslationX + " y = " + ball.TranslationY, "cancel");

            Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
            {
                ball.TranslateTo(x, y, 750);

                ballX = ball.TranslationX;
   //             DisplayAlert("Ball", " " + ballX, "cancel");

                if (ballX >= 200)
                {
                    x -= 40;
                    y -= 40;
                }
                else
                {
                    x += 40;
                    y += 40;
                }
                 return true; // True = Repeat again, False = Stop the timer
             });
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