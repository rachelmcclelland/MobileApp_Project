using Pong;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using MyUtility;

namespace Pong
{
    public partial class MainPage : ContentPage
    {
        bool bN;

        public MainPage()
        {
            InitializeComponent();
            BackgroundImage = "Assets/Images/mainpageImage.jpg";
            SetDefaultSettings();
        }

        private void SetDefaultSettings()
        {
            bN = false;
            App.players = Utils.ReadPlayersFromFile();
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
                App.pName = entName.Text;
                save.IsEnabled = true;
            }

            //if (bN)
            //{
            //    playBtn.IsEnabled = true;
            //}

        }

        private void CheckIfPlayerExists(string entName)
        {
            foreach (var player in App.players.ToList())
            {
                if(player.Name == entName)
                {
                    break;
                }
                else
                {
                    player.Name = entName;
                    player.Score = 0;
                    App.players.Add(player);
                    Utils.SavePlayerToFile(App.players);
                }
            }
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            CheckIfPlayerExists(entName.Text);
            playBtn.IsEnabled = true;

        }
    }
}
