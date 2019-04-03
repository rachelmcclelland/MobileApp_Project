using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Pong
{
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public Player() { }

        public Player(string n, int s)
        {
            Name = n;
            Score = s;
        }
    }
}
