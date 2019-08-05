using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

namespace YoutubeDownloader
{
    public class CustomWebClient : WebClient
    {
        public ProgressBar Progbar { get; set; }

        public int Index { get; set; }

        public string Name { get; set; }
    }
}
