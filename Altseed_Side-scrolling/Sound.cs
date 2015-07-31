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
        static asd.SoundSource[] SE=new asd.SoundSource[3];
        static Sound()
        {
            BGM0 = asd.Engine.Sound.CreateSoundSource("Resources/Sound/bgm0.ogg", false);
            BGM0.IsLoopingMode = true;
            BGM0.LoopEndPoint = 89.298f;
            BGM0.LoopEndPoint = 167.441f;

            SE[0] = asd.Engine.Sound.CreateSoundSource("Resources/Sound/jump.ogg", false);
            SE[1] = asd.Engine.Sound.CreateSoundSource("Resources/Sound/dead.ogg", false);
            SE[2] = asd.Engine.Sound.CreateSoundSource("Resources/Sound/clear.ogg", false);

        }

        static public void BGMStart()
        {
            playingBGMhandle = asd.Engine.Sound.Play(BGM0);
        }
        static public void BGMStop()
        {
            if (asd.Engine.Sound.IsPlaying(playingBGMhandle))
            {
                asd.Engine.Sound.Stop(playingBGMhandle);
            }
        }
        static public void SEPlay(int code)
        {
            playingBGMhandle = asd.Engine.Sound.Play(SE[code]);
        }

    }
}
