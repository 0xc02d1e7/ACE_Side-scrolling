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
        public Enemy(string path, ace.Vector2DF pos, Maps map)
            : base(map, 32.0f, 32.0f, pos)
        {
            Texture = ace.Engine.Graphics.CreateTexture2D(path);
            Velocity = new ace.Vector2DF(2.0f, 0.0f);
            CenterPosition = new ace.Vector2DF( (float)Texture.Size.X / 2.0f, (float)Texture.Size.Y / 2.0f);
            motal = 0;
        }

        protected override void OnUpdate()
        {
            Move();
            if (Math.Abs(Movement.X) < 0.1f)
            {
                Velocity.X *= -1.0f;
            }
        }
    }
}
