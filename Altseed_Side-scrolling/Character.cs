using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed_Side_scrolling
{
    public abstract class Character : asd.TextureObject2D
    {
        public asd.Vector2DF Velocity1, Velocity2;
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
            Movement = new asd.Vector2DF(0.0f, 0.0f);

            if (Velocity1.X + Velocity2.X < 0.0f)//左移動
            {
                if (!Map.Isblocked(Position + new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f) + new asd.Vector2DF(-Width / 2.0f, -Height / 2.0f)) &&
                    !Map.Isblocked(Position + new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f) + new asd.Vector2DF(-Width / 2.0f, Height / 2.0f - 1.0f)))
                {
                    Position += new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f);
                    Movement += new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f);
                    TurnLR = false;
                }
            }
            else if (Velocity1.X + Velocity2.X > 0.0f)//右移動
            {
                if (!Map.Isblocked(Position + new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f) + new asd.Vector2DF(Width / 2.0f, -Height / 2.0f)) &&
                    !Map.Isblocked(Position + new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f) + new asd.Vector2DF(Width / 2.0f, Height / 2.0f - 1.0f)))
                {
                    Position += new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f);
                    Movement += new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f);
                    TurnLR = true;
                }
            }

            if (Velocity1.Y < 0.0f)//上昇中
            {
                if (!Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(-Width / 2.0f, -Height / 2.0f)) &&
                   !Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(Width / 2.0f, -Height / 2.0f)))
                {
                    Position += new asd.Vector2DF(0.0f, Velocity1.Y);
                    Movement += new asd.Vector2DF(0.0f, Velocity1.Y);
                    Velocity1.Y += 0.2f;
                }
                else Velocity1.Y = 0.0f;
            }
            else//落下中
            {
                if (!Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(-Width / 2.0f, Height / 2.0f - 1.0f)) &&
                   !Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(Width / 2.0f, Height / 2.0f - 1.0f)))
                {
                    Position += new asd.Vector2DF(0.0f, Velocity1.Y);
                    Velocity1.Y = Math.Min(4.0f, Velocity1.Y + 0.2f);
                    if (!Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(-Width / 2.0f, Height / 2.0f - 1.0f)) &&
                   !Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(Width / 2.0f, Height / 2.0f - 1.0f)))
                    {
                        Movement += new asd.Vector2DF(0.0f, Velocity1.Y);
                    }
                }
                else
                {
                    //Velocity.Y = 0.0f;
                    while (Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(-Width / 2.0f, Height / 2.0f - 1.0f)) ||
                   Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(Width / 2.0f, Height / 2.0f - 1.0f))) Velocity1.Y -= 0.1f;
                }
            }
            Velocity2 = new asd.Vector2DF(0.0f, 0.0f);
            //敵との衝突検出
            IEnumerable<asd.Object2D> enemies = this.Layer.Objects;
            foreach (asd.Object2D obj in enemies)
            {
                if ((obj as Character) == null || obj == this) continue;
                asd.Vector2DF d;
                d = IsCollide((Character)obj);
                if (d.Y > 0.0f)
                {
                    OnCollide((Character)obj, d);
                }
            }
        }

        //めり込んだ量を返す
        protected asd.Vector2DF IsCollide(Character obj)
        {
            float dx = (Width + obj.Width) / 2.0f - Math.Abs(Position.X - obj.Position.X);
            if (dx > 0.0f)
            {
                float dy = (Height + obj.Height) / 2.0f - Math.Abs(Position.Y - obj.Position.Y);
                if (dy > 0.0f)
                {
                    return new asd.Vector2DF(dx, dy);
                }
            }
            return new asd.Vector2DF(-1.0f, -1.0f);
        }

        protected asd.Vector2DF IsCollide(asd.Chip2D chip)
        {
            float dx = (Width + 32.0f) / 2.0f - Math.Abs(Position.X - chip.Position.X);
            if (dx > 0.0f)
            {
                float dy = (Height + 32.0f) / 2.0f - Math.Abs(Position.Y - chip.Position.Y);
                if (dy > 0.0f)
                {
                    return new asd.Vector2DF(dx, dy);
                }
            }
            return new asd.Vector2DF(-1.0f, -1.0f);
        }

        protected virtual void OnCollide(Character obj, asd.Vector2DF d)
        {

        }
    }
}
