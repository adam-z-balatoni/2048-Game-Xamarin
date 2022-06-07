using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject.Model
{
    public class HighScoresEventArgs : EventArgs
    {
        public int[] TopHighScores { get; set; }
    }
}
