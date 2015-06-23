﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed_Side_scrolling
{
    public class Character : asd.TextureObject2D
    {
        protected asd.Vector2DF Velocity;
        protected asd.Vector2DF Movement;
        protected float Width, Height;
        protected int Anime;
        protected Maps Map;

        public Character(Maps map, float width, float height, asd.Vector2DF pos)
        {
            Width = width;
            Height = height;
            Map = map;
            Position = pos;
            Anime = 0;
        }

        protected override void OnUpdate()
        {
            IEnumerable<asd.Object2D> enemies = this.Layer.Objects;
            foreach (asd.Object2D obj in enemies)
            {
                if ((obj as Character) == null || obj==this) continue;
                if (IsCollide(obj as Character))
                {
                    OnCollide(obj as Character);
                }

            }
        }
        protected void Move()
        {
            Movement = new asd.Vector2DF(0.0f, 0.0f);

            if (Velocity.X < 0.0f)//左移動
            {
                if (!Map.Isblocked(Position + new asd.Vector2DF(Velocity.X, 0.0f) + new asd.Vector2DF(-Width / 2.0f, -Height / 2.0f)) &&
                    !Map.Isblocked(Position + new asd.Vector2DF(Velocity.X, 0.0f) + new asd.Vector2DF(-Width / 2.0f, Height / 2.0f-1.0f)))
                {
                    Position += new asd.Vector2DF(Velocity.X, 0.0f);
                    Movement += new asd.Vector2DF(Velocity.X, 0.0f);
                    Anime++;
                    TurnLR = false;
                }
            }
            else if (Velocity.X > 0.0f)//右移動
            {
                if (!Map.Isblocked(Position + new asd.Vector2DF(Velocity.X, 0.0f) + new asd.Vector2DF(Width / 2.0f, -Height / 2.0f )) &&
                    !Map.Isblocked(Position + new asd.Vector2DF(Velocity.X, 0.0f) + new asd.Vector2DF(Width / 2.0f, Height / 2.0f-1.0f)))
                {
                    Position += new asd.Vector2DF(Velocity.X, 0.0f);
                    Movement += new asd.Vector2DF(Velocity.X, 0.0f);
                    Anime++;
                    TurnLR = true;
                }
            }

            if (Velocity.Y < 0.0f)//上昇中
            {
                if (!Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity.Y) + new asd.Vector2DF(-Width / 2.0f, -Height / 2.0f )) &&
                   !Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity.Y) + new asd.Vector2DF(Width / 2.0f, -Height / 2.0f)))
                {
                    Position += new asd.Vector2DF(0.0f, Velocity.Y);
                    Movement += new asd.Vector2DF(0.0f, Velocity.Y);
                    Velocity.Y += 0.2f;
                }
                else Velocity.Y = 0.0f;
            }
            else//落下中
            {
                if (!Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity.Y) + new asd.Vector2DF(-Width / 2.0f, Height / 2.0f - 1.0f)) &&
                   !Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity.Y) + new asd.Vector2DF(Width / 2.0f, Height / 2.0f - 1.0f)))
                {
                    Position += new asd.Vector2DF(0.0f, Velocity.Y);
                    Velocity.Y = Math.Min(4.0f, Velocity.Y + 0.2f);
                    if (!Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity.Y) + new asd.Vector2DF(-Width / 2.0f, Height / 2.0f - 1.0f)) &&
                   !Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity.Y) + new asd.Vector2DF(Width / 2.0f, Height / 2.0f - 1.0f)))
                    {
                        Movement += new asd.Vector2DF(0.0f, Velocity.Y);
                    }
                }
                else
                {
                    //Velocity.Y = 0.0f;
                    while (Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity.Y) + new asd.Vector2DF(-Width / 2.0f, Height / 2.0f - 1.0f)) ||
                   Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity.Y) + new asd.Vector2DF(Width / 2.0f, Height / 2.0f - 1.0f))) Velocity.Y -= 0.1f;
                }
            }
        }
        
        protected bool IsCollide(Character obj)
        {
            if((Math.Abs(Position.X-obj.Position.X)<(Width+obj.Width)/2.0f))
            {
                if((Math.Abs(Position.Y-obj.Position.Y)<(Height+obj.Height)/2.0f))
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual void OnCollide(Character obj)
        {

        }
    }
}
