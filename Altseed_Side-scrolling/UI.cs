using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed_Side_scrolling
{
    public class FontContainer
    {
        public asd.Font PMP10_30B;
        public asd.Font PMP12_60B;
        public asd.Font PMP12_60W;

        public FontContainer()
        {
            PMP10_30B = asd.Engine.Graphics.CreateDynamicFont(@"\Resources\Font\PixelMplus10-Regular.ttf", 30, new asd.Color(0, 0, 0, 255), 0, new asd.Color(0, 0, 0, 0));
            PMP12_60B = asd.Engine.Graphics.CreateDynamicFont(@"\Resources\Font\PixelMplus12-Regular.ttf", 60, new asd.Color(0, 0, 0, 255), 0, new asd.Color(0, 0, 0, 0));
            PMP12_60W = asd.Engine.Graphics.CreateDynamicFont(@"\Resources\Font\PixelMplus12-Regular.ttf", 60, new asd.Color(255, 255, 255, 255), 0, new asd.Color(0, 0, 0, 0));

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
            Texture = asd.Engine.Graphics.CreateTexture2D("Resources/UI/back.png");
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

    public class FPSViewer : asd.TextObject2D
    {
        FontContainer Fonts;

        public FPSViewer(FontContainer fonts)
        {
            Fonts = fonts;
            Font = Fonts.PMP10_30B;
            Position = new asd.Vector2DF(0.0f, 0.0f);
        }

        protected override void OnUpdate()
        {
            Text = asd.Engine.CurrentFPS.ToString("F1") + "FPS";
        }
    }

    public class TimeCounter : asd.TextObject2D
    {
        private ulong time;
        FontContainer Fonts;

        public TimeCounter(FontContainer fonts)
        {
            time = 0;
            Fonts = fonts;
            Font = Fonts.PMP10_30B;
            Position = new asd.Vector2DF(300.0f, 0.0f);
        }

        protected override void OnUpdate()
        {
            time++;
            Text = "TIME: " + (time / 60).ToString("D3");
        }
    }

    public class BlinkingText : asd.TextObject2D
    {
        private int Count, Selecter;
        private int Interval;
        private string Text1, Text2;
        FontContainer Fonts;

        public BlinkingText(int interval, string text1, string text2,FontContainer fonts)
        {
            Interval = interval;
            Text1 = text1;
            Text2 = text2;
            Count = 0;
            Selecter = 0;
            Fonts = fonts;
        }

        protected override void OnUpdate()
        {
            if (Count % Interval == 0)
            {
                Selecter++;
                Text = (Selecter % 2 == 0 ? Text1 : Text2);
                asd.Vector2DI fsize = Fonts.PMP10_30B.CalcTextureSize(Text, asd.WritingDirection.Horizontal);
                CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            }
            Count++;
        }
    }
}
