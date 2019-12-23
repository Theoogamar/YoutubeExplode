using System.Collections.Generic;
using System.Linq;
using YoutubeExplode.Models.MediaStreams;

namespace YoutubeDownloader
{
    public static class CustomExtensions
    {
        /// <summary>
        /// Gets the audio stream with highest bitrate
        /// with out a selected AudioEncoding types.
        /// Returns null if sequence is empty.
        /// </summary>
        public static AudioStreamInfo GetHighestBitrate(this IEnumerable<AudioStreamInfo> streamInfos, out int highest, params AudioEncoding[] audioEncodings)
        {
            highest = default;

            // streamInfos.GuardNotNull(nameof(streamInfos));
            streamInfos = streamInfos.OrderByDescending(s => s.Bitrate);
            for (int i = 0; i < streamInfos.Count(); i++)
            {
                for (int ii = 0; ii < audioEncodings.Length; ii++)
                {
                    if (streamInfos.ElementAt(i).AudioEncoding != audioEncodings[ii])
                    {
                        highest = i;

                        return streamInfos.ElementAt(i);
                    }
                }
            }

            return null;
        }
    }
}
