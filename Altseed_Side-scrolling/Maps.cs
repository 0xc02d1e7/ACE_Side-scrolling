using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Altseed_Side_scrolling
{
    public class MapParts
    {
        public asd.Texture2D Texture;
        public bool IsBlock;

        public MapParts(string path, bool isblock)
        {
            Texture = asd.Engine.Graphics.CreateTexture2D(path);
            IsBlock = isblock;
        }
    }

    public class Maps : asd.MapObject2D
    {
        public int Length = int.MaxValue;
        public string[] Data = new string[30];
        public Dictionary<char, MapParts> Parts;
        public List<asd.Chip2D> Blocks=new List<asd.Chip2D>();

        public Maps(int len)
        {
            Parts = new Dictionary<char, MapParts>();

            IEnumerable<string> ChipList;
            ChipList = Directory.EnumerateFiles("Resources/Block/", "?.png");
            foreach (string C in ChipList)
            {
                Parts.Add(C.Substring(16)[0], new MapParts(C, true));
            }

            ChipList = Directory.EnumerateFiles("Resources/Enterable/", "?.png");
            foreach (string C in ChipList)
            {
                Parts.Add(C.Substring(20)[0], new MapParts(C, false));
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
                    chip.Texture = Parts[Data[j][i]].Texture;
                    chip.Position = new asd.Vector2DF(i * 32.0f, j * 32.0f);
                    chip.CenterPosition = new asd.Vector2DF(16.0f, 16.0f);
                    AddChip(chip);
                    Blocks.Add(chip);

                }
            }
        }

        public bool Isblocked(asd.Vector2DF pos)
        {
            asd.Vector2DI Cell = new asd.Vector2DI((int)pos.X / 32, (int)pos.Y / 32);
            return Parts[Data[Cell.Y][Cell.X]].IsBlock;
        }
    }
}
