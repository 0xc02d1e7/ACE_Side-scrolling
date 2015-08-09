using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Altseed_Side_scrolling
{

    public static class Altseed_Side_scrolling_Core
    {
        public static String Title = "ブツを運ぶやつ";
        public static int MapCount = 1;

        [STAThread]
        static void Main(string[] args)
        {
            // AC-Engineを初期化する。
            asd.Engine.Initialize(Title, 960, 640, new asd.EngineOption());

            asd.Engine.File.AddRootPackageWithPassword("Resources.pack","dragdrug");

            asd.Engine.ChangeScene(new TitleScene());

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