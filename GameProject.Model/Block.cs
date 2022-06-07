namespace GameProject.Models
{
    public class Block
    {
        #region Properties
        public int Position { get; set; }
        public int Value { get; set; }
        public bool Moved { get; set; }
        public bool Merged { get; set; }

        #endregion

        public Block(int position)
        {
            Position = position;
            Value = 0;
            Moved = false;
            Merged = false;
        }
    }
}
