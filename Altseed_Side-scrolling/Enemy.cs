using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed_Side_scrolling
{
    public class Enemy : Character
    {
        public int motal;
        public Enemy(string path, asd.Vector2DF pos, Maps map)
            : base(map, 32.0f, 32.0f, pos)
        {
            Texture = asd.Engine.Graphics.CreateTexture2D(path);
            Velocity1 = new asd.Vector2DF(2.0f, 0.0f);
            CenterPosition = new asd.Vector2DF((float)Texture.Size.X / 2.0f, (float)Texture.Size.Y / 2.0f);
            motal = 0;
            Width = Texture.Size.X;
            Height = Texture.Size.Y;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (Math.Abs(Movement.X) < 0.1f)
            {
                Velocity1.X *= -1.0f;
            }
        }
    }

    public class FlyingEnemy : asd.TextureObject2D
    {
        asd.Object2D Target;
        int Shoot, Interval;
        asd.Texture2D BulletTexture;
        public FlyingEnemy(asd.Texture2D texture, asd.Object2D target)
        {
            Texture = texture;
            Position = new asd.Vector2DF(0.0f, 32.0f);
            CenterPosition = new asd.Vector2DF((float)Texture.Size.X / 2.0f, (float)Texture.Size.Y / 2.0f);
            BulletTexture = asd.Engine.Graphics.CreateTexture2D("Resources/Characters/enemybullet.png");
            Target = target;
            Shoot = 0;
            Interval = 20;
        }
        protected override void OnUpdate()
        {
            if (TurnLR)
            {
                Position += new asd.Vector2DF(3.0f, 0.0f);
                if (Position.X > Target.Position.X + 300)
                {
                    TurnLR = false;
                    Shoot = 200;
                }
            }
            else
            {
                Position += new asd.Vector2DF(-3.0f, 0.0f);
                if (Position.X < Target.Position.X - 332)
                {
                    TurnLR = true;
                    Shoot = 200;
                }
            }

            //if(Math.Abs(Position.X-Target.Position.Y)<200)
            {
                Shoot--;
                if (Shoot % Interval == 0)
                {
                    asd.Vector2DF pos = Position + new asd.Vector2DF(0, 12);
                    EnemyBullet blt = new EnemyBullet(BulletTexture, pos, (Target.Position - pos).Normal * 3,Target);
                    this.Layer.AddObject(blt);
                }
            }

        }
    }
    public class EnemyBullet : asd.TextureObject2D
    {
        asd.Vector2DF Velocity;
        asd.Object2D Target;
        public EnemyBullet(asd.Texture2D texture, asd.Vector2DF pos, asd.Vector2DF velocity,asd.Object2D target)
        {
            Texture = texture;
            Velocity = velocity;
            Position = pos;
            Target = target;
            Angle = velocity.Degree;
            CenterPosition = new asd.Vector2DF(Texture.Size.X / 2, Texture.Size.Y / 2);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            Position += Velocity;
            asd.Vector2DF pos = Position - Target.Position;
            if (Math.Abs(pos.X) > 250 || Math.Abs(pos.Y) > 170)
            {
                //this.Destroy();
                Vanish();
                //this.Layer.RemoveObject(this);
            }
        }
    }
}
