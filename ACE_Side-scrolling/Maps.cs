using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Side_scrolling
{
    class Maps : ace.MapObject2D
    {
        int length;
        string[] data = new string[15];
        public Maps(int len)
        {

            ace.Texture2D chip_texture = ace.Engine.Graphics.CreateTexture2D("Resources/block.png");

            length = len;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < 480 / 32; j++) data[j] = "00000000000000000000000000000000000";
            }
            data[12] = "11111111111111111111111111111111111";

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < 480 / 32; j++)
                {
                    if (data[j][i] == '1')
                    {
                        ace.Chip2D chip = ace.Engine.Graphics.CreateChip2D();
                        chip.Texture = chip_texture;
                        chip.Src = new ace.RectF(i * 32.0f, j * 32.0f, 32.0f, 32.0f);
                        AddChip(chip);
                    }
                }
            }

        }
    }
}
