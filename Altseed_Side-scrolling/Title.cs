using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed_Side_scrolling
{
    public class Dialog : asd.Scene
    {
        int cursor;
        public Dialog()
        {
            cursor = 0;
        }

        protected override void OnUpdating()
        {
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Up) == asd.KeyState.Push)
            {
                cursor--;
            }
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Down) == asd.KeyState.Push)
            {
                cursor--;
            }

        }
    }
}
