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
        FontContainer Fonts;

        public GameScene(int stagecode, Maps map)
        {
            StageCode = stagecode;
            Map = map;
            Fonts = new FontContainer();
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
            FPSViewer fps = new FPSViewer(Fonts);
            Lui.AddObject(fps);
            TimeCounter tc = new TimeCounter(Fonts);
            Lui.AddObject(tc);

            Camera Cam;
            Cam = new Camera(player);
            Lgame.AddObject(Cam);

            BackgroundCamera BCam;
            BCam = new BackgroundCamera(player);
            Lback.AddObject(BCam);

            Sound.BGMStart();
        }

        protected override void OnUpdated()
        {
            FlyingEnemyTrigger Trigger = Map.HeliTrigger.Find(t => t.PositionX == (int)player.Position.X / 32);
            if (Trigger != null)
            {
                FlyingEnemy fe = new FlyingEnemy(asd.Engine.Graphics.CreateTexture2D("Resources/Characters/heli.png"), player, Map);
                fe.Position = new asd.Vector2DF(player.Position.X + (Trigger.TurnLR ? -300.0f : 300.0f), 32.0f);
                fe.TurnLR = Trigger.TurnLR;
                Lgame.AddObject(fe);
                Map.HeliTrigger.Remove(Trigger);
            }
        }
    }

    public class DeadScene : asd.Scene
    {
        private int StageCode;
        private asd.TextObject2D Tcursor = new asd.TextObject2D();
        FontContainer Fonts;

        public DeadScene(int stagecode)
        {
            StageCode = stagecode;
            Fonts = new FontContainer();
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
            Tdead.Font = Fonts.PMP12_60B;
            Tdead.Text = "GAME OVER";
            asd.Vector2DI fsize = Fonts.PMP12_60B.CalcTextureSize(Tdead.Text, asd.WritingDirection.Horizontal);
            Tdead.CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            Tdead.Position = new asd.Vector2DF(asd.Engine.WindowSize.X, asd.Engine.WindowSize.Y) / 2;
            Ldead.AddObject(Tdead);

            BlinkingText Tpzkts = new BlinkingText(60, "Z KEY: RETRY THIS STAGE", "X KEY: BACK TO TITLE",Fonts);
            Tpzkts.Font = Fonts.PMP10_30B;
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
        private int Cursor, MaxCount;
        private asd.TextObject2D Tstage = new asd.TextObject2D();
        FontContainer Fonts;

        public TitleScene()
        {
            Cursor = 1;
            MaxCount = 3;
            Fonts = new FontContainer();
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
            TTitle.Font = Fonts.PMP12_60B;
            asd.Vector2DI fsize = Fonts.PMP12_60B.CalcTextureSize(Altseed_Side_scrolling_Core.Title, asd.WritingDirection.Horizontal);
            TTitle.CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            TTitle.Position = new asd.Vector2DF(asd.Engine.WindowSize.X / 2, 128.0f);

            BlinkingText Tpzkts = new BlinkingText(60, "Z KEY: GAME START", "←/→ KEY: SELECT STAGE",Fonts);
            Tpzkts.Font = Fonts.PMP10_30B;
            Tpzkts.Position = new asd.Vector2DF(asd.Engine.WindowSize.X / 2, 384.0f);

            Tstage.Text = "STAGE : 1 ";
            Tstage.Font = Fonts.PMP10_30B;
            fsize = Fonts.PMP10_30B.CalcTextureSize(Tstage.Text, asd.WritingDirection.Horizontal);
            Tstage.CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            Tstage.Position = new asd.Vector2DF(asd.Engine.WindowSize.X / 2, 416.0f);

            Ltitle.AddObject(Tstage);
            Ltitle.AddObject(Tpzkts);
            Ltitle.AddObject(TTitle);
            AddLayer(Ltitle);
            AddLayer(Lback);

        }

        protected override void OnUpdated()
        {
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Z) == asd.KeyState.Push) asd.Engine.ChangeScene(new TalkScene(Cursor, MapManager.Read(Cursor)));
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Right) == asd.KeyState.Push && Cursor < MaxCount) Cursor++;
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Left) == asd.KeyState.Push && Cursor > 1) Cursor--;
            Tstage.Text = "STAGE : " + Cursor.ToString();
        }
    }

    public class TalkScene : asd.Scene
    {
        int StageCode;
        Maps Map;
        int Cursor, Count;
        asd.Layer2D Ltalk;
        asd.TextObject2D[] Texts;
        FontContainer Fonts;

        public TalkScene(int stagecode, Maps map)
        {
            StageCode = stagecode;
            Map = map;
            Cursor = 0;
            Count = 0;
            Texts = new asd.TextObject2D[map.Talk.Count];
            Fonts = new FontContainer();
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
                Texts[i].Font = Fonts.PMP10_30B;
                Ltalk.AddObject(Texts[i]);
            }

            AddLayer(Ltalk);
            AddLayer(Lback);
        }

        protected override void OnUpdated()
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
                    asd.Engine.ChangeScene(new GameScene(Cursor, Map));
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
        FontContainer Fonts;

        public SplashScene(int stagecode, Maps map)
        {
            StageCode = stagecode;
            Map = map;
            Fonts = new FontContainer();
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
            Tsplash.Font = Fonts.PMP12_60B;
            Tsplash.Text = "STAGE " + StageCode;
            asd.Vector2DI fsize = Fonts.PMP12_60B.CalcTextureSize(Tsplash.Text, asd.WritingDirection.Horizontal);
            Tsplash.CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            Tsplash.Position = new asd.Vector2DF(asd.Engine.WindowSize.X, asd.Engine.WindowSize.Y) / 2;
            Lsplash.AddObject(Tsplash);

            AddLayer(Lsplash);
            AddLayer(Lback);

            Count = 0;
        }
        protected override void OnUpdated()
        {
            Count++;
            if (Count >= 100) asd.Engine.ChangeScene(new GameScene(StageCode, Map));
        }
    }
}
