using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altseed_Side_scrolling
{
    public static class Sound
    {
        static int playingBGMhandle;
        static asd.SoundSource BGM0;
        static asd.SoundSource[] SE = new asd.SoundSource[3];
        static Sound()
        {
            BGM0 = asd.Engine.Sound.CreateSoundSource("Sound/bgm0.ogg", false);
            BGM0.IsLoopingMode = true;
            BGM0.LoopStartingPoint = 89.298f;
            BGM0.LoopEndPoint = 167.441f;

            SE[0] = asd.Engine.Sound.CreateSoundSource("Sound/jump.ogg", true);
            SE[1] = asd.Engine.Sound.CreateSoundSource("Sound/dead.ogg", true);
            SE[2] = asd.Engine.Sound.CreateSoundSource("Sound/clear.ogg", true);

        }

        static public void BGMStart()
        {
            if (asd.Engine.Sound.IsPlaying(playingBGMhandle))
            {
                asd.Engine.Sound.Stop(playingBGMhandle);
            }
            playingBGMhandle = asd.Engine.Sound.Play(BGM0);
        }
        static public void BGMStop()
        {
            if (asd.Engine.Sound.IsPlaying(playingBGMhandle))
            {
                asd.Engine.Sound.FadeOut(playingBGMhandle, 2.0f);

            }
        }
        static public void SEPlay(int code)
        {
            asd.Engine.Sound.Play(SE[code]);
        }

    }
}
