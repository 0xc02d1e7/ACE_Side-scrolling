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

            ace.Scene scene =new ace.Scene();

            ace.Layer2D layer=new ace.Layer2D();
            Player player = new Player();
            layer.DrawingPriority = 2;
            layer.AddObject(player);
            scene.AddLayer(layer);

            ace.Layer2D backlayer = new ace.Layer2D();
            ace.TextureObject2D back = new ace.TextureObject2D();
            back.Texture = ace.Engine.Graphics.CreateTexture2D("Resources/back.png");
            backlayer.AddObject(back);
            scene.AddLayer(backlayer);

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