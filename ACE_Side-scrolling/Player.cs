using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Side_scrolling
{

    public class Player : ace.TextureObject2D
    {
        private int Anime;
        private ace.Texture2D[] Bouningen = new ace.Texture2D[6];
        private float VSpeed;
        private bool VMove;
        private Maps Map;

        public Player(Maps _map)
        {
            for (int i = 0; i < 6; i++)
            {
                Bouningen[i] = ace.Engine.Graphics.CreateTexture2D("Resources/W" + i.ToString() + ".png");
            }
            Texture = Bouningen[0];
            Anime = 0;
            Position = new ace.Vector2DF(300.0f, 240.0f);
            VSpeed = 0.0f;
            VMove = false;
            Map = _map;
        }

        private ace.Vector2DI ConvertPos(ace.Vector2DF pos)
        {
            return new ace.Vector2DI((int)pos.X / 32, (int)pos.Y / 32);
        }

        protected override void OnUpdate()
        {
            ace.Vector2DI Pos = ConvertPos(Position);
            if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Left) == ace.KeyState.Hold)//左移動
            {
                if (!Map.Isblocked(Position + new ace.Vector2DF(-2.0f, 0.0f) + new ace.Vector2DF(5.0f, 0.0f)) &&
                    !Map.Isblocked(Position + new ace.Vector2DF(-2.0f, 0.0f) + new ace.Vector2DF(5.0f, 31.0f)))
                {
                    Position += new ace.Vector2DF(-2.0f, 0.0f);
                    Anime++;
                    TurnLR = false;
                }
            }
            if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Right) == ace.KeyState.Hold)//右移動
            {
                if (!Map.Isblocked(Position + new ace.Vector2DF(2.0f, 0.0f) + new ace.Vector2DF(18.0f, 0.0f)) &&
                    !Map.Isblocked(Position + new ace.Vector2DF(2.0f, 0.0f) + new ace.Vector2DF(18.0f, 31.0f)))
                {
                    Position += new ace.Vector2DF(2.0f, 0.0f);
                    Anime++;
                    TurnLR = true;
                }
            }
            if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Up) == ace.KeyState.Hold && !VMove)//ジャンプ
            {
                VSpeed = -4.0f;
            }

            if (VSpeed < 0.0f)//上昇中
            {
                if (!Map.Isblocked(Position + new ace.Vector2DF(0.0f, VSpeed) + new ace.Vector2DF(5.0f, 0.0f)) &&
                   !Map.Isblocked(Position + new ace.Vector2DF(0.0f, VSpeed) + new ace.Vector2DF(18.0f, 0.0f)))
                {
                    Position += new ace.Vector2DF(0.0f, VSpeed);
                    VSpeed += 0.2f;
                    VMove = true;
                }
                else VSpeed = 0.0f;
            }
            else//落下中
            {
                if (!Map.Isblocked(Position + new ace.Vector2DF(0.0f, VSpeed) + new ace.Vector2DF(5.0f, 31.0f)) &&
                   !Map.Isblocked(Position + new ace.Vector2DF(0.0f, VSpeed) + new ace.Vector2DF(18.0f, 31.0f)))
                {
                    Position += new ace.Vector2DF(0.0f, VSpeed);
                    VSpeed = Math.Min(4.0f, VSpeed + 0.2f);
                    if (!Map.Isblocked(Position + new ace.Vector2DF(0.0f, VSpeed) + new ace.Vector2DF(5.0f, 31.0f)) &&
                   !Map.Isblocked(Position + new ace.Vector2DF(0.0f, VSpeed) + new ace.Vector2DF(18.0f, 31.0f))) VMove = true;
                }
                else
                {
                    //VSpeed = 0.0f;
                    while (Map.Isblocked(Position + new ace.Vector2DF(0.0f, VSpeed) + new ace.Vector2DF(5.0f, 31.0f)) ||
                   Map.Isblocked(Position + new ace.Vector2DF(0.0f, VSpeed) + new ace.Vector2DF(18.0f, 31.0f))) VSpeed -= 0.1f;
                    VMove = false;
                }
            }

            if (VMove) Anime = 0;
            Texture = (VMove ? Bouningen[5] : Bouningen[(Anime / 4) % 4]);
            Console.WriteLine(VSpeed);
        }
    }
}
