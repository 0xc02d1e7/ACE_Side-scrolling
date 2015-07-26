using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed_Side_scrolling
{
    public class FPSViewer : asd.TextObject2D
    {
        public FPSViewer()
        {
            Font = FontContainer.PMP10_20;
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
            Font = FontContainer.PMP10_20;
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
            Text = "TIME: "+(time/3600).ToString("D2")+":"+((time / 60)%60).ToString("D2")+":"+(time % 60).ToString("D2");
        }
    }
}
