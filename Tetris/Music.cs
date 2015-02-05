using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.IO;
using System.Windows;
using System.Threading;
using System.ComponentModel;
using System.Media;
using System.Reflection;


namespace Tetris
{
    class Music
    {

        public Music(string sound)
        {
            SoundPlayer music = new SoundPlayer(sound);
            music.PlayLooping();
        }
    }
}
