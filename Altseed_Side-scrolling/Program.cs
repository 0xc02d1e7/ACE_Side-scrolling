using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Altseed_Side_scrolling
{

    static public class FontContainer
    {
        static public asd.Font PMP10_20;
        static public asd.Font PMP12_36;

        static FontContainer()
        {
            PMP10_20 = asd.Engine.Graphics.CreateDynamicFont(@"\Resources\Font\PixelMplus10-Regular.ttf", 30, new asd.Color(0, 0, 0, 255), 0, new asd.Color(0, 0, 0, 0));
            PMP12_36 = asd.Engine.Graphics.CreateDynamicFont(@"\Resources\Font\PixelMplus12-Regular.ttf", 60, new asd.Color(0, 0, 0, 255), 0, new asd.Color(0, 0, 0, 0));

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
        protected asd.Object2D Target;
        asd.Texture2D Texture;
        asd.Chip2D[] Chip;
        public Background(asd.Object2D target)
        {
            Target = target;
            Texture = asd.Engine.Graphics.CreateTexture2D("Resources/UI/back.png");
            Chip = new asd.Chip2D[3];
            for (int i = 0; i < 1; i++)
            {
                Chip[i] = new asd.Chip2D();
                Chip[i].Texture = Texture;
                this.AddChip(Chip[i]);
            }
        }
        protected override void OnUpdate()
        {
            int pos = (int)Target.Position.X;
            for (int i = 0; i < 1; i++)
            {
                Chip[i].Position = new asd.Vector2DF((int)(pos % 600)*0.8f + (int)(pos / 600) * 480.0f, 0.0f);
                System.Console.WriteLine("{0}:{1}+{2}={3}", pos, (int)(pos % 600) * 0.8f, (int)(pos / 600) * 480.0f, (int)(pos % 600) * 0.8f + (int)(pos / 600) * 480.0f);
            }
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // AC-Engineを初期化する。
            asd.Engine.Initialize("Empty", 960, 640, new asd.EngineOption());


            asd.Scene Stitle = new asd.Scene();
            asd.Layer2D Ltitle = new asd.Layer2D();

            asd.TextureObject2D Gtitleback = new asd.TextureObject2D();
            Gtitleback.Texture = asd.Engine.Graphics.CreateTexture2D("Resources/UI/back.png");
            asd.TextureObject2D Gtitlelogo = new asd.TextureObject2D();
            Gtitlelogo.Texture = asd.Engine.Graphics.CreateTexture2D("Resources/UI/logo.png");
            Gtitlelogo.Position = new asd.Vector2DF((asd.Engine.WindowSize.X - Gtitlelogo.Texture.Size.X) / 2.0f, 50.0f);
            Ltitle.AddObject(Gtitleback);
            Ltitle.AddObject(Gtitlelogo);
            Stitle.AddLayer(Ltitle);


            asd.Scene Sgame = new asd.Scene();

            asd.Layer2D Lblock = new asd.Layer2D();
            Lblock.DrawingPriority = 1;
            Sgame.AddLayer(Lblock);
            Maps map = new Maps();
            Lblock.AddObject(map);

            asd.Layer2D Lchar = new asd.Layer2D();
            Player player = new Player(map);
            player.DrawingPriority = 65536;
            Enemy train1 = new Enemy("Resources/Characters/trainL.png", new asd.Vector2DF(22.0f * 32.0f, 0.0f), map);
            Enemy train2 = new Enemy("Resources/Characters/truck.png", new asd.Vector2DF(10.0f * 32.0f, 0.0f), map);
            FlyingEnemy heli = new FlyingEnemy(asd.Engine.Graphics.CreateTexture2D("Resources/Characters/heli.png"),player);
            Lchar.DrawingPriority = 2;
            Lchar.AddObject(player);
            Lchar.AddObject(heli);
            //Lchar.AddObject(train1);
            //Lchar.AddObject(train2);
            Sgame.AddLayer(Lchar);

            asd.Layer2D Lback = new asd.Layer2D();
            Background Gbacks = new Background(player);
            Lback.DrawingPriority = 0;
            Lback.AddObject(Gbacks);
            Sgame.AddLayer(Lback);

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
            Cam = new Camera(player);
            Lback.AddObject(Cam);


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