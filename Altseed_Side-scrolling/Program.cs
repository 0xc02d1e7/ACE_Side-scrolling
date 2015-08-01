using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Altseed_Side_scrolling
{


    class Altseed_Side_scrolling_Core
    {
        [STAThread]
        static void Main(string[] args)
        {
            // AC-Engineを初期化する。
            asd.Engine.Initialize("Empty", 960, 640, new asd.EngineOption());

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