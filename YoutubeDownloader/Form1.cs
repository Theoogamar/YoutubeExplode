using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using NAudio.Wave;
using NAudio.MediaFoundation;
using System.Diagnostics;

namespace YoutubeDownloader
{
    public partial class Form1 : Form
    {
        // gets the users download folder
        private static readonly string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\";

        // data for downloading audio
        private int Bitrate;
        private string audioUrl;

        // data for downloading video
        private List<string> videoUrls;

        // iterator for the "loading..." animation
        private sbyte iter = -1;

        // stores the video lenght
        private TimeSpan duration;

        // stores the video id to put the source url in the .mp3 meta data
         private string id;

        // enum for listView
        private enum subItems
        {
            fileName,
            Progress,
            Total
        }

        // Constructor
        public Form1()
        {
            InitializeComponent();

            TxtUrl.Text = "";
        }

        // get the youtube Url from the user
        private async void BtnPaste_Click(object sender, EventArgs e)
        {
            //getting the users copyed text getting the first line
            TxtUrl.Text = Clipboard.GetText().Split('\n').FirstOrDefault();

            // toggle UI elements off
            toggleThings(false);
            btnPaste.Enabled = false;

            // Get the video ID form the youtube URL            
            if (tryParseVideoId(TxtUrl.Text, out id))
            {
                // tell the user it's loading
                iter = 0;
                txtLoading.Visible = true;

                // media stream info
                StreamManifest streams;

                try
                {
                    // Client
                    YoutubeClient client = new YoutubeClient();

                    // Get media stream info
                    streams = await client.Videos.Streams.GetManifestAsync(id);

                    // get the meta data for the video
                    var video = await client.Videos.GetAsync(id);
     
                    // Set video tile to text box
                    txtBoxVidName.Text = video.Title;

                    // get duration and set the duration
                    duration = video.Duration;
                    txtLast.Text = duration.ToString(@"mm\:ss");

                    // Set thimbnail pic to picture box
                    Thumbnail.LoadAsync(video.Thumbnails.MaxResUrl);
                }
                catch (Exception ex)
                {
                    // if the YoutubeClient fails to parse the youtube url tell the user and return
                    txtLoading.Text = ex.Message;
                    txtLoading.Visible = true;
                    iter = -1;
                    btnPaste.Enabled = true;
                    return;
                }

                // get the highest bitrate audio stream with out vorbis audio encoding
                var Audio = streams.GetAudioOnly().WithHighestBitrateWithoutVorbisEncoding(out int highest);

                if (Audio == null)
                {
                    // if the YoutubeClient fails to get any audio
                    txtLoading.Text = $"There's no audio, number of audios: {highest}";
                    txtLoading.Visible = true;
                    iter = -1;
                    btnPaste.Enabled = true;
                    return;
                }

                // add the highest quailty audio button
                if (highest == 0)
                    btnAudio.Text = $"Highest bitrate: {(int)Audio.Bitrate.KiloBitsPerSecond}kbps .mp3";
                else
                    btnAudio.Text = $"{highest + 1}nd Highest bitrate: {(int)Audio.Bitrate.KiloBitsPerSecond}kbps .mp3";

                // save the url for downloading
                audioUrl = Audio.Url;

                // save the bitrate for downloading
                Bitrate = (int)Audio.Bitrate.BitsPerSecond;

                // clear and make a new list
                videoUrls = new List<string>();

                // Set all video things
                foreach (var video in streams.GetMuxed())
                {
                    // add video info to combobox
                    comBoxVideo.Items.Add($"Resolution: {video.VideoQualityLabel} .{video.Container}");

                    // add download url to string arry
                    videoUrls.Add(video.Url);
                }

                // toggle it back off
                txtLoading.Visible = false;

                // toggle UI elements on
                toggleThings(true);

                // stop the animation
                iter = -1;
            }

            // toggle paste button back on
            btnPaste.Enabled = true;
        }

        private bool tryParseVideoId(string url, out string id)
        {
            try
            {
                id = new VideoId(url);
                return true;
            }
            catch
            {
                id = "";
                return false;
            }
        }

        // toggle the UI
        private void toggleThings(bool b)
        {
            txtBoxVidName.Enabled = b;
            txtBoxVidName.Enabled = b;
            comBoxVideo.Enabled = b;
            btnDownload.Enabled = b;
            toggleSlideBar(b);
            btnX.Enabled = b;

            // if disabling UI elements also clear them
            if (!b)
            {
                btnAudio.Text = "";
                btnAudio.ForeColor = Color.Black;
                comBoxVideo.Items.Clear();
                txtBoxVidName.Text = "";
                txtFirst.Text = "00:00";
                txtLast.Text = "00:00";
                barFirst.Value = 0;
                barLast.Value = barLast.Maximum;
            }
        }

        // download dialog
        private async void btnDownload_Click(object sender, EventArgs e)
        {
            // Remove Illegal Path Characters
            string fileName = RemoveIllegalPathCharacters(txtBoxVidName.Text);

            // clear the picture box
            Thumbnail.Image = null;

            // clear pasted text
            TxtUrl.Text = "";

            // make sure the file isn't the same as ones being downloading or get an IO exception
            for (int i = 0; i < listView.Items.Count; i++)
            {
                // checking if the downloading files isn't the one trying to be downloaded
                if (listView.Items[i].SubItems[(int)subItems.fileName].Text == fileName)
                {
                    // disable and clear the ui and tell the user it's already being downloading
                    toggleThings(false);
                    Thumbnail.Image = null;
                    txtLoading.Text = "Already added";
                    txtLoading.Visible = true;
                    return;
                }
            }

            // if the user picked to download audio or video
            if (comBoxVideo.SelectedIndex == -1)
            {
                // copy the cut values off the UI thread
                int first = barFirst.Value;
                int last = barLast.Maximum - barLast.Value;

                // reset the video settings in the UI
                toggleThings(false);
                Refresh();

                try
                {
                    // download the auido from url
                    using (var reader = new MediaFoundationReader(audioUrl))
                    {
                        // set up the encoder for mp3 file formate
                        var mediaType = MediaFoundationEncoder.SelectMediaType(AudioSubtypes.MFAudioFormat_MP3, reader.WaveFormat, Bitrate);
                        if (mediaType == null) throw new InvalidOperationException("No suitable MP3 encoders available");
                        var audio = new CustomMediaFoundationEncoder(mediaType);

                        // add all the nessary varables to the custom class
                        audio.Item = LvAddItem(ref listView, fileName, out ProgressBar progbar);
                        audio.ProgBar = progbar;
                        audio.Index = listView.Items.Count - 1;

                        audio.LoadProgressChanged += encoder_LoadProgressChanged;
                        audio.LoadComplete += encoder_LoadComplete;

                        // encode it to mp3
                        await Task.Run(() => audio.Encode($"{FilePath}{fileName}.mp3", reader, first, last));

                        void encoder_LoadProgressChanged(object o, LoadProgressChangedEventArgs ev)
                        {
                            // set current encoder
                            var encoder = o as CustomMediaFoundationEncoder;

                            // Running on the UI thread
                            listView.Invoke((MethodInvoker)delegate
                            {
                                // update the bytes received in the listView
                                encoder.Item.SubItems[(int)subItems.Total].Text =
                                        $"{ev.TimeEncoded.ToString(@"mm\:ss")} / {ev.TotalTimeToEncode.ToString(@"mm\:ss")}MB";
                            });

                            // Running on the UI thread
                            encoder.ProgBar.Invoke((MethodInvoker)delegate
                            {
                                // set the progress on the progress bar
                                encoder.ProgBar.Value = ev.ProgressPercentage;

                                // checks if the listitem has moved down
                                if (encoder.Item.Index != encoder.Index)
                                {
                                    encoder.Index = encoder.Item.Index;

                                    // move the progress bar down one
                                    encoder.ProgBar.Top -= 18;
                                }
                            });
                        }

                        void encoder_LoadComplete(object o, EventArgs ev)
                        {
                            // puts the source url in the .mp3 meta data in the comment section
                            TagLib.File f = TagLib.File.Create($"{FilePath}{fileName}.mp3");
                            f.Tag.Comment = $"www.youtube.com/watch?v={id}";
                            f.Save();

                            // set current encoder
                            var encoder = o as CustomMediaFoundationEncoder;

                            encoder.LoadProgressChanged -= encoder_LoadProgressChanged;
                            encoder.LoadComplete -= encoder_LoadComplete;

                            // Running on the UI thread, remove the progress bar
                            encoder.ProgBar.Invoke((MethodInvoker)delegate {
                                encoder.ProgBar.Dispose();
                            });

                            // Running on the UI thread, remove the listViewItem
                            listView.Invoke((MethodInvoker)delegate {
                                encoder.Item.Remove();
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // if something messes up tell the user
                    txtLoading.Text = ex.Message;
                    txtLoading.Visible = true;
                }                                   
            }
            else
            {
                // get the video codec
                string item = comBoxVideo.Items[comBoxVideo.SelectedIndex] as string;
                string codec = item.Split(' ')[2];

                // getting the download url
                string url = videoUrls[comBoxVideo.SelectedIndex];

                // reset the video settings in the UI
                toggleThings(false);
                Refresh();

                // download the video from url
                using (CustomWebClient web = new CustomWebClient())
                {
                    // adds a download item to the listview
                    web.Item = LvAddItem(ref listView, fileName, out ProgressBar probar);

                    // add the progress bar
                    web.ProgBar = probar;

                    // the position in the listViewItem to measure changes
                    web.Index = listView.Items.Count - 1;

                    // add events
                    web.DownloadProgressChanged += web_DownloadProgressChanged;
                    web.DownloadFileCompleted += web_DownloadFileCompleted;

                    // download the video file
                    await web.DownloadFileTaskAsync(url, FilePath + fileName + codec);

                    // displaying the download progress
                    void web_DownloadProgressChanged(object o, DownloadProgressChangedEventArgs ev)
                    {
                        // set current webclient
                        var customWebClient = o as CustomWebClient;

                        // set the progress on the progress bar
                        customWebClient.ProgBar.Value = ev.ProgressPercentage;

                        // checks if the listitem has moved down
                        if (customWebClient.Item.Index != customWebClient.Index)
                        {
                            customWebClient.Index = customWebClient.Item.Index;

                            // move the progress bar down one
                            customWebClient.ProgBar.Top -= 18;
                        }

                        // update the bytes received in the listViewItem
                        customWebClient.Item.SubItems[(int)subItems.Total].Text = 
                            $"{(ev.BytesReceived / 1024f / 1042f).ToString("00.00")} / {(ev.TotalBytesToReceive / 1024f / 1024f).ToString("00.00")} MB";
                    }

                    // download complete 
                    void web_DownloadFileCompleted(object o, AsyncCompletedEventArgs ev)
                    {
                        // set current webclient
                        var customWebClient = o as CustomWebClient;

                        // remove the progress bar
                        customWebClient.ProgBar.Dispose();

                        // removes downloaded element
                        customWebClient.Item.Remove();

                        // remove events
                        customWebClient.DownloadProgressChanged -= web_DownloadProgressChanged;
                        customWebClient.DownloadFileCompleted -= web_DownloadFileCompleted;
                    }
                }
            }
        }

        // timer for loading animation and updating the stopwatch in the listview
        private void timer_Tick(object sender, EventArgs e)
        {
            // loading animation
            switch (iter)
            {
                case 0:
                    txtLoading.Text = "Loading";
                    iter++;
                    break;
                case 1:
                    txtLoading.Text = "Loading.";
                    iter++;
                    break;
                case 2:
                    txtLoading.Text = "Loading..";
                    iter++;
                    break;
                case 3:
                    txtLoading.Text = "Loading...";
                    iter++;
                    break;
                case 4:
                    txtLoading.Text = "Loading....";
                    iter++;
                    break;
                case 5:
                    txtLoading.Text = "Loading.....";
                    iter = 0;
                    break;
            }
        }

        // so the user can't selected audio and video it's one or the other
        private void btnAudio_Click(object sender, EventArgs e)
        {
            if (btnAudio.Text == "")
            {
                // easter egg
                Clipboard.SetText("https://www.youtube.com/watch?v=laBOGMG-k_c");
                BtnPaste_Click(sender, e);
                btnAudio.Text = "Swauss";
            }
            else if (btnAudio.Text != "Swauss")
            {
                btnAudio.ForeColor = Color.Black;
                comBoxVideo.SelectedIndex = -1;
                toggleSlideBar(true);
            }
        }
        private void ComBoxVideo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comBoxVideo.SelectedIndex != -1)
            {
                btnAudio.ForeColor = Color.White;
                toggleSlideBar(false);
            }
        }

        // toggle the slide bar only aviable with audio downloads
        private void toggleSlideBar(bool b)
        {
            barFirst.Enabled = b;
            barLast.Enabled = b;
            txtFirst.Visible = b;
            txtLast.Visible = b;
        }

        // remove illegal characters function
        private static string RemoveIllegalPathCharacters(string path)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(path, "");
        }

        // helper function for adding items to the listView
        private static ListViewItem LvAddItem(ref ListView listView, string filename, out ProgressBar progbar)
        {
            // make new item for the listview
            ListViewItem item = new ListViewItem(filename);

            // this is where the progress bar goes
            item.SubItems.Add(@"¯\_(ツ)_/¯");

            // Total colum
            item.SubItems.Add("00:00 / ??:??MB");

            // add it all to the listview
            listView.Items.Add(item);

            // add progress bar
            Rectangle rect = default;
            ProgressBar ProgBar = new ProgressBar();

            // Get bounds of the second colum
            rect = listView.Items[listView.Items.Count - 1].SubItems[(int)subItems.Progress].Bounds;

            // parent progressBar to the listView
            ProgBar.Parent = listView;

            // Put Prog bar In listView 
            ProgBar.SetBounds(rect.X, rect.Y, rect.Width, rect.Height);

            progbar = ProgBar;

            return item;
        }

        // listview Event so the user can't change the colum width
        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView.Columns[e.ColumnIndex].Width;
        }

        // updates text for the scroll bar
        private void barFirst_Scroll(object sender, EventArgs e)
        {
            txtFirst.Text = $"{TimeSpan.FromSeconds(barFirst.Value).ToString(@"mm\:ss")}";
        }
        private void barLast_Scroll(object sender, EventArgs e)
        {
            txtLast.Text = $"{(duration - TimeSpan.FromSeconds(barLast.Maximum - barLast.Value)).ToString(@"mm\:ss")}";
        }

        // remove the current pasted info
        private void btnX_Click(object sender, EventArgs e)
        {
            toggleThings(false);

            // clear pasted text and thumbnail
            TxtUrl.Text = "";
            Thumbnail.Image = null;
        }

        // event for expanding the scroll bar to full video lenght on button press
        private void txtFirst_Click(object sender, EventArgs e)
        {
            if (!txtFirst.AllowDrop)
                barFirst.Maximum = (int)duration.TotalSeconds;
            else
            {
                barFirst.Maximum = 100;
                barFirst_Scroll(sender, e);
            }

            txtFirst.AllowDrop = !txtFirst.AllowDrop;
        }
        private void txtLast_Click(object sender, EventArgs e)
        {
            if (!txtLast.AllowDrop)
            {
                barLast.Maximum = (int)duration.TotalSeconds;
                barLast.Value = (int)duration.TotalSeconds - (100 - barLast.Value);
            }
            else
            {
                int idx = Math.Max(0, 100 - ((int)duration.TotalSeconds - barLast.Value));              
                barLast.Maximum = 100;
                barLast.Value = idx;
                barLast_Scroll(sender, e);
            }

            txtLast.AllowDrop = !txtLast.AllowDrop;
        }

        private void Thumbnail_Click(object sender, EventArgs e)
        {
            // open the thumbnail in the default web broswer
            Process.Start($"https://img.youtube.com/vi/{id}/maxresdefault.jpg");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("SWAUSS");
        }
    }
}
