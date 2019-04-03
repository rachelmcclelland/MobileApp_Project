using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Pong;

namespace MyUtility
{
    public class Utils
    {

        public static ObservableCollection<Player> ReadPlayersFromFile()
        {
            ObservableCollection<Player> list = new ObservableCollection<Player>();
            string fileString;

            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string filename = Path.Combine(path, "playerDetails.txt");

                using (var reader = new StreamReader(filename))
                {
                    fileString = reader.ReadToEnd();
                }
            }
            catch
            {

                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("Pong.DefaultData.txt");

                using (var reader = new StreamReader(stream))
                {
                    fileString = reader.ReadToEnd();

                }

            } //catch 

            list = JsonConvert.DeserializeObject<ObservableCollection<Player>>(fileString);
            return list;
        }// ReadPlayersFromFile

        public static void SavePlayerToFile(ObservableCollection<Player> list)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filename = Path.Combine(path, "playerDetails.txt");

            using (var writer = new StreamWriter(filename, false))
            {
                string stringifiedText = JsonConvert.SerializeObject(list);

                writer.WriteLine(stringifiedText);
            }
        }// SavePlayerToFile

    }
}
