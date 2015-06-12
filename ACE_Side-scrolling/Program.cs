using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ace;

namespace ACE_Side_scrolling
{
    class FPSViewer : ace.TextObject2D
    {
        public FPSViewer()
        {
            Font = ace.Engine.Graphics.CreateFont("Resources/FPS_font.aff");
            Position = new Vector2DF(0.0f, 0.0f);
        }
        protected override void OnUpdate()
        {
            Text = ace.Engine.CurrentFPS.ToString() + "FPS";
        }
    }

    class Camera : ace.CameraObject2D
    {
        ace.Object2D Target;
        public Camera(ace.Object2D target)
        {
            Target = target;
            Dst = new ace.RectI(0, 0, 640, 480);
        }
        protected override void OnUpdate()
        {
            Src = new ace.RectI((int)Target.Position.X - 320, 0, 640, 480);
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // AC-Engineを初期化する。
            Engine.Initialize("Empty", 640, 480, new ace.EngineOption());

            ace.Scene scene = new ace.Scene();

            ace.Layer2D backlayer = new ace.Layer2D();
            ace.TextureObject2D backG = new ace.TextureObject2D();
            backG.Texture = ace.Engine.Graphics.CreateTexture2D("Resources/back.png");
            backlayer.DrawingPriority = 0;
            backlayer.AddObject(backG);
            scene.AddLayer(backlayer);

            ace.Layer2D blocklayer = new ace.Layer2D();
            blocklayer.DrawingPriority = 1;
            scene.AddLayer(blocklayer);
            Maps map = new Maps(30);
            blocklayer.AddObject(map);

            ace.Layer2D layer = new ace.Layer2D();
            Player player = new Player(map);
            layer.DrawingPriority = 2;
            layer.AddObject(player);
            scene.AddLayer(layer);

            ace.Layer2D UIlayer = new ace.Layer2D();
            UIlayer.DrawingPriority = 3;
            scene.AddLayer(UIlayer);

            FPSViewer fps = new FPSViewer();
            UIlayer.AddObject(fps);


            Camera Cam;
            Cam = new Camera(player);
            layer.AddObject(Cam);
            Cam = new Camera(player);
            blocklayer.AddObject(Cam);

            ace.Engine.ChangeScene(scene);

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