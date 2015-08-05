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
            PMP10_30B = asd.Engine.Graphics.CreateDynamicFont(@"\Resources\Font\PixelMplus10-Regular.ttf", 30, new asd.Color(0, 0, 0, 255), 0, new asd.Color(0, 0, 0, 0));
            PMP12_60B = asd.Engine.Graphics.CreateDynamicFont(@"\Resources\Font\PixelMplus12-Regular.ttf", 60, new asd.Color(0, 0, 0, 255), 0, new asd.Color(0, 0, 0, 0));
            PMP12_60W = asd.Engine.Graphics.CreateDynamicFont(@"\Resources\Font\PixelMplus12-Regular.ttf", 60, new asd.Color(255, 255, 255, 255), 0, new asd.Color(0, 0, 0, 0));

        }
    }

    class Camera : asd.CameraObject2D
    {
        protected asd.Object2D Target;
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
        asd.Texture2D Texture;
        asd.Chip2D[] Chip;
        public Background(int size)
        {
            Texture = asd.Engine.Graphics.CreateTexture2D("Resources/UI/back.png");
            int length = (int)(size / 0.8f / Texture.Size.X) + 4;
            Chip = new asd.Chip2D[length];
            for (int i = 0; i < length; i++)
            {
                Chip[i] = new asd.Chip2D();
                Chip[i].Texture = Texture;
                Chip[i].Position = new asd.Vector2DF((i - 2) * Texture.Size.X, 0.0f);
                this.AddChip(Chip[i]);
            }
        }
    }

    class BackgroundCamera : asd.CameraObject2D
    {
        asd.Object2D Target;
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
        public FPSViewer()
        {
            Font = FontContainer.PMP10_30B;
            Position = new asd.Vector2DF(0.0f, 0.0f);
        }
        protected override void OnUpdate()
        {
            Text = asd.Engine.CurrentFPS.ToString("F1") + "FPS";
        }
    }

    public class TimeCounter : asd.TextObject2D
    {
        protected bool counting;
        protected long time;

        public TimeCounter()
        {
            time = 0;
            counting = false;
            Font = FontContainer.PMP10_30B;
            Position = new asd.Vector2DF(300.0f, 0.0f);
        }

        public void Start()
        {
            counting = true;
        }
        public void Stop()
        {
            counting = false;
        }
        public void Reset()
        {
            time = 0;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (counting) time++;
            Text = "TIME: " + (time / 3600).ToString("D2") + ":" + ((time / 60) % 60).ToString("D2") + ":" + (time % 60).ToString("D2");
        }
    }

    public class BlinkingText : asd.TextObject2D
    {
        private int c, s;
        private int Interval;
        public string Text1, Text2;
        public BlinkingText(int interval)
        {
            Interval = interval;
            c = 0;
            s = 0;
        }
        protected override void OnUpdate()
        {
            c++;
            if (c % Interval == 0)
            {
                this.IsDrawn = !this.IsDrawn;
                if (this.IsDrawn) s++;
                this.Text = (s % 2 == 0 ? Text1 : Text2);
                asd.Vector2DI fsize = FontContainer.PMP10_30B.CalcTextureSize(this.Text, asd.WritingDirection.Horizontal);
                this.CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;

            }
        }
    }
}
