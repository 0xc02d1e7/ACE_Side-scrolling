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
        int Vspeed = 0;
        ace.Texture2D[] bouningen = new ace.Texture2D[6];
        ace.Vector2DF pos;

        public Player()
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

                if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Up) == ace.KeyState.Hold)
                {
                    status = 1;
                    anime = 0;
                    Texture = bouningen[4];
                }
            }
            else if (status == 1 && count % 5 == 0)
            {
                status = 2;
                Vspeed = 10;
                Texture = bouningen[5];
            }

            if (status == 2)
            {
                if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Left) == ace.KeyState.Hold && TurnLR == false)
                {
                    pos.X--;
                }
                if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Left) == ace.KeyState.Hold && TurnLR == true)
                {
                    pos.X++;
                }
            }

            if (pos.Y > 240.0 && Vspeed < 0)//床に触っているかの判定で置き換える
            {
                status = 0;
                Vspeed = 0;
            }
            else
            {
                pos.Y -= Vspeed;
                Vspeed -= 1;
            }

            Position = pos;
            count++;

        }
    }
}
