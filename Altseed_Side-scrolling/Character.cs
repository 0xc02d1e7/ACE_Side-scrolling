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
            Velocity1 = new asd.Vector2DF(0.0f, 0.0f);
            Velocity2 = new asd.Vector2DF(0.0f, 0.0f);

        }

        protected override void OnUpdate()
        {
            Velocity1.Y = Math.Min(Velocity1.Y+0.5f,4.0f);

            Movement = Velocity1+Velocity2;//結局どれだけ動いたのかが入る
            asd.Vector2DF PosBuffer = Position;
            Position += Movement;

            foreach (var block in Map.Blocks)
            {
                //if ((block as MapParts) == null) continue;
                asd.Vector2DF d = IsCollide(block);
                if(d.X>0.0f)
                {
                    Movement.X -= d.X;
                }
                if(d.Y>0.0f)
                {
                    Movement.Y -= d.Y;
                }
            }
            Anime = 0;
            Position = PosBuffer + Movement;
            /*
            foreach (var enemy in this.Layer.Objects)
            {
                if ((enemy as Character) == null || enemy == this) continue;//敵だけ見ればよい
                asd.Vector2DF d = IsCollide((Character)enemy);
                if (d.X > 0.0f || d.Y > 0.0f)
                {

                }
            }
            */
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
