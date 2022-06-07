using GameProject.Models;
using GameProject.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameProject.Model
{
    public class GameProjectModel
    {
        // Fields
        private readonly GameProjectJSONFilePersistence _persistence;
        private readonly int gameSize = 4;


        // Properties
        private Block[] GameBlocks { get; set; }
        private List<int> HighScores { get; set; }


        // Events
        public event EventHandler<BlockPositionEventArgs> GameStateChanged;
        public event EventHandler GameOver;
        public event EventHandler GameWon;
        public event EventHandler<HighScoresEventArgs> ShowHighScores;


        // Constructors
        public GameProjectModel(GameProjectJSONFilePersistence persistence)
        {
            _persistence = persistence;

            if (HighScores == null)
                HighScores = new List<int> { 0 };

            StartGame();
        }


        // PRIVATE METHODS //
        //
        private void OnGameStateChanged()
        {
            var blockPositionEventArgs = new BlockPositionEventArgs
                ((int)Math.Pow(gameSize, 2), CalculateScore(GameBlocks), HighScores.Max());

            for (int i = 0; i < blockPositionEventArgs.BlockPositions.Length; i++)
            {
                blockPositionEventArgs.BlockPositions[i] = GameBlocks[i].Value;
            }
            GameStateChanged?.Invoke(this, blockPositionEventArgs);
        }
        private void OnGameOver()
        {
            HighScores.Add(CalculateScore(GameBlocks));
            GameOver?.Invoke(this, EventArgs.Empty);
        }
        private void OnGameWon()
        {
            HighScores.Add(CalculateScore(GameBlocks));
            GameWon?.Invoke(this, EventArgs.Empty);
        }


        // Start Game & Create Grid
        private void StartGame()
        {
            GameBlocks = CreateGrid(gameSize);
            AddNewBlock(GameBlocks);
            OnGameStateChanged();
        }
        private Block[] CreateGrid(int gameSize)
        {
            int blocks = (int)Math.Pow(gameSize, 2);
            Block[] gameBlocks = new Block[blocks];
            for (int i = 0; i < blocks; i++)
            {
                gameBlocks[i] = new Block(i);
            }

            return gameBlocks;
        }


        // Add Block & Show Grid
        private int[] CountEmptyBlocks(Block[] gameBlocks)
        {
            int arrayLength = gameBlocks.Count(block => block.Value == 0);

            int[] emptyBlocks = new int[arrayLength];

            for (int i = 0, counter = 0; i < gameBlocks.Length; i++)
            {
                if (gameBlocks[i].Value == 0)
                {
                    emptyBlocks[counter] = gameBlocks[i].Position;
                    counter++;
                }
            }
            return emptyBlocks;
        }
        private bool AddNewBlock(Block[] gameBlocks)
        {
            int[] emptyBlocksArray = CountEmptyBlocks(gameBlocks);

            Random rnd = new Random();
            int emptyBlock = emptyBlocksArray[rnd.Next(0, emptyBlocksArray.Length)];
            gameBlocks[emptyBlock].Value++;

            // Is the Grid Full?
            return !(emptyBlocksArray.Length > 1);
            // False --> Continue
            // True --> Check End of Game Condition
        }


        // RunGameLoop
        public void RunGameLoop(MoveEnum direction)
        {
            bool blocksMoved;
            switch (direction)
            {
                case MoveEnum.UP:
                    blocksMoved = MoveActionUP();
                    break;
                case MoveEnum.LEFT:
                    blocksMoved = MoveActionLEFT();
                    break;
                case MoveEnum.DOWN:
                    blocksMoved = MoveActionDOWN();
                    break;
                default:
                    blocksMoved = MoveActionRIGHT();
                    break;
            }

            if(blocksMoved)
            {
                // Check End of Game Condition
                if (AddNewBlock(GameBlocks))
                {
                    if (IsGameEnded())
                        OnGameOver();
                }

                // Check Win Condition
                if (IsGameWon())
                {
                    OnGameWon();
                }
                OnGameStateChanged();
            }
        }


        // Reset Game
        public void ResetGame()
        {
            HighScores.Add(CalculateScore(GameBlocks));
            foreach (Block block in GameBlocks)
            {
                block.Value = 0;
            }
            AddNewBlock(GameBlocks);
            OnGameStateChanged();
        }


        // RequestUIUpdate
        public void RequestUIUpdate()
        {
            OnGameStateChanged();
        }


        // Move Action Methods
        public bool MoveActionUP()
        {
            bool blocksMoved = false;

            for (int i = 0; i < gameSize; i++)       // gameSize = 4
            {
                if (LineAction(GameBlocks[15 - i], GameBlocks[11 - i],
                               GameBlocks[7 - i], GameBlocks[3 - i]))
                    blocksMoved = true;
            }

            foreach (Block block in GameBlocks)
            {
                block.Moved = false;
                block.Merged = false;
            }

            return blocksMoved;
        }
        public bool MoveActionLEFT()
        {
            bool blocksMoved = false;

            for (int i = 0; i < gameSize; i++)       // gameSize = 4
            {
                if (LineAction(GameBlocks[3 + i * 4], GameBlocks[2 + i * 4],
                        GameBlocks[1 + i * 4], GameBlocks[0 + i * 4]))
                    blocksMoved = true;
            }

            foreach (Block block in GameBlocks)
            {
                block.Moved = false;
                block.Merged = false;
            }

            return blocksMoved;
        }
        public bool MoveActionDOWN()
        {
            bool blocksMoved = false;

            for (int i = 0; i < gameSize; i++)       // gameSize = 4
            {
                if (LineAction(GameBlocks[3 - i], GameBlocks[7 - i],
                        GameBlocks[11 - i], GameBlocks[15 - i]))
                    blocksMoved = true;
            }

            foreach (Block block in GameBlocks)
            {
                block.Moved = false;
                block.Merged = false;
            }

            return blocksMoved;
        }
        public bool MoveActionRIGHT()
        {
            bool blocksMoved = false;

            for (int i = 0; i < gameSize; i++)       // gameSize = 4
            {
                if (LineAction(GameBlocks[0 + i * 4], GameBlocks[1 + i * 4],
                        GameBlocks[2 + i * 4], GameBlocks[3 + i * 4]))
                    blocksMoved = true;
            }

            foreach (Block block in GameBlocks)
            {
                block.Moved = false;
                block.Merged = false;
            }

            return blocksMoved;
        }
        private bool LineAction(Block block0, Block block1, Block block2, Block block3)
        {
            BlockAction(block2, block3);
            ThreeBlockAction(block1, block2, block3);
            FourBlockAction(block0, block1, block2, block3);

            return block0.Moved || block1.Moved || block2.Moved;

        }
        private void BlockAction(Block block2, Block block3)
        {
            // Move Block2 to Block3
            // Block2 --> 0, Moved
            // Block3 --> Block2
            if (block2.Value != 0 && block3.Value == 0)
            {
                block3.Value = block2.Value;
                block2.Value = 0;
                block2.Moved = true;
            }

            // Merge Block2 to Block3
            // Block2 --> 0, Moved
            // Block3 --> ++, Merged
            else if (block2.Value != 0 && block2.Value == block3.Value)
            {
                block3.Value++;
                block3.Merged = true;
                block2.Value = 0;
                block2.Moved = true;
            }
        }
        private void ThreeBlockAction(Block block1, Block block2, Block block3)
        {
            // Initiate BlockAction of Block1 and Block3
            // if Block1 can be Moved to Block3
            if ((block2.Value == 0 && block3.Value == 0)
                // or Block1 can be Merged to Block3
                || (block2.Value == 0 && block1.Value == block3.Value && !block3.Merged))
            {
                BlockAction(block1, block3);
            }
            else
            {
                BlockAction(block1, block2);
            }
        }
        private void FourBlockAction(Block block0, Block block1, Block block2, Block block3)
        {
            // Initiate BlockAction of Block0 and Block3
            // if Block0 can be Moved to Block3
            if ((block1.Value == 0 && block2.Value == 0 && block3.Value == 0)
                // or Block0 can be Merged to Block3
                || (block1.Value == 0 && block2.Value == 0 && block0.Value == block3.Value && !block3.Merged))
            {
                BlockAction(block0, block3);
            }
            else
            {
                ThreeBlockAction(block0, block1, block2);
            }
        }


        // End of Game Conditions
        private bool IsGameWon()
        {
            foreach (Block block in GameBlocks)
            {
                if (block.Value == 11)
                    return true;
            }

            return false;
        }
        private bool IsGameEnded()
        {
            bool gameEnded = true;
            bool loopEnded = false;
            while (gameEnded && loopEnded == false)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (j == 3 && i < 3)
                        {
                            if (GameBlocks[j * 4 + i].Value == GameBlocks[j * 4 + i + 1].Value)
                                gameEnded = false;
                        }
                        else if (i == 3 && j < 3)
                        {
                            if (GameBlocks[j * 4 + i].Value == GameBlocks[(j + 1) * 4 + i].Value)
                                gameEnded = false;
                        }
                        else if (i < 3 || j < 3)
                        {
                            if (GameBlocks[j * 4 + i].Value == GameBlocks[j * 4 + i + 1].Value ||
                               GameBlocks[j * 4 + i].Value == GameBlocks[j * 4 + i + 4].Value)
                                gameEnded = false;
                        }
                    }
                }

                loopEnded = true;
            }

            return gameEnded;
        }


        // Calculate Score
        public int CalculateScore(Block[] gameBlocks)
        {
            int score = 0;
            foreach (Block block in gameBlocks)
            {
                score += (int)Math.Pow(block.Value, 2);
            }
            return score;
        }


        // Persistence
        public async Task SaveGameAsync()
        {
            int[] gameBlockValues = new int[16];
            for (int i = 0; i < gameBlockValues.Length; i++)
            {
                gameBlockValues[i] = GameBlocks[i].Value;
            }

            await _persistence.SaveGameStateAsync(new GameProjectState()
            {
                GameBlocks = gameBlockValues,
                HighScores = HighScores
            }); ;
        }
        public async Task LoadGameAsync()
        {
            GameProjectState state = await _persistence.LoadGameStateAsync();
            if (state != null)
            {
                for (int i = 0; i < GameBlocks.Length; i++)
                {
                    GameBlocks[i].Value = state.GameBlocks[i];
                }

                HighScores = state.HighScores;
                OnGameStateChanged();
            }
        }


        // HighScorePage
        public void OnShowHighScores()
        {
            var highScoresEventArgs = new HighScoresEventArgs
            {
                TopHighScores = new int[5]
            };

            HighScores.Sort();
            HighScores.Reverse();

            if (HighScores.Count < 5)
            {
                for (int i = 0; i < HighScores.Count; i++)
                {
                    highScoresEventArgs.TopHighScores[i] = HighScores[i];
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    highScoresEventArgs.TopHighScores[i] = HighScores[i];
                }
            }
            

            ShowHighScores?.Invoke(this, highScoresEventArgs);
        }
    }
}
