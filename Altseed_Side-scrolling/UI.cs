using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed_Side_scrolling
{
    static public class FontContainer
    {
        static public asd.Font PMP10_30B;
        static public asd.Font PMP12_60B;
        static public asd.Font PMP12_60W;

        static FontContainer()
        {
            PMP10_30B = asd.Engine.Graphics.CreateDynamicFont("Font/PixelMplus10-Regular.ttf", 30, new asd.Color(0, 0, 0, 255), 0, new asd.Color(0, 0, 0, 0));
            PMP12_60B = asd.Engine.Graphics.CreateDynamicFont("Font/PixelMplus12-Regular.ttf", 60, new asd.Color(0, 0, 0, 255), 0, new asd.Color(0, 0, 0, 0));
            PMP12_60W = asd.Engine.Graphics.CreateDynamicFont("Font/PixelMplus12-Regular.ttf", 60, new asd.Color(255, 255, 255, 255), 0, new asd.Color(0, 0, 0, 0));

        }
    }

    class Camera : asd.CameraObject2D
    {
        private asd.Object2D Target;

        public Camera(asd.Object2D target)
        {
            Target = target;
            Dst = new asd.RectI(0, 0, 960, 640);
        }

        protected override void OnUpdate()
        {
            Src = new asd.RectI((int)Target.Position.X - 240, 0, 480, 320);
        }
    }

    class Background : asd.MapObject2D
    {
        private asd.Texture2D Texture;

        public Background(int width)
        {
            Texture = asd.Engine.Graphics.CreateTexture2D("UI/back.png");
            int length = (int)(width / 0.8f / Texture.Size.X) + 4;
            for (int i = 0; i < length; i++)
            {
                asd.Chip2D chip = new asd.Chip2D();
                chip = new asd.Chip2D();
                chip.Texture = Texture;
                chip.Position = new asd.Vector2DF((i - 2) * Texture.Size.X, 0.0f);
                AddChip(chip);
            }
        }
    }

    class BackgroundCamera : asd.CameraObject2D
    {
        private asd.Object2D Target;

        public BackgroundCamera(asd.Object2D target)
        {
            Target = target;
            Dst = new asd.RectI(0, 0, 960, 640);
        }

        protected override void OnUpdate()
        {
            Src = new asd.RectI((int)(Target.Position.X * 0.8) - 240, 0, 480, 320);
        }
    }

    public class BlinkingText : asd.TextObject2D
    {
        private int Count, Selecter;
        private int Interval;
        private string Text1, Text2;

        public BlinkingText(int interval, string text1, string text2)
        {
            Interval = interval;
            Text1 = text1;
            Text2 = text2;
            Count = 0;
            Selecter = 0;
        }

        protected override void OnUpdate()
        {
            if (Count % Interval == 0)
            {
                Selecter++;
                Text = (Selecter % 2 == 0 ? Text1 : Text2);
                asd.Vector2DI fsize = FontContainer.PMP10_30B.CalcTextureSize(Text, asd.WritingDirection.Horizontal);
                CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            }
            Count++;
        }
    }
}
