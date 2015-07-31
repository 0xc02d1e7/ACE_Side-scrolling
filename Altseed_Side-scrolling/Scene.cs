using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed_Side_scrolling
{
    public class GameScene : asd.Scene
    {
        protected override void OnStart()
        {
            asd.Layer2D Lblock = new asd.Layer2D();
            Lblock.DrawingPriority = 1;
            this.AddLayer(Lblock);
            Maps map = new Maps();
            Lblock.AddObject(map);

            asd.Layer2D Lchar = new asd.Layer2D();
            Player player = new Player(map);
            player.DrawingPriority = 65536;
            Enemy train1 = new Enemy("Resources/Characters/trainL.png", new asd.Vector2DF(22.0f * 32.0f, 0.0f), map);
            Enemy train2 = new Enemy("Resources/Characters/truck.png", new asd.Vector2DF(10.0f * 32.0f, 0.0f), map);
            FlyingEnemy heli = new FlyingEnemy(asd.Engine.Graphics.CreateTexture2D("Resources/Characters/heli.png"), player, map);
            Lchar.DrawingPriority = 2;
            Lchar.AddObject(player);
            Lchar.AddObject(heli);
            Lchar.AddObject(train1);
            Lchar.AddObject(train2);
            this.AddLayer(Lchar);

            asd.Layer2D Lback = new asd.Layer2D();
            Background Gbacks = new Background(map.Length * 32);
            Lback.DrawingPriority = 0;
            Lback.AddObject(Gbacks);
            this.AddLayer(Lback);

            asd.Layer2D Lui = new asd.Layer2D();
            Lui.DrawingPriority = 3;
            this.AddLayer(Lui);

            FPSViewer fps = new FPSViewer();
            Lui.AddObject(fps);
            TimeCounter tc = new TimeCounter();
            Lui.AddObject(tc);
            tc.Start();

            Camera Cam;
            Cam = new Camera(player);
            Lchar.AddObject(Cam);
            Cam = new Camera(player);
            Lblock.AddObject(Cam);
            BackgroundCamera BCam;
            BCam = new BackgroundCamera(player);
            Lback.AddObject(BCam);
        }
    }

    public class DeadScene : asd.Scene
    {
        protected override void OnStart()
        {
            asd.Layer2D Ldead = new asd.Layer2D();
            asd.TextObject2D Tdead = new asd.TextObject2D();
            Tdead.Font = FontContainer.PMP12_60;
            Tdead.Text = "突然の死";
            asd.Vector2DI fsize = FontContainer.PMP12_60.CalcTextureSize("突然の死", asd.WritingDirection.Horizontal);
            Tdead.CenterPosition = new asd.Vector2DF(fsize.X, fsize.Y) / 2;
            Tdead.Position = new asd.Vector2DF(asd.Engine.WindowSize.X, asd.Engine.WindowSize.Y) / 2;
            Ldead.AddObject(Tdead);
            this.AddLayer(Ldead);
        }
        protected override void OnUpdated()
        {
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Z) == asd.KeyState.Push) asd.Engine.ChangeScene(new GameScene());
        }
    }

    public class TitleScene : asd.Scene
    {
        protected override void OnStart()
        {
            asd.Layer2D Ltitle = new asd.Layer2D();

            asd.TextureObject2D Gtitleback = new asd.TextureObject2D();
            Gtitleback.Texture = asd.Engine.Graphics.CreateTexture2D("Resources/UI/back.png");
            asd.TextureObject2D Gtitlelogo = new asd.TextureObject2D();
            Gtitlelogo.Texture = asd.Engine.Graphics.CreateTexture2D("Resources/UI/logo.png");
            Gtitlelogo.Position = new asd.Vector2DF((asd.Engine.WindowSize.X - Gtitlelogo.Texture.Size.X) / 2.0f, 50.0f);
            Ltitle.AddObject(Gtitleback);
            Ltitle.AddObject(Gtitlelogo);
            this.AddLayer(Ltitle);
        }

        protected override void OnUpdated()
        {
            if (asd.Engine.Keyboard.GetKeyState(asd.Keys.Z) == asd.KeyState.Push) asd.Engine.ChangeScene(new GameScene());
        }
    }
}
