using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Side_scrolling
{

    public class Player : ace.TextureObject2D
    {
        private int anime;
        private ace.Texture2D[] bouningen = new ace.Texture2D[6];

        private Maps map;
        private ace.Vector2DF Velocity;

        public Player(Maps _map)
        {
            for (int i = 0; i < 6; i++)
            {
                bouningen[i] = ace.Engine.Graphics.CreateTexture2D("Resources/W" + i.ToString() + ".png");
            }
            Texture = bouningen[0];
            Position = new ace.Vector2DF(320.0f, 240.0f);
            map = _map;
            anime = 0;
            Velocity = new ace.Vector2DF(0.0f, 0.0f);
            CenterPosition = new ace.Vector2DF(12, 16);
        }

        protected override void OnUpdate()
        {
            //if (Velocity.Y == 0.0f)//浮遊中でなければX軸方向に移動できる。
            if (Velocity.Y == 0.0f)
            {
                Velocity.X = 0.0f;
                if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Up) == ace.KeyState.Hold)
                {
                    Texture = bouningen[4];
                }
                else
                {
                    if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Left) == ace.KeyState.Hold)
                    {
                        anime++;
                        Velocity.X = -1.0f;
                        TurnLR = false;
                    }
                    if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Right) == ace.KeyState.Hold)
                    {
                        anime++;
                        Velocity.X = 1.0f;
                        TurnLR = true;
                    }
                    Texture = bouningen[(anime / 5) % 4];
                    Velocity.Y = 0.2f;//落下しようとしてみる。
                }

                if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Up) == ace.KeyState.Release)
                {
                    Velocity.Y = -4.0f;
                    anime = 0;
                }

            }
            else
            {
                Velocity.Y = Math.Min(Velocity.Y+0.2f, 4.0f);
                Texture = bouningen[5];
            }

            RegulateVelocity();
            Position += Velocity;
        }

        private void RegulateVelocity()//そのまま突き進んでも壁に当たらないよう速度を調整する
        {
            ace.Vector2DF ExpectedPosition = Position+Velocity;

            if (Velocity.Y > 0.0)//下方向
            {
                ace.Vector2DI cell = new ace.Vector2DI((int)ExpectedPosition.X / 16, (int)(ExpectedPosition.Y + 16.0f) / 16);
                if (map.data[cell.Y][cell.X] == '1') Velocity.Y = 0.0f;
            }
            else if (Velocity.Y < 0.0)//上方向
            {
                ace.Vector2DI cell = new ace.Vector2DI((int)ExpectedPosition.X / 16, (int)(ExpectedPosition.Y - 16.0f) / 16);
                if (map.data[cell.Y][cell.X] == '1') Velocity.Y = 0.0f;
            }

            if (Velocity.X < 0.0)//左方向
            {
                ace.Vector2DI cell = new ace.Vector2DI((int)(ExpectedPosition.X - 6.0f) / 16, (int)ExpectedPosition.Y / 16);
                if (map.data[cell.Y][cell.X] == '1') Velocity.X = 0.0f;
            }
            else if (Velocity.X > 0.0)//右方向
            {
                ace.Vector2DI cell = new ace.Vector2DI((int)(ExpectedPosition.X + 12.0f) / 16, (int)ExpectedPosition.Y / 16);
                if (map.data[cell.Y][cell.X] == '1') Velocity.X = 0.0f;
            }

        }
    }
}
