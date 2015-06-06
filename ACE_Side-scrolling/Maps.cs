using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE_Side_scrolling
{
    public class Maps : ace.MapObject2D
    {
        int length=int.MaxValue;
        public string[] data = new string[30];

        public Maps(int len)
        {

            StreamReader reader = new StreamReader("Maps/01.txt", Encoding.Unicode);
            for (int i = 0; i < 30;i++ )
            {
                data[i] = reader.ReadLine();
                length = Math.Min(length, data[i].Length);
            }
            reader.Close();

            ace.Texture2D chip_texture = ace.Engine.Graphics.CreateTexture2D("Resources/block.png");
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < 480 / 16; j++)
                {
                    if (data[j][i] == '1')
                    {
                        ace.Chip2D chip = ace.Engine.Graphics.CreateChip2D();
                        chip.Texture = chip_texture;
                        chip.Src = new ace.RectF(i * 16.0f, j * 16.0f, 16.0f, 16.0f);
                        AddChip(chip);
                    }
                }
            }

        }
    }
}
