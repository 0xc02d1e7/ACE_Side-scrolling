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
            DrawingPriority = 2;
        }

        protected override void OnUpdate()
        {
            Movement = new asd.Vector2DF(0.0f, 0.0f);

            if (Velocity1.X + Velocity2.X < 0.0f)//左移動
            {
                if (!Map.Isblocked(Position + new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f) + new asd.Vector2DF(-Width / 2.0f, -Height / 2.0f + 1.0f)) &&
                    !Map.Isblocked(Position + new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f) + new asd.Vector2DF(-Width / 2.0f, Height / 2.0f)))
                {
                    Position += new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f);
                    Movement += new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f);
                    TurnLR = false;
                }
            }
            else if (Velocity1.X + Velocity2.X > 0.0f)//右移動
            {
                if (!Map.Isblocked(Position + new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f) + new asd.Vector2DF(Width / 2.0f, -Height / 2.0f + 1.0f)) &&
                    !Map.Isblocked(Position + new asd.Vector2DF(Velocity1.X + Velocity2.X, 0.0f) + new asd.Vector2DF(Width / 2.0f, Height / 2.0f)))
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
                if (!Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(-Width / 2.0f, Height / 2.0f)) &&
                   !Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(Width / 2.0f, Height / 2.0f)))
                {
                    Position += new asd.Vector2DF(0.0f, Velocity1.Y);
                    Velocity1.Y = Math.Min(4.0f, Velocity1.Y + 0.2f);
                    if (!Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(-Width / 2.0f, Height / 2.0f)) &&
                   !Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(Width / 2.0f, Height / 2.0f)))
                    {
                        Movement += new asd.Vector2DF(0.0f, Velocity1.Y);
                    }
                }
                else
                {
                    //Velocity.Y = 0.0f;
                    while (Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(-Width / 2.0f, Height / 2.0f)) ||
                   Map.Isblocked(Position + new asd.Vector2DF(0.0f, Velocity1.Y) + new asd.Vector2DF(Width / 2.0f, Height / 2.0f))) Velocity1.Y -= 0.1f;
                }
            }
            Velocity2 = new asd.Vector2DF(0.0f, 0.0f);


            //敵との衝突検出
            IEnumerable<asd.Object2D> enemies = this.Layer.Objects;
            foreach (asd.Object2D obj in enemies)
            {
                if (obj == this) continue;
                if ((obj as Character) != null)
                {
                    asd.Vector2DF d;
                    d = IsCollide((Character)obj);
                    if (d.Y > 0.0f)
                    {
                        OnCollide((Character)obj, d);
                    }
                }
                else if ((obj as EnemyBullet) != null)
                {
                    if (IsCollide((EnemyBullet)obj))
                    {
                        OnCollide((EnemyBullet)obj);
                    }
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

        protected bool IsCollide(EnemyBullet obj)
        {
            float Radius2 = (Width > Height ? Height : Width) / 2 + 6;
            Radius2 *= Radius2;

            if ((Position - obj.Position).SquaredLength < Radius2) return true;
            return false;
        }

        protected virtual void OnCollide(Character obj, asd.Vector2DF d)
        {

        }

        protected virtual void OnCollide(EnemyBullet obj)
        {

        }
    }

    public class Player : Character
    {
        private asd.Texture2D[] Bouningen = new asd.Texture2D[7];
        public int KillFlag;

        public Player(Maps _map)
            : base(_map, 13.0f, 32.0f, new asd.Vector2DF(256.0f, 100.0f))
        {
            for (int i = 0; i < 7; i++)
            {
                Bouningen[i] = asd.Engine.Graphics.CreateTexture2D("Resources/Characters/W" + i.ToString() + ".png");
            }
            Texture = Bouningen[0];
            CenterPosition = new asd.Vector2DF((float)Texture.Size.X / 2.0f, (float)Texture.Size.Y / 2.0f);
            KillFlag = -1;
            DrawingPriority = 65536;
        }

        protected override void OnUpdate()
        {
            if (KillFlag == -1)
            {
                if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Left) == asd.KeyState.Hold)//左移動
                {
                    Velocity1.X = -2.0f;
                    Anime++;
                }
                else if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Right) == asd.KeyState.Hold)//右移動
                {
                    Velocity1.X = 2.0f;
                    Anime++;
                }
                else Velocity1.X = 0.0f;

                if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Up) == asd.KeyState.Push && Math.Abs(Movement.Y) < 0.1f)//ジャンプ
                {
                    Velocity1.Y = -4.0f;
                    Sound.SEPlay(0);
                }

                base.OnUpdate();

                if (Math.Abs(Movement.Y) > 0.1f)
                {
                    Texture = Bouningen[5];
                    Anime = 0;
                }
                else Texture = Bouningen[(Anime / 4) % 4];
                if (Position.Y > 330.0f) KillFlag = 0;
            }
            else
            {
                if (KillFlag == 0) Sound.SEPlay(1);
                Texture = Bouningen[6];
                Position += new asd.Vector2DF((TurnLR ? -2.0f : 2.0f), (KillFlag - 30) / 5);
                KillFlag++;
                if (Position.Y > 330.0) asd.Engine.ChangeScene(new DeadScene(Map.StageCode));
            }
        }

        protected override void OnCollide(Character obj, asd.Vector2DF d)
        {
            Enemy e = (Enemy)obj;

            if (d.X > d.Y)
            {
                if (e.motal == 0)
                {
                    Movement.Y = 0.0f;
                    Velocity2 = obj.Velocity1;
                    Velocity1.Y = 0.0f;
                }
            }
            else if (d.X < d.Y && d.X > 1.0f)
            {
                KillFlag = 0;
                TurnLR = (e.Position.X > this.Position.X);
            }
        }

        protected override void OnCollide(EnemyBullet obj)
        {
            KillFlag = 0;
            TurnLR = (obj.Position.X > this.Position.X);
        }
    }

    public class Enemy : Character
    {
        public int motal;

        public Enemy(asd.Texture2D texture, asd.Vector2DF pos, Maps map)
            : base(map, 32.0f, 32.0f, pos)
        {
            Texture = texture;
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
        Maps Map;

        public FlyingEnemy(asd.Texture2D texture, asd.Object2D target, Maps map)
        {
            Texture = texture;
            Position = new asd.Vector2DF(0.0f, 32.0f);
            CenterPosition = new asd.Vector2DF((float)Texture.Size.X / 2.0f, (float)Texture.Size.Y / 2.0f);
            BulletTexture = asd.Engine.Graphics.CreateTexture2D("Resources/Characters/enemybullet.png");
            Target = target;
            Map = map;
            Shoot = 0;
            Interval = 30;
            DrawingPriority = 2;
        }

        protected override void OnUpdate()
        {
            if (TurnLR)
            {
                Position += new asd.Vector2DF(3.0f, 0.0f);
                if (Position.X > Target.Position.X + 300)
                {
                    TurnLR = false;
                }
            }
            else
            {
                Position += new asd.Vector2DF(-3.0f, 0.0f);
                if (Position.X < Target.Position.X - 332)
                {
                    TurnLR = true;
                }
            }

            Shoot++;
            if (Shoot % Interval == 0)
            {
                if (TurnLR ^ Position.X > Target.Position.X)
                {
                    asd.Vector2DF pos = Position + new asd.Vector2DF(0, 12);
                    EnemyBullet blt = new EnemyBullet(BulletTexture, pos, (Target.Position - pos).Normal * 3, Target, Map);
                    this.Layer.AddObject(blt);
                }
            }
        }
    }

    public class EnemyBullet : asd.TextureObject2D
    {
        asd.Vector2DF Velocity;
        asd.Object2D Target;
        Maps Map;

        public EnemyBullet(asd.Texture2D texture, asd.Vector2DF pos, asd.Vector2DF velocity, asd.Object2D target, Maps map)
        {
            Texture = texture;
            Position = pos;
            Velocity = velocity;
            Target = target;
            Map = map;

            Angle = velocity.Degree;
            CenterPosition = new asd.Vector2DF(Texture.Size.X / 2, Texture.Size.Y / 2);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            Position += Velocity;
            asd.Vector2DF pos = Position - Target.Position;
            if (Math.Abs(pos.X) > 480 || Math.Abs(pos.Y) > 320 || Map.Isblocked(Position))
            {
                Vanish();
            }
        }
    }
}
