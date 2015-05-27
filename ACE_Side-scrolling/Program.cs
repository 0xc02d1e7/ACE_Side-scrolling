using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Side_scrolling
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // AC-Engineを初期化する。
            ace.Engine.Initialize("Empty", 640, 480, new ace.EngineOption());

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

            /*
            ace.CameraObject2D camera = new ace.CameraObject2D();
            camera.Dst = new ace.RectI(0, 0, 640, 480);
            camera.Src = new ace.RectI(0, 0, 640, 480);
            layer.AddObject(camera);
            */

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