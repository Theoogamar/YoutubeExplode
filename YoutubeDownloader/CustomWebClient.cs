using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

namespace YoutubeDownloader
{
    public class CustomWebClient : WebClient
    {
        public ProgressBar ProgBar { get; set; }
        public int Index { get; set; }
    }
}
