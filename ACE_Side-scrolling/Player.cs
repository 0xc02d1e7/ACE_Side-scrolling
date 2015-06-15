using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Side_scrolling
{

    public class Player : Character
    {
        private ace.Texture2D[] Bouningen = new ace.Texture2D[6];

        public Player(Maps _map)
            : base(_map, 13.0f, 32.0f, new ace.Vector2DF(100.0f, 100.0f))
        {
            for (int i = 0; i < 6; i++)
            {
                Bouningen[i] = ace.Engine.Graphics.CreateTexture2D("Resources/Characters/W" + i.ToString() + ".png");
            }
            Texture = Bouningen[0];
            CenterPosition = new ace.Vector2DF((float)Texture.Size.X / 2.0f, (float)Texture.Size.Y / 2.0f);

        }

        protected override void OnUpdate()
        {
            if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Left) == ace.KeyState.Hold)//左移動
            {
                Velocity.X = -2.0f;
            }
            else if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Right) == ace.KeyState.Hold)//右移動
            {
                Velocity.X = 2.0f;
            }
            else Velocity.X = 0.0f;
            if (ace.Engine.Keyboard.GetKeyState(ace.Keys.Up) == ace.KeyState.Hold && Math.Abs(Movement.Y) < 0.1f)//ジャンプ
            {
                Velocity.Y = -4.0f;
            }

            Move();

            if (Math.Abs(Movement.Y) > 0.1f)
            {
                Texture = Bouningen[5];
                Anime = 0;
            }
            else Texture = Bouningen[(Anime / 4) % 4];
        }
    }
}
