using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed_Side_scrolling
{
    public class GameScene : asd.Scene
    {
        private int StageCode;
        private Maps Map;
        private Player player;
        private asd.Layer2D Lgame;
        private asd.TextObject2D Tfps, Ttime;
        private uint Time;

        public GameScene(int stagecode, Maps map)
        {
            StageCode = stagecode;
            Map = map;
        }

        protected override void OnStart()
        {
            Lgame = new asd.Layer2D();
            Lgame.DrawingPriority = 2;

            Lgame.AddObject(Map);

            player = new Player(Map);
            Lgame.AddObject(player);
            foreach (Enemy e in Map.Enemies)
            {
                Lgame.AddObject(e);
            }
            AddLayer(Lgame);

            asd.Layer2D Lback = new asd.Layer2D();
            Background Gbacks = new Background(Map.Length * 32);
            Lback.DrawingPriority = 0;
            Lback.AddObject(Gbacks);
            AddLayer(Lback);


            asd.Layer2D Lui = new asd.Layer2D();
            Lui.DrawingPriority = 3;
            AddLayer(Lui);
            Tfps = new asd.TextObject2D();
            Tfps.Font = FontContainer.PMP10_30B;
            Tfps.Position = new asd.Vector2DF(0.0f, 0.0f);
            Lui.AddObject(Tfps);
            Ttime = new asd.TextObject2D();
            Ttime.Font = FontContainer.PMP10_30B;
            Ttime.Position = new asd.Vector2DF(500.0f, 0.0f);
            Lui.AddObject(Ttime);

            Camera Cam;
            Cam = new Camera(player);
            Lgame.AddObject(Cam);

            BackgroundCamera BCam;
            BCam = new BackgroundCamera(player);
            Lback.AddObject(BCam);

            Time = 0;
            Sound.BGMStart();
        }

        protected override void OnUpdating()
        {
            FlyingEnemyTrigger Trigger = Map.HeliTrigger.Find(t => t.PositionX == (int)player.Position.X / 32);
            if (Trigger != null)
            {
                FlyingEnemy fe = new FlyingEnemy(asd.Engine.Graphics.CreateTexture2D("Characters/heli.png"), player, Map);
                fe.Position = new asd.Vector2DF(player.Position.X + (Trigger.TurnLR ? -300.0f : 300.0f), 32.0f);
                fe.TurnLR = Trigger.TurnLR;
                Lgame.AddObject(fe);
                Map.HeliTrigger.Remove(Trigger);
            }

            Tfps.Text = asd.Engine.CurrentFPS.ToString("F1") + "FPS";
            Time++;
            Ttime.Text = "TIME: " + (Time / 60).ToString("D3");

            if (Map.IsGoal(player.Position))
            {
                Sound.SEPlay(2);
                asd.Engine.ChangeScene(new ResultScene(StageCode, Time));
            }
        }
    }

    public class DeadScene : asd.Scene
    {
        private int StageCode;
        private asd.TextObject2D Tcursor = new asd.TextObject2D();

        public DeadScene(int stagecode)
        {
            StageCode = stagecode;
        }

        protected override void OnStart()
        {
            asd.Layer2D Lback = new asd.Layer2D();
            Lback.DrawingPriority = 0;
            Background Gback = new Background(0);
            Lback.AddObject(Gback);
            asd.CameraObject2D Camera = new asd.CameraObject2D();
            Camera.Dst = new asd.RectI(0, 0, 960, 640);
            Camera.Src = new asd.RectI(0, 0, 480, 320);
            Lback.AddObject(Camera);

            asd.Layer2D Ldead = new asd.Layer2D();
            Ldead.DrawingPriority = 1;
            asd.TextObject2D Tdead = new asd.TextObject2D();
            Tdead.Font = FontContainer.PMP12_60B;
            Tdead.Text = "MISSION FAILED";
            asd.Vector2DI fsize = FontContainer.PMP12_60B.CalcTextureSize(Tdead.Text, asd.WritingDirection.Horizontal);
            Tdead.CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            Tdead.Position = new asd.Vector2DF(asd.Engine.WindowSize.X, asd.Engine.WindowSize.Y) / 2;
            Ldead.AddObject(Tdead);

            BlinkingText Tpzkts = new BlinkingText(60, "Z KEY: RETRY THIS STAGE", "X KEY: BACK TO TITLE");
            Tpzkts.Font = FontContainer.PMP10_30B;
            Tpzkts.Position = new asd.Vector2DF(asd.Engine.WindowSize.X / 2, 384.0f);
            Ldead.AddObject(Tpzkts);

            AddLayer(Ldead);
            AddLayer(Lback);

            Sound.BGMStop();
        }
        protected override void OnUpdated()
        {
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Z) == asd.KeyState.Push) asd.Engine.ChangeScene(new SplashScene(StageCode, MapManager.Read(StageCode)));
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.X) == asd.KeyState.Push) asd.Engine.ChangeScene(new TitleScene());
        }
    }

    public class TitleScene : asd.Scene
    {
        private int Cursor;
        private asd.TextObject2D Tstage = new asd.TextObject2D();

        public TitleScene()
        {
            Cursor = 1;
        }

        protected override void OnStart()
        {
            asd.Layer2D Lback = new asd.Layer2D();
            Lback.DrawingPriority = 0;
            Background Gback = new Background(0);
            Lback.AddObject(Gback);
            asd.CameraObject2D Camera = new asd.CameraObject2D();
            Camera.Dst = new asd.RectI(0, 0, 960, 640);
            Camera.Src = new asd.RectI(0, 0, 480, 320);
            Lback.AddObject(Camera);

            asd.Layer2D Ltitle = new asd.Layer2D();
            Ltitle.DrawingPriority = 1;
            asd.TextObject2D TTitle = new asd.TextObject2D();
            TTitle.Text = Altseed_Side_scrolling_Core.Title;
            TTitle.Font = FontContainer.PMP12_60B;
            asd.Vector2DI fsize = FontContainer.PMP12_60B.CalcTextureSize(Altseed_Side_scrolling_Core.Title, asd.WritingDirection.Horizontal);
            TTitle.CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            TTitle.Position = new asd.Vector2DF(asd.Engine.WindowSize.X / 2, 128.0f);

            BlinkingText Tpzkts = new BlinkingText(60, "Z KEY: GAME START", "←/→ KEY: SELECT STAGE");
            Tpzkts.Font = FontContainer.PMP10_30B;
            Tpzkts.Position = new asd.Vector2DF(asd.Engine.WindowSize.X / 2, 384.0f);

            Tstage.Text = "STAGE : 1 ";
            Tstage.Font = FontContainer.PMP10_30B;
            fsize = FontContainer.PMP10_30B.CalcTextureSize(Tstage.Text, asd.WritingDirection.Horizontal);
            Tstage.CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            Tstage.Position = new asd.Vector2DF(asd.Engine.WindowSize.X / 2, 416.0f);

            Ltitle.AddObject(Tstage);
            Ltitle.AddObject(Tpzkts);
            Ltitle.AddObject(TTitle);
            AddLayer(Ltitle);
            AddLayer(Lback);

        }

        protected override void OnUpdating()
        {
            Tstage.Text = "STAGE : " + Cursor.ToString();
        }

        protected override void OnUpdated()
        {
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Z) == asd.KeyState.Push) asd.Engine.ChangeScene(new TalkScene(Cursor, MapManager.Read(Cursor)));
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Right) == asd.KeyState.Push && Cursor < Altseed_Side_scrolling_Core.MapCount) Cursor++;
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Left) == asd.KeyState.Push && Cursor > 1) Cursor--;

        }
    }

    public class TalkScene : asd.Scene
    {
        int StageCode;
        Maps Map;
        int Cursor, Count;
        asd.Layer2D Ltalk;
        asd.TextObject2D[] Texts;

        public TalkScene(int stagecode, Maps map)
        {
            StageCode = stagecode;
            Map = map;
            Cursor = 0;
            Count = 0;
            Texts = new asd.TextObject2D[map.Talk.Count];
        }

        protected override void OnStart()
        {
            asd.Layer2D Lback = new asd.Layer2D();
            Lback.DrawingPriority = 0;
            Background Gback = new Background(0);
            Lback.AddObject(Gback);
            asd.CameraObject2D Camera = new asd.CameraObject2D();
            Camera.Dst = new asd.RectI(0, 0, 960, 640);
            Camera.Src = new asd.RectI(0, 0, 480, 320);
            Lback.AddObject(Camera);

            Ltalk = new asd.Layer2D();
            Ltalk.DrawingPriority = 1;
            for (int i = 0; i < Texts.Length; i++)
            {
                Texts[i] = new asd.TextObject2D();
                Texts[i].Font = FontContainer.PMP10_30B;
                Ltalk.AddObject(Texts[i]);
            }

            AddLayer(Ltalk);
            AddLayer(Lback);
        }

        protected override void OnUpdating()
        {
            Count++;
            for (int i = 0; i <= Cursor; i++)
            {
                Texts[i].Position = new asd.Vector2DF(80.0f, 500.0f - (Cursor - i) * 40.0f);
            }

            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Z) == asd.KeyState.Push)
            {
                Texts[Cursor].Text = Map.Talk[Cursor];
                Count = 0;
                Cursor++;
                if (Cursor == Map.Talk.Count)
                {
                    asd.Engine.ChangeScene(new SplashScene(StageCode, Map));
                    return;
                }

            }

            if (Map.Talk[Cursor].Length >= Count / 5)
            {
                Texts[Cursor].Text = Map.Talk[Cursor].Substring(0, Count / 5);
            }
        }
    }

    public class SplashScene : asd.Scene
    {
        private int Count;
        private int StageCode;
        Maps Map;

        public SplashScene(int stagecode, Maps map)
        {
            StageCode = stagecode;
            Map = map;
        }

        protected override void OnStart()
        {
            asd.Layer2D Lback = new asd.Layer2D();
            Lback.DrawingPriority = 0;
            Background Gback = new Background(0);
            Lback.AddObject(Gback);
            asd.CameraObject2D Camera = new asd.CameraObject2D();
            Camera.Dst = new asd.RectI(0, 0, 960, 640);
            Camera.Src = new asd.RectI(0, 0, 480, 320);
            Lback.AddObject(Camera);

            asd.Layer2D Lsplash = new asd.Layer2D();
            Lsplash.DrawingPriority = 1;
            asd.TextObject2D Tsplash = new asd.TextObject2D();
            Tsplash.Font = FontContainer.PMP12_60B;
            Tsplash.Text = "STAGE " + StageCode;
            asd.Vector2DI fsize = FontContainer.PMP12_60B.CalcTextureSize(Tsplash.Text, asd.WritingDirection.Horizontal);
            Tsplash.CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            Tsplash.Position = new asd.Vector2DF(asd.Engine.WindowSize.X, asd.Engine.WindowSize.Y) / 2;
            Lsplash.AddObject(Tsplash);

            AddLayer(Lsplash);
            AddLayer(Lback);

            Count = 0;
            System.Console.WriteLine(StageCode);
        }
        protected override void OnUpdated()
        {
            Count++;
            if (Count >= 100) asd.Engine.ChangeScene(new GameScene(StageCode, Map));
        }
    }

    public class ResultScene : asd.Scene
    {
        private int StageCode;
        private uint Time;
        private asd.TextObject2D Tcursor = new asd.TextObject2D();

        public ResultScene(int stagecode, uint time)
        {
            StageCode = stagecode;
            Time = time;
        }

        protected override void OnStart()
        {
            asd.Layer2D Lback = new asd.Layer2D();
            Lback.DrawingPriority = 0;
            Background Gback = new Background(0);
            Lback.AddObject(Gback);
            asd.CameraObject2D Camera = new asd.CameraObject2D();
            Camera.Dst = new asd.RectI(0, 0, 960, 640);
            Camera.Src = new asd.RectI(0, 0, 480, 320);
            Lback.AddObject(Camera);

            asd.Layer2D Lclear = new asd.Layer2D();
            Lclear.DrawingPriority = 1;
            asd.TextObject2D Tclear = new asd.TextObject2D();
            Tclear.Font = FontContainer.PMP12_60B;
            Tclear.Text = "MISSION ACCOMPLISHED!";
            asd.Vector2DI fsize = FontContainer.PMP12_60B.CalcTextureSize(Tclear.Text, asd.WritingDirection.Horizontal);
            Tclear.CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            Tclear.Position = new asd.Vector2DF(asd.Engine.WindowSize.X, asd.Engine.WindowSize.Y) / 2;
            Lclear.AddObject(Tclear);

            asd.TextObject2D Tpzkts;
            if (StageCode == Altseed_Side_scrolling_Core.MapCount)
            {
                Tpzkts = new asd.TextObject2D();
                Tpzkts.Text = "X KEY: BACK TO TITLE";
                fsize = FontContainer.PMP10_30B.CalcTextureSize(Tpzkts.Text, asd.WritingDirection.Horizontal);
                Tpzkts.CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            }
            else
            {
                Tpzkts = new BlinkingText(60, "Z KEY: GO TO NEXT STAGE", "X KEY: BACK TO TITLE");
            }
            Tpzkts.Font = FontContainer.PMP10_30B;
            Tpzkts.Position = new asd.Vector2DF(asd.Engine.WindowSize.X / 2, 384.0f);
            Lclear.AddObject(Tpzkts);

            asd.TextObject2D Ttime = new asd.TextObject2D();
            Ttime.Font = FontContainer.PMP10_30B;
            Ttime.Text = "TIME:" + (Time / 60).ToString("D3");
            fsize = FontContainer.PMP10_30B.CalcTextureSize(Ttime.Text, asd.WritingDirection.Horizontal);
            Ttime.CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            Ttime.Position = new asd.Vector2DF(asd.Engine.WindowSize.X / 2, 424.0f);
            Lclear.AddObject(Ttime);

            AddLayer(Lclear);
            AddLayer(Lback);

            Sound.BGMStop();
        }
        protected override void OnUpdated()
        {
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Z) == asd.KeyState.Push) asd.Engine.ChangeScene(new TalkScene(StageCode + 1, MapManager.Read(StageCode + 1)));
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.X) == asd.KeyState.Push) asd.Engine.ChangeScene(new TitleScene());
        }
    }
}
