using System;
using SplashKitSDK;

namespace BlockBreaker
{
    public class BlockBreaker
    {
        private Window _gameWindow;
        private Paddle _paddle;
        private Ball _ball;
        private List<Block> _blocks = new List<Block>();
        private int _blockWidth = 60;
        private int _blockHeight = 30;
        private int _numBlocksPerRow;
        private int _blockGAP = 4;
        private SplashKitSDK.Timer _timer;
        private int _score;
        private List<int> _scoreBoard = new List<int>();

        private int _lives;
        private bool _gameOver;

        public BlockBreaker()
        {
            this._gameWindow = new Window("Block Breaker", 390, 700);
            this._paddle = new Paddle(this._gameWindow);
            this._ball = new Ball(this._gameWindow);
            this._timer = SplashKit.CreateTimer("timer");
            this._score = 0;
            this._lives = 3;
            this._gameOver = false;
        }

        public void Run()
        {
            this.loadGame();

            while (!this._gameWindow.CloseRequested && SplashKit.KeyDown(KeyCode.EscapeKey) == false)
            {
                SplashKit.ProcessEvents();
                this.HandleInput();
                this.update();
                this.draw();

                if (this._gameOver)
                {
                    this.showGameOverMessage();

                    if (SplashKit.KeyDown(KeyCode.SpaceKey))
                    {
                        this._gameWindow.Clear(Color.Black);
                        this.restartGame();
                    }
                }

                this._gameWindow.Refresh(60);
            }

            this._gameWindow.Close();
        }

        private void loadGame()
        {
            this._gameWindow.Clear(Color.Black);
            this._timer.Start();
            this.initialiseBlocks();
        }

        private void HandleInput()
        {
            this._paddle.HandleInput();
            this._paddle.StayOnWindow(this._gameWindow);
        }

        private void update()
        {
            if (!this._gameOver)
            {
                // ball behavior
                this._ball.Update(this._gameWindow);
                this.checkCollisions();

                // block system
                this.updateBlocks();
                this.addBlocks();

                // game lives
                this.checkLives();
            }
        }

        private void draw()
        {
            this._gameWindow.Clear(Color.Black);

            this._paddle.Draw();
            this._ball.Draw();

            foreach (Block block in this._blocks)
            {
                if (!block.IsDestroyed)
                {
                    block.Draw();
                }
            }

            string scoreText = $"Score: {this._score}";
            this._gameWindow.DrawText(scoreText, Color.White, this._gameWindow.Width - 100, this._gameWindow.Height - 30);

            string livesText = $"Attampts: {this._lives}";
            this._gameWindow.DrawText(livesText, Color.White, 10, this._gameWindow.Height - 30);
        }

        private void checkCollisions()
        {
            SoundEffect CollidedWith = new SoundEffect("CollidedWith", "CollidedWith.wav");

            // Check for collisions between ball and paddle
            if (this._ball.CollidedWith(this._paddle))
            {
                CollidedWith.Play();
                this._ball.VelocityY *= -1;
            }

            // Check for collisions between ball and blocks
            foreach (Block block in this._blocks)
            {
                if (this._ball.CollidedWith(block))
                {
                    CollidedWith.Play();
                    block.IsDestroyed = true;
                    this._ball.VelocityY *= -1;

                    if (block is LineBlock)
                    {
                        this._score += 15;
                    }
                    else if (block is DotBlock)
                    {
                        this._score += 20;
                    }
                    else if (block is SquareBlock)
                    {
                        this._score += 20;
                    }
                    else if (block is Block)
                    {
                        this._score += 10;
                    }
                }
            }
        }

        private void initialiseBlocks()
        {
            // Initialising the block system
            this._numBlocksPerRow = this._gameWindow.Width / this._blockWidth;
            int numRows = 9;

            for (int row = 0; row < numRows; row++)
            {
                int y = row * (this._blockHeight + this._blockGAP) + this._blockGAP;

                for (int col = 0; col < this._numBlocksPerRow; col++)
                {
                    int x = col * (this._blockWidth + this._blockGAP) + this._blockGAP;
                    Block block;

                    // Randomly choose a block type
                    int random = new Random().Next(0, 4);
                    if (random == 0)
                    {
                        block = new Block(x, y, this._blockWidth, this._blockHeight);
                    }
                    else if (random == 1)
                    {
                        block = new LineBlock(x, y, this._blockWidth, this._blockHeight);
                    }
                    else if (random == 2)
                    {
                        block = new DotBlock(x, y, this._blockWidth, this._blockHeight);
                    }
                    else
                    {
                        block = new SquareBlock(x, y, this._blockWidth, this._blockHeight);
                    }

                    this._blocks.Add(block);
                }
            }
        }

        private void updateBlocks()
        {
            // Remove blocks that are destroyed
            List<Block> blocksToRemove = new List<Block>();

            foreach (Block block in this._blocks)
            {
                if (this._ball.CollidedWith(block))
                {
                    blocksToRemove.Add(block);
                }
            }

            foreach (Block block in blocksToRemove)
            {
                this._blocks.Remove(block);
            }
        }

        private void addBlocks()
        {
            // Add a row of blocks every 5 seconds
            if (this._timer.Ticks > 5000)
            {
                for (int col = 0; col < this._numBlocksPerRow; col++)
                {
                    int x = col * (this._blockWidth + this._blockGAP) + this._blockGAP;
                    int y = -this._blockHeight;

                    Block newblock;

                    // Randomly choose a block type
                    int random = new Random().Next(0, 4);
                    if (random == 0)
                    {
                        newblock = new Block(x, y, this._blockWidth, this._blockHeight);
                    }
                    else if (random == 1)
                    {
                        newblock = new LineBlock(x, y, this._blockWidth, this._blockHeight);
                    }
                    else if (random == 2)
                    {
                        newblock = new DotBlock(x, y, this._blockWidth, this._blockHeight);
                    }
                    else
                    {
                        newblock = new SquareBlock(x, y, this._blockWidth, this._blockHeight);
                    }

                    this._blocks.Add(newblock);
                }

                // Move existing blocks downward
                foreach (Block block in this._blocks)
                {
                    if (!block.IsDestroyed)
                    {
                        block.Y += this._blockHeight + this._blockGAP;
                    }
                }

                this._timer.Reset();
            }
        }

        private void checkLives()
        {
            SoundEffect GameOver = new SoundEffect("GameOver", "GameOver.wav");

            // if the ball drops off the bottom of the screen, lose a life
            if (this._ball.Y > this._gameWindow.Height)
            {
                SoundEffect BallDrop = new SoundEffect("BallDrop", "BallDrop.wav");
                BallDrop.Play();
                this._lives--;

                if (this._lives > 0)
                {
                    this._ball.Reset(this._gameWindow);
                }
                else
                {
                    this._gameOver = true;
                    this._scoreBoard.Add(_score);
                    GameOver.Play();
                }
            }

            // if the block corsses the paddle, lose a life
            bool blockCollided = false;
            foreach (Block block in this._blocks)
            {
                if (block.Height > this._paddle.Y)
                {
                    blockCollided = true;
                    break;
                }
            }
            if (blockCollided)
            {
                this._lives--;
                if (this._lives > 0)
                {
                    this._ball.Reset(this._gameWindow);
                }
                else
                {
                    this._gameOver = true;
                    this._scoreBoard.Add(_score);
                    GameOver.Play();
                }
            }
        }

        private void showGameOverMessage()
        {
            this._gameWindow.Clear(Color.Black);
            this._gameWindow.DrawText("--- Game Over ---", Color.White, (this._gameWindow.Width - 150) / 2, (this._gameWindow.Height - 300) / 2);
            this._gameWindow.DrawText($"You got score: {_score}", Color.White, (this._gameWindow.Width - 150) / 2, (this._gameWindow.Height - 250) / 2);
            this._gameWindow.DrawText("> Press SpaceKey to restart", Color.White, (this._gameWindow.Width - 200) / 2, (this._gameWindow.Height - 200) / 2);
            this._gameWindow.DrawText("> Press Esc to quit", Color.White, (this._gameWindow.Width - 200) / 2, (this._gameWindow.Height - 150) / 2);

            // Sort the scores in descending order with lambda expression
            this._scoreBoard.Sort((x, y) => y.CompareTo(x));

            // Display the scoreboard
            this._gameWindow.DrawText("-- Scoreboard --", Color.White, (this._gameWindow.Width - 150) / 2, (this._gameWindow.Height - 100) / 2);
            int y = (this._gameWindow.Height - 50) / 2;
            int rank = 1;
            foreach (int score in _scoreBoard)
            {
                this._gameWindow.DrawText($"{rank}. {score}", Color.White, (this._gameWindow.Width - 150) / 2, y);
                y += 30;
                rank++;
            }

        }
        
        private void restartGame()
        {
            this._gameOver = false;
            this._gameWindow.Clear(Color.Black);
            this._timer.Start();
            this._score = 0;
            this._lives = 3;
            this._blocks.Clear();
            this.initialiseBlocks();
            this._ball.Reset(this._gameWindow);
        }
    }
}