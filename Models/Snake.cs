﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Snake.Enums;
using System;
using System.Collections.Generic;

namespace MonoGame_Snake.Models
{
    //Create a Snake class for the model of the snake
    public class Snake
    {
        private readonly Core _core;
        private Vector2 _position;
        private Texture2D? _textureBody;
        private int _length;

        public Direction Direction { get; set; } = Direction.Right;

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                if (IsInvalidPosition(value))
                {
                    _position = AdjustToAValidPosition(value);
                }
            }
        }

        public float Speed { get; set; }

        public Texture2D? TextureBody
        {
            get => _textureBody;
            set { _textureBody = value; BodySize = value?.Height ?? 20; }
        }

        public Texture2D? TextureHead {get;set;}

        public int Length
        {
            get => _length;
            set
            {
                _length = value;
                if (_length < 1)
                {
                    _length = 1;
                }
                if(Body is not null)
                {
                    var newPositions = GetNewPosition(Body[0]);
                    //Add the new position to the body of the snake, at the beginning of the snake
                    AddNewPosition(newPositions);
                    if (Speed > 0.0f)
                    {
                        Speed -= 0.005f;
                    }
                }

            }
        }

        public List<Vector2> Body { get; set; }

        public int BodySize { get; set; } = 50;


        public Snake(Core core)
        {
            _core = core;
            Speed = 0.5f;
            Length = 1;
            Body = new List<Vector2>();
            //Set the initial position of the snake in the middle of the screen use Vector2Extensions
            var x = _core.Graphics.PreferredBackBufferWidth / 2;
            var y = _core.Graphics.PreferredBackBufferHeight / 2;
            Position = new Vector2(x, y);
            Body.Add(Position);
        }

        private void AddNewPosition(Vector2 position)
        {
            Body.Insert(0, position);
        }

        public void ShowPosition(SpriteFont font, float scale, Vector2 position)
        {
            _core.SpriteBatch?.DrawString(font, $"Snake x={Position.X} y={Position.Y} (x={Body[0].X},y={Body[0].Y})", position, Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void ShowSpeed(SpriteFont font, float scale, Vector2 position)
        {
            _core.SpriteBatch?.DrawString(font, $"Speed={Speed} Direction={Direction}", position, Color.Black, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void Update(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                Direction = Direction.Up;
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                Direction = Direction.Down;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                Direction = Direction.Left;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                Direction = Direction.Right;
            }
        }

        public void Move(int bannerSize)
        {
            // Don't move if the position is the edge of the screen
            if (IsOnTheLimit(Position, bannerSize))
            {
                return;
            }
            var newHeadPosition = GetNewPosition(Body[0]);
            Position = newHeadPosition;
            Body.Insert(0, Position);
            //Delete last part of the snake
            if (Body.Count > Length)
            {
                Body.RemoveAt(Body.Count - 1);
            }
        }

        private bool IsOnTheLimit(Vector2 position,int bannerSize)
        {
            // Define the limits of the screen
            // var minX = BodySize;
            // var maxX = _core.Graphics.PreferredBackBufferWidth;
            // var minY = BodySize + bannerSize;
            // var maxY = _core.Graphics.PreferredBackBufferHeight;

            
            var minX = 0;
            var maxX = _core.Graphics.PreferredBackBufferWidth - BodySize;
            var minY = bannerSize;
            var maxY = _core.Graphics.PreferredBackBufferHeight - BodySize;

            // Calculate next position
            var nextPosition = GetNewPosition(position);

            // Check if the next position is out of the screen
            if (nextPosition.X < minX || nextPosition.X > maxX)
            {
                return true;
            }
            if (nextPosition.Y < minY || nextPosition.Y > maxY)
            {
                return true;
            }
            return false;
        }

        private Vector2 GetNewPosition(Vector2 basePosition)
        {
            var newPosition = basePosition;
            switch (Direction)
            {
                case Direction.Up:
                    newPosition.Y -= BodySize;
                    break;

                case Direction.Down:
                    newPosition.Y += BodySize;
                    break;

                case Direction.Left:
                    newPosition.X -= BodySize;
                    break;

                case Direction.Right:
                    newPosition.X += BodySize;
                    break;
            }
            return newPosition;
        }

        public void Draw()
        {
            foreach (var part in Body)
            {
                var texture = part == Body[0] ? TextureHead : TextureBody;

                // Calculate the origin as the center of the sprite
                //Vector2 origin = part;//new Vector2(texture?.Width ?? 1 / 2, texture?.Height ?? 1 / 2);
                // Calculate the origin as the center of the current texture

                // var originX = Direction switch
                // {
                //     Direction.Left =>  -1 * (BodySize) ,
                //     Direction.Right =>  (BodySize),
                //     _ => 0,
                // };
                // var originY = Direction switch
                // {
                //     Direction.Up =>  - BodySize ,
                //     Direction.Down =>  BodySize ,
                //     _ => 0,
                // };
                //Vector2 origin = new Vector2(BodySize/2 + BodySize/4, BodySize/2 + BodySize/4);

                

                // Get the rotation angle based on the direction
                float rotationAngle = GetRotation();

                DrawRectangleCenteredRotation(texture, new Rectangle((int)part.X, (int)part.Y, BodySize, BodySize), Color.White, rotationAngle, false, false);

                // Get the flip effect based on the direction
                //SpriteEffects flipEffect = GetFlipEffect();
                // var newX = Direction switch
                // {
                //     Direction.Left => part.X - BodySize / 2,
                //     Direction.Right => part.X + BodySize / 2,
                //     _ => part.X,
                // };

                //  var newY = Direction switch
                // {
                //     Direction.Up => part.Y - BodySize / 2,
                //     Direction.Down => part.Y + BodySize / 2,
                //     _ => part.Y,
                // };

                // var newPart = new Vector2(newX, newY);
                //var origin = new Vector2(texture?.Width ?? 1 / 2, texture?.Height ?? 1 / 2);
                //origin = Vector2.Zero;

                // Draw the texture with rotation and flip effect
                // _core.SpriteBatch?.Draw(texture, part, null,
                //     Color.White, rotationAngle,
                //     origin, 
                //     1f, SpriteEffects.None, 0f);
            }
        }

          public void DrawRectangleCenteredRotation(Texture2D textureImage, Rectangle rectangleAreaToDrawAt, Color color, float rotationInRadians, bool flipVertically, bool flipHorizontally)
        {
            SpriteEffects seffects = SpriteEffects.None;
            if (flipHorizontally)
                seffects = seffects | SpriteEffects.FlipHorizontally;
            if (flipVertically)
                seffects = seffects | SpriteEffects.FlipVertically;
            
            // We must make a couple adjustments in order to properly center this.
            Rectangle r = rectangleAreaToDrawAt;
            Rectangle destination = new Rectangle(r.X + r.Width /2, r.Y + r.Height /2 , r.Width, r.Height);
            Vector2 originOffset = new Vector2(textureImage.Width / 2, textureImage.Height / 2);
            
            // This is a full spriteBatch.Draw method it has lots of parameters to fully control the draw.
            _core.SpriteBatch?.Draw(textureImage, destination, new Rectangle(0, 0, textureImage.Width, textureImage.Height), color, rotationInRadians, originOffset, seffects, 0);
        }

                

                // // Calculate the origin as the center of the sprite
                // Vector2 origin = new Vector2(texture?.Width ?? 1 / 2, texture?.Height ?? 1 / 2);
                // // new Vector2(texture.Width / 2, texture.Height / 2);

                // //Rotation
                // var _rotationAngle = GetRotation();

                // // Calculate the new position after rotation
                // var newPart = Vector2.Transform(part, Matrix.CreateRotationZ(_rotationAngle));
                                
                // // Apply rotation transformation
                // _core.SpriteBatch?.Draw(texture, newPart, null, 
                //     Color.White, _rotationAngle,
                //     //new Vector2(texture?.Width ?? 1 / 2, texture?.Height ?? 1 / 2),
                //     origin,
                //     1f, SpriteEffects.None, 0f);

        //     }
        // }

        // private SpriteEffects GetFlipEffect()
        // {
        //     return Direction switch
        //     {
        //         Direction.Left => SpriteEffects.FlipHorizontally,
        //         Direction.Right => SpriteEffects.FlipHorizontally,
        //         //Direction.Up => SpriteEffects.FlipVertically,
        //         _ => SpriteEffects.None,
        //     };
        // }

        private float GetRotation()
        {
            return Direction switch
            {
                Direction.Up => MathHelper.ToRadians(270),
                Direction.Down => MathHelper.ToRadians(90),
                Direction.Left => MathHelper.ToRadians(180),
                Direction.Right => MathHelper.ToRadians(0),
                _ => 0f,
            };
        }

        public Vector2 RandomPositionWithoutSnake(int bannerSize)
        {
            //Create a random position for the food
            var position = GetRandomPosition(bannerSize);
            //Check if the random position is not in the snake
            while (Body.Contains(position))
            {
                position = GetRandomPosition(bannerSize);
            }
            return position;
        }

        public Vector2 GetRandomPosition(int bannerSize)
        {
            var random = new Random();
            var x = random.Next(0, _core.Graphics.PreferredBackBufferWidth / BodySize) * BodySize;
            var y = random.Next(0, _core.Graphics.PreferredBackBufferHeight / BodySize) * BodySize;            
            var position = new Vector2(x, y < bannerSize ? bannerSize : y);
            return position;
        }

        private Vector2 AdjustToAValidPosition(Vector2 value)
        {
            //Adjust the position base on a matrix of the screen of the size of the snake
            var x = (int)value.X / BodySize * BodySize;
            var y = (int)value.Y / BodySize * BodySize;
            return new Vector2(x, y);
        }

        private bool IsInvalidPosition(Vector2 value)
        {
            //Check if the position is out of the screen
            if (value.X < 0 || value.X > _core.Graphics.PreferredBackBufferWidth - BodySize)
            {
                return true;
            }
            if (value.Y < 0 || value.Y > _core.Graphics.PreferredBackBufferHeight - BodySize)
            {
                return true;
            }
            //Check if the position is aligned with the matrix of the screen of the size of the snake
            if (value.X % BodySize != 0 || value.Y % BodySize != 0)
            {
                return true;
            }
            return false;
        }
    }
}