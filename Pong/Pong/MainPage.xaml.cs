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

        public MainPage()
        {
            InitializeComponent();
            setUpBackground();
            
        }

        private void setUpBackground()
        {
            //Image background = new Image();

            //var assembly = typeof(MainPage);
            //string filename = "Pong.Assets.Images.background.jpg";
            //background.Source = ImageSource.FromResource(filename, assembly);
            //background.Aspect = Aspect.AspectFill;

            BackgroundImage = "Assets/Images/background.png";  
        //    mainGrid.Children.Add(background);
        }

        private void GamePage_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GamePage());
        }
    }
}
