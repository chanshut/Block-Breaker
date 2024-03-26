using System;
using SplashKitSDK;

namespace BlockBreaker
{
    public class Block
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsDestroyed { get; set; }
        public Color BlockColor;

        public Rectangle CollisionRectangle
        {
            get
            {
                return SplashKit.RectangleFrom(X, Y, Width, Height);
            }
        }

        public Block(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.BlockColor = (Color.RandomRGB(200));
            this.IsDestroyed = false;
        }

        public virtual void Draw()
        {
            if (!this.IsDestroyed)
            {
                SplashKit.FillRectangle(this.BlockColor, X, Y, Width, Height);
            }
        }
    }

    public class LineBlock : Block
    {
        public LineBlock(int x, int y, int width, int height) : base(x, y, width, height)
        {
            this.Height = height / 2;
        }

        public override void Draw()
        {
            if (!this.IsDestroyed)
            {
                SplashKit.FillRectangle(this.BlockColor, X, Y, Width, Height);

                // Draw diagonal lines within the block
                for (int i = 0; i < Width; i += 10)
                {
                    SplashKit.DrawLine(Color.Black, X + i, Y, X + i, Y + Height);
                }
            }
        }

    }

    public class DotBlock : Block
    {
        public DotBlock(int x, int y, int width, int height) : base(x, y, width, height)
        {
            this.Width = width / 2;

        }
        public override void Draw()
        {
            if (!this.IsDestroyed)
            {
                SplashKit.FillRectangle(Color.Black, X, Y, Width, Height);

                // Draw dots within the block
                int dotSize = 3;
                for (int i = dotSize; i < Width; i += dotSize * 2)
                {
                    for (int j = dotSize; j < Height; j += dotSize * 2)
                    {
                        SplashKit.FillCircle(this.BlockColor, X + i, Y + j, dotSize);
                    }
                }
            }
        }
    }
    public class SquareBlock : Block
    {
        public SquareBlock(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }
        public override void Draw()
        {
            if (!this.IsDestroyed)
            {
                SplashKit.FillRectangle(this.BlockColor, X, Y, Width, Height);

                // Draw square within the block
                int squareSize = Width / 2;
                for (int i = 0; i < Width; i += squareSize)
                {
                    for (int j = 0; j < Height; j += squareSize)
                    {
                        if ((i / squareSize + j / squareSize) % 2 == 0)
                        {
                            SplashKit.FillRectangle(Color.Black, X + i, Y + j, squareSize, squareSize);
                        }
                    }
                }
            }
        }
    }
}

