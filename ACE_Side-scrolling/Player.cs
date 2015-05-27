using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Side_scrolling
{
    public class Player : ace.TextureObject2D
    {
        int count = 0, anime = 0, status = 0;
        float Vspeed = 0;
        bool Xjump;
        ace.Texture2D[] bouningen = new ace.Texture2D[6];
        ace.Vector2DF pos;
        Maps map;

        public Player(Maps _map)
        {
            for (int i = 0; i < 4; i++)
            {
                bouningen[i] = ace.Engine.Graphics.CreateTexture2D("Resources/W" + i.ToString() + ".png");
            }
            for (int i = 0; i < 2; i++)
            {
                bouningen[i + 4] = ace.Engine.Graphics.CreateTexture2D("Resources/J" + i.ToString() + ".png");
            }
            Texture = bouningen[0];
            Position = pos = new ace.Vector2DF(320.0f, 240.0f);
            map = _map;
        }

        protected override void OnUpdate()
        {
            if (status == 0)
            {
                Texture = bouningen[anime % 4];
                if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Left) == ace.KeyState.Hold)
                {
                    if (count % 4 == 0) anime++;
                    pos.X--;
                    TurnLR = false;
                }
                if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Right) == ace.KeyState.Hold)
                {
                    if (count % 4 == 0) anime++;
                    pos.X++;
                    TurnLR = true;
                }

                if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Up) == ace.KeyState.Push)
                {
                    status = 1;
                    anime = 0;
                    Vspeed = 4.0f;

                    if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Left) == ace.KeyState.Hold && !TurnLR) Xjump = true;
                    else if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Right) == ace.KeyState.Hold && TurnLR) Xjump = true;
                    else Xjump = false;
                }
            }

            if (status == 1)
            {
                if (Xjump)
                {
                    if (TurnLR) pos.X++;
                    else pos.X--;

                    if (Vspeed > 2.0f) Texture = bouningen[0];
                    else if (Vspeed > 0.0f) Texture = bouningen[1];
                    else if (Vspeed > -2.0f) Texture = bouningen[2];
                    else Texture = bouningen[3];
                }
                else
                {
                    Texture = bouningen[4];
                }
            }

            if (!IsEnterable() && Vspeed < 0.0f)
            {
                Vspeed = 0.0f;
                status = 0;
            }
            else
            {
                pos.Y -= Vspeed;
                Vspeed -= 0.2f;
            }
            Position = pos;
            count++;

        }
        public bool IsEnterable()
        {
            int x = ((int)pos.X + 16) / 32;
            int y = (int)pos.Y / 32 + 1;
            return !(map.data[y][x] == '1');
        }
    }
}
