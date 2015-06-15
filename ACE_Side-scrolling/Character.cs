using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Side_scrolling
{
    public class Character : ace.TextureObject2D
    {
        protected ace.Vector2DF Velocity;
        protected ace.Vector2DF Movement;
        protected float Width, Height;
        protected int Anime;
        protected Maps Map;

        public Character(Maps map,float width,float height,ace.Vector2DF pos)
        {
            Width = width;
            Height = height;
            Map = map;
            Position = pos;
            Anime = 0;
        }



        protected void Move()
        {
            Movement = new ace.Vector2DF(0.0f, 0.0f);

            if (Velocity.X < 0.0f)//左移動
            {
                if (!Map.Isblocked(Position + new ace.Vector2DF(Velocity.X, 0.0f) + new ace.Vector2DF(-Width / 2.0f, -Height / 2.0f)) &&
                    !Map.Isblocked(Position + new ace.Vector2DF(Velocity.X, 0.0f) + new ace.Vector2DF(-Width / 2.0f, Height / 2.0f)))
                {
                    Position += new ace.Vector2DF(Velocity.X, 0.0f);
                    Movement += new ace.Vector2DF(Velocity.X, 0.0f);
                    Anime++;
                    TurnLR = false;
                }
            }
            else if (Velocity.X > 0.0f)//右移動
            {
                if (!Map.Isblocked(Position + new ace.Vector2DF(Velocity.X, 0.0f) + new ace.Vector2DF(Width / 2.0f, -Height / 2.0f)) &&
                    !Map.Isblocked(Position + new ace.Vector2DF(Velocity.X, 0.0f) + new ace.Vector2DF(Width / 2.0f, Height / 2.0f)))
                {
                    Position += new ace.Vector2DF(Velocity.X, 0.0f);
                    Movement += new ace.Vector2DF(Velocity.X, 0.0f);
                    Anime++;
                    TurnLR = true;
                }
            }

            if (Velocity.Y < 0.0f)//上昇中
            {
                if (!Map.Isblocked(Position + new ace.Vector2DF(0.0f, Velocity.Y) + new ace.Vector2DF(-Width / 2.0f, -Height / 2.0f)) &&
                   !Map.Isblocked(Position + new ace.Vector2DF(0.0f, Velocity.Y) + new ace.Vector2DF(Width / 2.0f, -Height / 2.0f)))
                {
                    Position += new ace.Vector2DF(0.0f, Velocity.Y);
                    Movement += new ace.Vector2DF(0.0f, Velocity.Y); 
                    Velocity.Y += 0.2f;
                }
                else Velocity.Y = 0.0f;
            }
            else//落下中
            {
                if (!Map.Isblocked(Position + new ace.Vector2DF(0.0f, Velocity.Y) + new ace.Vector2DF(-Width / 2.0f, Height / 2.0f)) &&
                   !Map.Isblocked(Position + new ace.Vector2DF(0.0f, Velocity.Y) + new ace.Vector2DF(Width / 2.0f, Height / 2.0f)))
                {
                    Position += new ace.Vector2DF(0.0f, Velocity.Y);
                    Velocity.Y = Math.Min(4.0f, Velocity.Y + 0.2f);
                    if (!Map.Isblocked(Position + new ace.Vector2DF(0.0f, Velocity.Y) + new ace.Vector2DF(-Width / 2.0f, Height / 2.0f)) &&
                   !Map.Isblocked(Position + new ace.Vector2DF(0.0f, Velocity.Y) + new ace.Vector2DF(Width / 2.0f, Height / 2.0f)))
                    {
                        Movement+=new ace.Vector2DF(0.0f,Velocity.Y);
                    }
                }
                else
                {
                    //Velocity.Y = 0.0f;
                    while (Map.Isblocked(Position + new ace.Vector2DF(0.0f, Velocity.Y) + new ace.Vector2DF(-Width / 2.0f, Height / 2.0f)) ||
                   Map.Isblocked(Position + new ace.Vector2DF(0.0f, Velocity.Y) + new ace.Vector2DF(Width / 2.0f, Height / 2.0f))) Velocity.Y -= 0.1f;
                }
            }
        }
    }
}
