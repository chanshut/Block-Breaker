using System;
using SplashKitSDK;

namespace BlockBreaker
{
    public class Paddle
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

         public Rectangle CollisionRectangle
        {
            get
            {
                return SplashKit.RectangleFrom(X, Y, Width, Height);
            }
        }

        public Paddle(Window gameWindow)
        {
            this.X = (gameWindow.Width - 100) / 2;
            this.Y = gameWindow.Height - 50;
            this.Width= 60;
            this.Height = 5;
        }

        public void HandleInput()
        {
            const int SPEED = 5;    
            if (SplashKit.KeyDown(KeyCode.LeftKey))
            {
                this.X -= SPEED;
            }
            else if (SplashKit.KeyDown(KeyCode.RightKey))
            {
                this.X += SPEED;
            }
        }

        public void Draw()
        {
            SplashKit.FillRectangle(Color.WhiteSmoke, this.X, this.Y, this.Width, this.Height);
        }

        public void StayOnWindow(Window gameWindow)
        {
            const int GAP = 10;

            if (this.X < GAP)
            {
                this.X = GAP;
            }
            else if (this.X > gameWindow.Width - this.Width - GAP)
            {
                this.X = gameWindow.Width - this.Width - GAP;
            }
        }
    }
}