using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Altseed_Side_scrolling
{
    public class MapChips
    {
        public asd.Texture2D Texture;
        public bool IsBlock;

        public MapChips(string path, bool isblock)
        {
            Texture = asd.Engine.Graphics.CreateTexture2D(path);
            IsBlock = isblock;
        }
    }

    public class Maps : asd.MapObject2D
    {
        public int Length = int.MaxValue;
        public string[] Data = new string[30];
        Dictionary<char, MapChips> Chips;

        public Maps(int len)
        {
            Chips = new Dictionary<char, MapChips>();

            IEnumerable<string> ChipList;
            ChipList = Directory.EnumerateFiles("Resources/Block/", "?.png");
            foreach (string C in ChipList)
            {
                Chips.Add(C.Substring(16)[0], new MapChips(C, true));
            }

            ChipList = Directory.EnumerateFiles("Resources/Enterable/", "?.png");
            foreach (string C in ChipList)
            {
                Chips.Add(C.Substring(20)[0], new MapChips(C, false));
            }


            StreamReader reader = new StreamReader("Maps/01.txt", Encoding.Unicode);

            for (int i = 0; i < 480 / 32; i++)
            {
                Data[i] = reader.ReadLine();
                Length = Math.Min(Length, Data[i].Length);
            }
            reader.Close();

            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < 480 / 32; j++)
                {
                    asd.Chip2D chip = new asd.Chip2D();
                    chip.Texture = Chips[Data[j][i]].Texture;
                    chip.Position = new asd.Vector2DF(i * 32.0f, j * 32.0f);

                    AddChip(chip);
                }
            }
        }

        public bool Isblocked(asd.Vector2DF pos)
        {
            asd.Vector2DI Cell = new asd.Vector2DI((int)pos.X / 32, (int)pos.Y / 32);
            return Chips[Data[Cell.Y][Cell.X]].IsBlock;
        }
    }
}
