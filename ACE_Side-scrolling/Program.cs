using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Test
{
    class Player : ace.TextureObject2D
    {
        int count = 0, anime = 0;
        ace.Texture2D[] bouningen = new ace.Texture2D[4];
        ace.Vector2DF pos;
        public Player()
        {
            for (int i = 0; i < 4; i++)
            {
                bouningen[i] = ace.Engine.Graphics.CreateTexture2D("Resources/W" + i.ToString() + ".png");
            }

            Position = pos = new ace.Vector2DF(320.0f, 240.0f);
        }

        protected override void OnUpdate()
        {
            if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Left) == ace.KeyState.Hold)
            {
                if (count % 5 == 0)
                {
                    anime++;
                    pos.X -= 3;
                    Position = pos;
                    TurnLR = false;
                }

            }
            else if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Right) == ace.KeyState.Hold)
            {
                if (count % 5 == 0)
                {
                    anime++;
                    pos.X += 3;
                    Position = pos;
                    TurnLR = true;
                }

            }
            else
            {
                anime = 0;
            }
            count++;
            Texture = bouningen[anime % 4];
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // AC-Engineを初期化する。
            ace.Engine.Initialize("Empty", 640, 480, new ace.EngineOption());

            Player player = new Player();
            ace.Engine.AddObject2D(player);

            // AC-Engineが進行可能かチェックする。
            while (ace.Engine.DoEvents())
            {
                // AC-Engineを更新する。
                ace.Engine.Update();
            }

            // AC-Engineを終了する。
            ace.Engine.Terminate();
        }
    }

}