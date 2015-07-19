using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Altseed_Side_scrolling
{

    static public class FontContainer
    {
        static public asd.Font font;
        static FontContainer()
        {
            //font = asd.Engine.Graphics.CreateDynamicFont(@"\Resources\PixelMplus12-Regular.ttf", 12, new asd.Color(0, 0, 0, 0), 0, new asd.Color(0, 0, 0, 0));
            font = asd.Engine.Graphics.CreateFont("Resources/FPSfont.aff");
        }
    }

    class Camera : asd.CameraObject2D
    {
        asd.Object2D Target;
        public Camera(asd.Object2D target)
        {
            Target = target;
            Dst = new asd.RectI(0, 0, 800, 480);
        }
        protected override void OnUpdate()
        {
            Src = new asd.RectI((int)Target.Position.X - 400, 0, 800, 480);
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // AC-Engineを初期化する。
            asd.Engine.Initialize("Empty", 800, 600, new asd.EngineOption());


            asd.Scene Stitle = new asd.Scene();
            asd.TextureObject2D Gtitleback = new asd.TextureObject2D();
            Gtitleback.Texture = asd.Engine.Graphics.CreateTexture2D("Resources/back.png");


            asd.Scene Sgame = new asd.Scene();
            asd.Layer2D Lback = new asd.Layer2D();
            asd.TextureObject2D Gback = new asd.TextureObject2D();
            Gback.Texture = asd.Engine.Graphics.CreateTexture2D("Resources/back.png");
            Lback.DrawingPriority = 0;
            Lback.AddObject(Gback);
            Sgame.AddLayer(Lback);

            asd.Layer2D Lblock = new asd.Layer2D();
            Lblock.DrawingPriority = 1;
            Sgame.AddLayer(Lblock);
            Maps map = new Maps(30);
            Lblock.AddObject(map);

            asd.Layer2D Lchar = new asd.Layer2D();
            Player player = new Player(map);
            player.DrawingPriority = 65536;
            Enemy train1 = new Enemy("Resources/Characters/train.png", new asd.Vector2DF(22.0f * 32.0f, 100.0f), map);
            Enemy train2 = new Enemy("Resources/Characters/truck.png", new asd.Vector2DF(10.0f * 32.0f, 100.0f), map);
            Lchar.DrawingPriority = 2;
            Lchar.AddObject(player);
            Lchar.AddObject(train1);
            Lchar.AddObject(train2);
            Sgame.AddLayer(Lchar);

            asd.Layer2D Lui = new asd.Layer2D();
            Lui.DrawingPriority = 3;
            Sgame.AddLayer(Lui);

            FPSViewer fps = new FPSViewer();
            Lui.AddObject(fps);
            TimeCounter tc = new TimeCounter();
            Lui.AddObject(tc);
            tc.Start();

            Camera Cam;
            Cam = new Camera(player);
            Lchar.AddObject(Cam);
            Cam = new Camera(player);
            Lblock.AddObject(Cam);

            asd.Engine.ChangeScene(Sgame);

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