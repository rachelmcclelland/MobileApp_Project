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
        bool bN;

        public MainPage()
        {
            InitializeComponent();
            BackgroundImage = "Assets/Images/background.jpg";
            setDefaultSettings();
        }

        private void setDefaultSettings()
        {
            bN = false;
        }

        private void GamePage_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GamePage());
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(entName.Text == "")
            {
                bN = false;
                playBtn.IsEnabled = false;
            }
            else
            {
                bN = true;
            }

            if (bN) playBtn.IsEnabled = true;

        }
    }
}
