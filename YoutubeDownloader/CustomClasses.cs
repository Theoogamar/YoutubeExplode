using System.Collections.Generic;
using System.Linq;
using YoutubeExplode.Videos.Streams;
using System.Windows.Forms;
using System.Net;
using System.ComponentModel;

namespace YoutubeDownloader
{
    public static class CustomExtensions
    {
        /// <summary>
        /// Gets the audio stream with highest bitrate
        /// with out a selected AudioEncoding types.
        /// Returns null if sequence is empty.
        /// </summary>
        //public static IStreamInfo? WithHighestBitrate(this IEnumerable<IStreamInfo> streamInfos) =>
        //    streamInfos.OrderByDescending(s => s.Bitrate).FirstOrDefault();

    }

    public class CustomWebClient : WebClient
    {
        public ProgressBar Progbar { get; set; }

        public int Index { get; set; }

        public string Name { get; set; }
    }

    class CustomBackgroundWorker : BackgroundWorker
    {
        public string Name { get; set; }

        public ProgressBar Progbar { get; set; }

        public int Index { get; set; }
    }
}
