using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed_Side_scrolling
{
    public class TitleLogo : asd.TextureObject2D
    {
        public TitleLogo(String path)
        {
            Texture = asd.Engine.Graphics.CreateTexture2D(path);
            Position = new asd.Vector2DF((asd.Engine.WindowSize.X - Texture.Size.X) / 2.0f, 50.0f);
        }

        protected override void OnUpdate()
        {

        }
    }
}
