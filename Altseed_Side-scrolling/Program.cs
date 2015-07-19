using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Altseed_Side_scrolling
{
    class FPSViewer : asd.TextObject2D
    {
        public FPSViewer()
        {
            Font = asd.Engine.Graphics.CreateFont("Resources/FPS_font.aff");
            Position = new asd.Vector2DF(0.0f, 0.0f);
        }
        protected override void OnUpdate()
        {
            Text = asd.Engine.CurrentFPS.ToString("##.#") + "FPS";
        }
    }

    class Camera : asd.CameraObject2D
    {
        asd.Object2D Target;
        public Camera(asd.Object2D target)
        {
            Target = target;
            Dst = new asd.RectI(0, 0, 640, 480);
        }
        protected override void OnUpdate()
        {
            Src = new asd.RectI((int)Target.Position.X - 320, 0, 640, 480);
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // AC-Engineを初期化する。
            asd.Engine.Initialize("Empty", 640, 480, new asd.EngineOption());

            asd.Scene scene = new asd.Scene();

            asd.Layer2D backlayer = new asd.Layer2D();
            asd.TextureObject2D backG = new asd.TextureObject2D();
            backG.Texture = asd.Engine.Graphics.CreateTexture2D("Resources/back.png");
            backlayer.DrawingPriority = 0;
            backlayer.AddObject(backG);
            scene.AddLayer(backlayer);

            asd.Layer2D blocklayer = new asd.Layer2D();
            blocklayer.DrawingPriority = 1;
            scene.AddLayer(blocklayer);
            Maps map = new Maps(30);
            blocklayer.AddObject(map);

            asd.Layer2D layer = new asd.Layer2D();
            Player player = new Player(map);
            player.DrawingPriority = 65536;
            Enemy train1 = new Enemy("Resources/Characters/train.png", new asd.Vector2DF(22.0f * 32.0f, 100.0f), map);
            layer.DrawingPriority = 2;
            layer.AddObject(player);
            layer.AddObject(train1);
            scene.AddLayer(layer);

            asd.Layer2D UIlayer = new asd.Layer2D();
            UIlayer.DrawingPriority = 3;
            scene.AddLayer(UIlayer);

            FPSViewer fps = new FPSViewer();
            UIlayer.AddObject(fps);


            Camera Cam;
            Cam = new Camera(player);
            layer.AddObject(Cam);
            Cam = new Camera(player);
            blocklayer.AddObject(Cam);

            asd.Engine.ChangeScene(scene);

            // AC-Engineが進行可能かチェックする。
            while (asd.Engine.DoEvents())
            {
                // AC-Engineを更新する。
                asd.Engine.Update();
            }

            // AC-Engineを終了する。
            asd.Engine.Terminate();
        }
    }

}