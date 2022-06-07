using System.Collections.Generic;

namespace GameProject.Persistence
{
    public class GameProjectState
    {
        public int[] GameBlocks { get; set; }   // Change to Block[] if necessary
        public List<int> HighScores { get; set; }
    }
}
