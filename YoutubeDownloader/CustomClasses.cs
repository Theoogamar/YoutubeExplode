using System.Collections.Generic;
using System.Linq;
using YoutubeExplode.Videos.Streams;
using System.Windows.Forms;
using System.Net;
using NAudio.Wave;
using NAudio.MediaFoundation;

namespace YoutubeDownloader
{
    internal static class CustomExtensions
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
        public ListViewItem Item { get; set; }

        public ProgressBar ProgBar { get; set; }

        public int Index { get; set; }
    }

    public class CustomMediaFoundationEncoder : MediaFoundationEncoder
    {
        public ListViewItem Item { get; set; }

        public ProgressBar ProgBar { get; set; }

        public int Index { get; set; }

        public CustomMediaFoundationEncoder(MediaType mediaType) : base(mediaType)
        {

        }
    }
}
