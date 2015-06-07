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
        public int Length = int.MaxValue;
        public string[] Data = new string[30];

        public Maps(int len)
        {

            StreamReader reader = new StreamReader("Maps/01.txt", Encoding.Unicode);
            for (int i = 0; i < 480 / 32; i++)
            {
                Data[i] = reader.ReadLine();
                Length = Math.Min(Length, Data[i].Length);
            }
            reader.Close();

            ace.Texture2D chip_texture = ace.Engine.Graphics.CreateTexture2D("Resources/block.png");
            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < 480 / 32; j++)
                {
                    if (Data[j][i] == '1')
                    {
                        ace.Chip2D chip = ace.Engine.Graphics.CreateChip2D();
                        chip.Texture = chip_texture;
                        chip.Src = new ace.RectF(i * 32.0f, j * 32.0f, 32.0f, 32.0f);
                        AddChip(chip);
                    }
                }
            }
        }

        public bool Isblocked(ace.Vector2DF pos)
        {
            ace.Vector2DI Cell = new ace.Vector2DI((int)pos.X / 32, (int)pos.Y / 32);
            return Data[Cell.Y][Cell.X]=='1';
        }
    }
}
