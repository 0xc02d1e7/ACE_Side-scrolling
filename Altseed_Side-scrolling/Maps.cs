using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Globalization;


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
                Parts[i] = asd.Engine.Graphics.CreateTexture2D("Block/" + i.ToString("X2") + ".png");
            }

            Chars = new asd.Texture2D[256];
            for (int i = 0; i <= 0xFF; i++)
            {
                Chars[i] = asd.Engine.Graphics.CreateTexture2D("Characters/" + i.ToString("X2") + ".png");
                if (Chars[i] == null) break;
            }
        }

        static public Maps Read(int stagecode)
        {
            Maps map = new Maps();
            map.StageCode = stagecode;

            NumberStyles Hex = NumberStyles.AllowHexSpecifier;
            Encoding UTF8Encoder = Encoding.GetEncoding("UTF-8");

            asd.StaticFile reader = asd.Engine.File.CreateStaticFile("Maps/" + stagecode.ToString("X2") + ".xml");
            XmlDocument XmlDoc = new XmlDocument();
            System.Console.Write(UTF8Encoder.GetString(reader.Buffer));
            XmlDoc.LoadXml(UTF8Encoder.GetString(reader.Buffer));
            XmlNode Root = XmlDoc.DocumentElement;

            foreach (XmlNode TalkNode in Root.SelectSingleNode("Talk").ChildNodes)
            {
                map.Talk.Add(TalkNode.InnerText);
            }

            XmlNodeList ChipsNodes = Root.SelectSingleNode("Field").ChildNodes;
            List<List<int>> FieldBuffer = new List<List<int>>();
            for (int i = 0; i < 10; i++)
            {
                FieldBuffer.Add(new List<int>());
                string[] splited = ChipsNodes.Item(i).InnerText.Split(' ');
                for (int j = 0; j < splited.Length; j++)
                {
                    FieldBuffer[i].Add(int.Parse(splited[j], Hex));
                }
                map.Length = Math.Min(splited.Length, map.Length);
            }

            map.Field = new int[10, map.Length];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < map.Length; j++)
                {
                    map.Field[i, j] = FieldBuffer[i][j];

                    asd.Chip2D chip = new asd.Chip2D();
                    chip.Texture = Parts[map.Field[i, j]];
                    chip.Position = new asd.Vector2DF(j * 32.0f, i * 32.0f);
                    chip.CenterPosition = new asd.Vector2DF(16.0f, 16.0f);
                    map.AddChip(chip);
                }
            }

            foreach (XmlNode EnemyNode in Root.SelectNodes("Enemy"))
            {
                int Type = int.Parse(EnemyNode.SelectSingleNode("Type").InnerText, Hex);
                int X = int.Parse(EnemyNode.SelectSingleNode("X").InnerText);
                int Y = int.Parse(EnemyNode.SelectSingleNode("Y").InnerText);
                Enemy e = new Enemy(Chars[Type], new asd.Vector2DF(X * 32.0f, Y * 32.0f), map);
                map.Enemies.Add(e);
            }

            foreach (XmlNode TriggerNode in Root.SelectSingleNode("FlyingEnemyTriggers").ChildNodes)
            {
                int X = int.Parse(TriggerNode.SelectSingleNode("X").InnerText);
                bool turn = (TriggerNode.SelectSingleNode("Turn").InnerText == "true");
                FlyingEnemyTrigger t = new FlyingEnemyTrigger(X, turn);
                map.HeliTrigger.Add(t);
            }
            return map;
        }
    }

    public class Maps : asd.MapObject2D
    {
        public int StageCode;
        public int Length = int.MaxValue;
        public int[,] Field;
        public List<string> Talk;
        public List<Enemy> Enemies;
        public List<FlyingEnemyTrigger> HeliTrigger;

        public Maps()
        {
            DrawingPriority = 1;
            Talk = new List<string>();
            Enemies = new List<Enemy>();
            HeliTrigger = new List<FlyingEnemyTrigger>();
        }

        public bool Isblocked(asd.Vector2DF pos)
        {
            asd.Vector2DI Cell = new asd.Vector2DI((int)pos.X / 32, (int)pos.Y / 32);
            if (Cell.X < 0 || Cell.Y < 0 || Cell.X >= Length || Cell.Y >= 10) return false;
            return (Field[Cell.Y, Cell.X] < 0x80);
        }

        public bool IsGoal(asd.Vector2DF pos)
        {
            asd.Vector2DI Cell = new asd.Vector2DI((int)pos.X / 32, (int)pos.Y / 32);
            if (Cell.X < 0 || Cell.Y < 0 || Cell.X >= Length || Cell.Y >= 10) return false;
            return (Field[Cell.Y, Cell.X] == 0xFF);
        }
    }
}
