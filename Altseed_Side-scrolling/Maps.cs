using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace Altseed_Side_scrolling
{
    static public class MapManager
    {
        static public asd.Texture2D[] Parts;
        static public asd.Texture2D[] Chars;
        static MapManager()
        {
            Parts = new asd.Texture2D[256];
            for (int i = 0; i <= 0xFF; i++)
            {
                Parts[i] = asd.Engine.Graphics.CreateTexture2D("Resources/Block/" + i.ToString("X2") + ".png");
            }

            Chars = new asd.Texture2D[256];
            for (int i = 0; i < 256; i++)
            {
                Chars[i] = asd.Engine.Graphics.CreateTexture2D("Resources/Characters/" + i.ToString("X2") + ".png");
                if (Chars[i] == null) break;
            }
        }

        static public Maps Read(int stagecode)
        {
            System.Globalization.NumberStyles Hex = System.Globalization.NumberStyles.AllowHexSpecifier;
            XmlDocument doc = new XmlDocument();
            doc.Load("Maps/" + stagecode.ToString("X2") + ".xml");


            Maps map = new Maps();
            map.StageCode = stagecode;
            XmlNode root = doc.DocumentElement;
            XmlNodeList data = root.SelectSingleNode("Map").ChildNodes;
            for (int i = 0; i < 10; i++)
            {
                string[] splited = data.Item(i).InnerText.Split(' ');
                for (int j = 0; j < splited.Length; j++)
                {

                    map.Data[i, j] = int.Parse(splited[j], Hex);
                }
                map.Length = Math.Min(map.Length, splited.Length);
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < map.Length; j++)
                {
                    asd.Chip2D chip = new asd.Chip2D();
                    chip.Texture = Parts[map.Data[i, j]];
                    chip.Position = new asd.Vector2DF(j * 32.0f, i * 32.0f);
                    chip.CenterPosition = new asd.Vector2DF(16.0f, 16.0f);
                    map.AddChip(chip);
                }
            }

            XmlNodeList enemy = root.SelectNodes("Enemy");
            foreach (XmlNode en in enemy)
            {
                int Type = int.Parse(en.SelectSingleNode("Type").InnerText, Hex);
                int X = int.Parse(en.SelectSingleNode("X").InnerText);
                int Y = int.Parse(en.SelectSingleNode("Y").InnerText);
                Enemy e = new Enemy(Chars[Type], new asd.Vector2DF(X * 32.0f, Y * 32.0f), map);
                map.Enemies.Add(e);
            }
            return map;
        }
    }

    public class Maps : asd.MapObject2D
    {
        public int StageCode;
        public int Length = int.MaxValue;
        public int[,] Data = new int[10, 512];
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
            return (Data[Cell.Y, Cell.X] < 0x80);
        }
    }
}
