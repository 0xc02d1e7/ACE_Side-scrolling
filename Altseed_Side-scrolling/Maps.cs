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

    static public class MapManager
    {
        static public Dictionary<char, MapParts> Parts;
        static public Dictionary<string, asd.Texture2D> EnemyGraphic;

        static MapManager()
        {
            Parts = new Dictionary<char, MapParts>();
            EnemyGraphic = new Dictionary<string, asd.Texture2D>();
            IEnumerable<string> Enumerate;
            Enumerate = Directory.EnumerateFiles("Resources/Block/", "?.png");
            foreach (string C in Enumerate)
            {
                Parts.Add(C.Substring(16)[0], new MapParts(C, true));
            }

            Enumerate = Directory.EnumerateFiles("Resources/Enterable/", "?.png");
            foreach (string C in Enumerate)
            {
                Parts.Add(C.Substring(20)[0], new MapParts(C, false));
            }

            Enumerate = Directory.EnumerateFiles("Resources/Characters/", "*.png");
            foreach (string C in Enumerate)
            {
                EnemyGraphic.Add(C.Substring(21).Split('.')[0], asd.Engine.Graphics.CreateTexture2D(C));
            }
        }

        static public Maps Read(String Filename)
        {
            Maps map = new Maps();

            StreamReader reader = new StreamReader(Filename, Encoding.Unicode);
            for (int i = 0; i < 10; i++)
            {
                map.Data[i] = reader.ReadLine();
                map.Length = Math.Min(map.Length, map.Data[i].Length);
            }

            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    asd.Chip2D chip = new asd.Chip2D();
                    chip.Texture = Parts[map.Data[j][i]].Texture;
                    chip.Position = new asd.Vector2DF(i * 32.0f, j * 32.0f);
                    chip.CenterPosition = new asd.Vector2DF(16.0f, 16.0f);
                    map.AddChip(chip);
                }
            }

            while (reader.Peek() >= 0)
            {
                String[] Datas = (reader.ReadLine()).Split(' ');
                int x=int.Parse(Datas[1]), y=int.Parse(Datas[2]);
                Enemy e = new Enemy(EnemyGraphic[Datas[0]], new asd.Vector2DF(x * 32.0f, y * 32.0f), map);
                map.Enemies.Add(e);
            }
            reader.Close();

            return map;
        }
    }

    public class Maps : asd.MapObject2D
    {
        public int Length = int.MaxValue;
        public string[] Data = new string[10];
        public List<Enemy> Enemies;

        public Maps()
        {
            this.DrawingPriority = 1;
            Enemies = new List<Enemy>();
        }

        public bool Isblocked(asd.Vector2DF pos)
        {
            asd.Vector2DI Cell = new asd.Vector2DI((int)pos.X / 32, (int)pos.Y / 32);
            if (Cell.X < 0 || Cell.Y < 0 || Cell.X >= Length || Cell.Y >= 10) return false;
            return MapManager.Parts[Data[Cell.Y][Cell.X]].IsBlock;
        }
    }
}
