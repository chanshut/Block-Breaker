using System;
using SplashKitSDK;

namespace BlockBreaker
{
    public class Ball
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; set; }
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }

        public Rectangle CollisionRectangle
        {
            get
            {
                return SplashKit.RectangleFrom(this.X, this.Y, this.Radius * 2, this.Radius * 2);
            }
        }

        public Ball(Window gameWindow)
        {
            this.X = SplashKit.Rnd(gameWindow.Width);
            this.Y = gameWindow.Height / 2 + 100;
            this.Radius = 7;
            this.VelocityX = 2;
            this.VelocityY = 4;
        }

        public void Draw()
        {
            SplashKit.FillCircle(Color.White, this.X, this.Y, this.Radius);
        }

        public void Update(Window gameWindow)
        {
            this.X += this.VelocityX;
            this.Y += this.VelocityY;

            // Reverse direction if ball hits the left or right edge of window
            if (this.X < this.Radius || this.X > gameWindow.Width - this.Radius * 2)
            {
                this.VelocityX *= -1;
            }

            // Reverse direction if ball hits the top edge of window
            if (this.Y <= 0)
            {
                this.VelocityY *= -1;
            }
        }

        public bool CollidedWith(Paddle paddle)
        {
            return SplashKit.RectanglesIntersect(this.CollisionRectangle, paddle.CollisionRectangle);
        }

        public bool CollidedWith(Block block)
        {
            return SplashKit.RectanglesIntersect(this.CollisionRectangle, block.CollisionRectangle);
        }

        public void Reset(Window gameWindow)
        {
            this.X = SplashKit.Rnd(gameWindow.Width);
            this.Y = gameWindow.Height / 2 + 100;
            this.VelocityX = 2;
            this.VelocityY = 4;
        }
    }
}