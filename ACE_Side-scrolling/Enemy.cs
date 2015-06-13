using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Side_scrolling
{
    public class Enemy : ace.TextureObject2D
    {
        float HSpeed, VSpeed;
        Maps Map;

        public Enemy(string path, ace.Vector2DF pos, Maps map)
        {
            Texture = ace.Engine.Graphics.CreateTexture2D(path);
            Position = pos;
            HSpeed = 2.0f;
            VSpeed = 0.0f;
            Map = map;
        }
        protected override void OnUpdate()
        {
            if (HSpeed < 0.0f)//左移動
            {
                if (!Map.Isblocked(Position + new ace.Vector2DF(HSpeed, 0.0f) + new ace.Vector2DF(0.0f, 0.0f)) &&
                    !Map.Isblocked(Position + new ace.Vector2DF(HSpeed, 0.0f) + new ace.Vector2DF(0.0f, 31.0f)))
                {
                    Position += new ace.Vector2DF(HSpeed, 0.0f);
                    TurnLR = false;
                }
                else HSpeed *= -1.0f;
            }
            if (HSpeed > 0.0f)//右移動
            {
                if (!Map.Isblocked(Position + new ace.Vector2DF(HSpeed, 0.0f) + new ace.Vector2DF(31.0f, 0.0f)) &&
                    !Map.Isblocked(Position + new ace.Vector2DF(HSpeed, 0.0f) + new ace.Vector2DF(31.0f, 31.0f)))
                {
                    Position += new ace.Vector2DF(HSpeed, 0.0f);
                    TurnLR = true;
                }
                else HSpeed *= -1.0f;
            }

            VSpeed = 4.0f;
            if (!Map.Isblocked(Position + new ace.Vector2DF(0.0f, VSpeed) + new ace.Vector2DF(0.0f, 31.0f)) &&
                   !Map.Isblocked(Position + new ace.Vector2DF(0.0f, VSpeed) + new ace.Vector2DF(31.0f, 31.0f)))
            {
                Position += new ace.Vector2DF(0.0f, VSpeed);
                VSpeed = Math.Min(4.0f, VSpeed + 0.2f);
            }
            else
            {
                while (Map.Isblocked(Position + new ace.Vector2DF(0.0f, VSpeed) + new ace.Vector2DF(0.0f, 31.0f)) ||
               Map.Isblocked(Position + new ace.Vector2DF(0.0f, VSpeed) + new ace.Vector2DF(31.0f, 31.0f))) VSpeed -= 0.1f;
            }
        }
    }
}
