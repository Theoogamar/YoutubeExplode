using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoutubeDownloader
{
    class CustomBackgroundWorker : BackgroundWorker
    {
        public string Name { get; set; }

        public ProgressBar Progbar { get; set; }

        public int Index { get; set; }
    }
}
