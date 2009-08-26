using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Factories;
using FarseerGames.FarseerPhysics.Collisions;

namespace MarioPhysicsTest
{
    public class Camera2d
    {
        private Vector2? position;
        private Vector2 dimensions;
        private Vector2? center;
        private Vector2 padding;


        public Vector2 Center
        {
            get
            {
                if (this.center == null)
                {
                    this.center = new Vector2(this.Position.X + dimensions.X / 2, this.Position.Y + dimensions.Y / 2);
                }
                return center.Value;
            }

            set
            {
                this.position = null;
                this.center = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                if (this.position == null)
                {
                    this.position = new Vector2(this.Center.X - dimensions.X / 2, this.Center.Y - dimensions.Y / 2);
                }
                return position.Value;
            }

            set
            {
                this.center = null;
                this.position = value;
            }
        }


        public Camera2d(Vector2 position, Vector2 dimensions, Vector2 padding)
        {
            this.position = position;
            this.dimensions = dimensions;
            this.padding = padding;
        }

        public void Move(Vector2 p_moveVector)
        {
            position += p_moveVector;
        }

        public Vector2 GetScreenPosition(Vector2 spritePos)
        {
            var off = GetOffset(spritePos);
            off.Y = this.dimensions.Y - off.Y;
            return off;
        }

        private Vector2 GetOffset(Vector2 spritePos)
        {
            return new Vector2(spritePos.X - Position.X, (spritePos.Y - Position.Y));
        }

        public void KeepOnScreen(GameObject go)
        {
            var screenpos = this.GetOffset(go.Center);
            if (screenpos.X < this.padding.X )
            {
                Vector2 difference = new Vector2(screenpos.X - this.padding.X, 0);
                this.Move(difference);
            }
            else if (screenpos.X > (this.dimensions.X - this.padding.X))
            {
                Vector2 difference = new Vector2(screenpos.X - (this.dimensions.X - this.padding.X), 0);
                this.Move(difference);
            }

            if (screenpos.Y < this.padding.Y)
            {
                Vector2 difference = new Vector2(0, screenpos.Y - this.padding.Y);
                this.Move(difference);
            }
            else if (screenpos.Y > (this.dimensions.Y - this.padding.Y))
            {
                Vector2 difference = new Vector2(0, screenpos.Y - (this.dimensions.Y - this.padding.Y));
                this.Move(difference);
            }
        }

    }
}
