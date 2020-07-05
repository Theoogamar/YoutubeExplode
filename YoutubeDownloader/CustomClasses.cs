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
        /// Gets the audio stream with highest bitrate without vorbis encoding.
        /// Returns null if sequence is empty.
        /// </summary>
        public static IStreamInfo WithHighestBitrateWithoutVorbisEncoding(this IEnumerable<AudioOnlyStreamInfo> streamInfos, out int highest)
        {
            var audios = streamInfos.OrderByDescending(s => s.Bitrate);

            highest = 0;

            foreach (var audio in audios)
            {
                if (audio.AudioCodec != "vorbis")
                    return audio;

                highest++;
            }

            return null;
        }
    }

    public class CustomWebClient : WebClient
    {
        public string Name { get; set; }

        public ProgressBar Progbar { get; set; }

        public int Index { get; set; }
    }
}
