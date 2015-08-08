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

        public GameScene(int stagecode)
        {
            StageCode = stagecode;
        }

        protected override void OnStart()
        {
            asd.Layer2D Lgame = new asd.Layer2D();
            Maps map = MapManager.Read(StageCode);
            Lgame.AddObject(map);

            Player player = new Player(map);
            FlyingEnemyLoop heli = new FlyingEnemyLoop(asd.Engine.Graphics.CreateTexture2D("Resources/Characters/heli.png"), player, map);
            Lgame.DrawingPriority = 2;
            Lgame.AddObject(player);
            Lgame.AddObject(heli);
            foreach (Enemy e in map.Enemies)
            {
                Lgame.AddObject(e);
            }
            AddLayer(Lgame);

            asd.Layer2D Lback = new asd.Layer2D();
            Background Gbacks = new Background(map.Length * 32);
            Lback.DrawingPriority = 0;
            Lback.AddObject(Gbacks);
            AddLayer(Lback);


            asd.Layer2D Lui = new asd.Layer2D();
            Lui.DrawingPriority = 3;
            AddLayer(Lui);
            FPSViewer fps = new FPSViewer();
            Lui.AddObject(fps);
            TimeCounter tc = new TimeCounter();
            Lui.AddObject(tc);

            Camera Cam;
            Cam = new Camera(player);
            Lgame.AddObject(Cam);

            BackgroundCamera BCam;
            BCam = new BackgroundCamera(player);
            Lback.AddObject(BCam);

            Sound.BGMStart();
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
            Tdead.Text = "GAME OVER";
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
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Z) == asd.KeyState.Push) asd.Engine.ChangeScene(new SplashScene(StageCode));
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.X) == asd.KeyState.Push) asd.Engine.ChangeScene(new TitleScene());

        }
    }

    public class TitleScene : asd.Scene
    {
        private int Cursor, MaxCount;
        private asd.TextObject2D Tstage = new asd.TextObject2D();

        public TitleScene()
        {
            Cursor = 1;
            MaxCount = 3;
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

        protected override void OnUpdated()
        {
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Z) == asd.KeyState.Push) asd.Engine.ChangeScene(new SplashScene(Cursor));
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Right) == asd.KeyState.Push && Cursor < MaxCount) Cursor++;
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Left) == asd.KeyState.Push && Cursor > 1) Cursor--;
            Tstage.Text = "STAGE : " + Cursor.ToString();


        }
    }

    public class SplashScene : asd.Scene
    {
        private int Count;
        private int StageCode;

        public SplashScene(int stagecode)
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
        }
        protected override void OnUpdated()
        {
            Count++;
            if (Count >= 100) asd.Engine.ChangeScene(new GameScene(StageCode));
        }
    }
}
