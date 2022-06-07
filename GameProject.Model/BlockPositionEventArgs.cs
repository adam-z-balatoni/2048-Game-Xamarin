using System;
using System.Collections.Generic;
using System.Text;

namespace GameProject.Model
{
    public class BlockPositionEventArgs : EventArgs
    {
        // Properties
        public int[] BlockPositions { get; set; }
        public int Score { get; set; }
        public int HighScore { get; set; }


        // Constructor
        public BlockPositionEventArgs(int size, int score, int highScore)
        {
            BlockPositions = new int[size];
            Score = score;
            HighScore = highScore;
        }
    }
}
