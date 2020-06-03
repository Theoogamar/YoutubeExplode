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
using YoutubeExplode.Models.MediaStreams;
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

        // stores the vidoe lenght
        private TimeSpan duration;

        // enum for listView
        private enum subItems
        {
            fileName,
            Progress,
            Total,
            Media
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
            if (YoutubeClient.TryParseVideoId(TxtUrl.Text, out string id))
            {
                // tell the user it's loading
                iter = 0;
                txtLoading.Visible = true;

                // media stream info
                MediaStreamInfoSet streamInfo = null;

                try
                {
                    // Client
                    YoutubeClient client = new YoutubeClient();

                    // Get media stream info
                    streamInfo = await client.GetVideoMediaStreamInfosAsync(id);

                    // Set video tile to text box
                    var video = await client.GetVideoAsync(id);
                    txtBoxVidName.Text = video.Title;

                    // get duration and set the duration
                    duration = video.Duration;
                    txtLast.Text = duration.ToString(@"mm\:ss");
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

                // Set thimbnail pic to picture box
                string picId = "https://i.ytimg.com/vi/" + id + "/hqdefault.jpg";
                Thumbnail.LoadAsync(picId);

                // get the highest bitrate audio stream with out vorbis
                var Audio = streamInfo.Audio.GetHighestBitrate(out int highest, AudioEncoding.Vorbis);

                if (Audio == null)
                {
                    // if the YoutubeClient fails to get any audio
                    txtLoading.Text = "There's no audio";
                    txtLoading.Visible = true;
                    iter = -1;
                    btnPaste.Enabled = true;
                    return;
                }

                // add the highest quailty audio to the list
                if (highest == 0)
                    btnAudio.Text = $"Highest bitrate: {Audio.Bitrate / 1000}kbps .mp3";
                else
                    btnAudio.Text = $"{highest + 1}nd Highest bitrate: {Audio.Bitrate / 1000}kbps .mp3";

                // save the url for downloading
                audioUrl = Audio.Url;

                // save the bitrate for downloading
                Bitrate = (int)Audio.Bitrate;

                // clear and make a new list
                videoUrls = new List<string>();

                // Set all video things
                for (int i = 0; i < streamInfo.Muxed.Count; i++)
                {
                    // add video info to combobox
                    comBoxVideo.Items.Add($"Resolution: {streamInfo.Muxed[i].VideoQualityLabel} .{streamInfo.Muxed[i].Container}");

                    // add download url to string arry
                    videoUrls.Add(streamInfo.Muxed[i].Url);
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
                barLast.Value = 100;
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
                using (CustomBackgroundWorker Worker = new CustomBackgroundWorker())
                {
                    // copy the cut values off the UI thread
                    int first = barFirst.Value;
                    int last = barLast.Maximum - barLast.Value;

                    // reset the video settings in the UI
                    toggleThings(false);
                    Refresh();

                    // adds a donwload item to the listview
                    Worker.Progbar = LvAddItem(ref listView, fileName, true, "a");

                    // the position in the listView to put the percentage downloaded
                    Worker.Index = listView.Items.Count - 1;

                    // starting the thread
                    Worker.DoWork += worker_DoWork;
                    Worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                    Worker.Name = fileName;
                    Worker.RunWorkerAsync();

                    void worker_DoWork(object o, DoWorkEventArgs ev)
                    {
                        try
                        {
                            // download the auido from url
                            using (var reader = new MediaFoundationReader(audioUrl))
                            {
                                // set up the encoder for mp3 file formate
                                var mediaType = MediaFoundationEncoder.SelectMediaType(AudioSubtypes.MFAudioFormat_MP3, reader.WaveFormat, Bitrate);
                                if (mediaType == null) throw new InvalidOperationException("No suitable MP3 encoders available");
                                var audio = new MediaFoundationEncoder(mediaType);

                                audio.LoadProgressChanged += encoder_LoadProgressChanged;
                                audio.LoadComplete += encoder_LoadComplete;

                                // encode it to mp3
                                audio.Encode($"{FilePath}{fileName}.mp3", reader, first, last);

                                void encoder_LoadProgressChanged(object obj, LoadProgressChangedEventArgs evt)
                                {
                                    // set current worker
                                    var customWorker = o as CustomBackgroundWorker;

                                    int index = 0;

                                    // Running on the UI thread
                                    listView.Invoke((MethodInvoker)delegate {

                                        // searchs for the currents index
                                        index = getIndexByName(customWorker.Name);

                                        // update the bytes received in the listView
                                        listView.Items[customWorker.Index].SubItems[(int)subItems.Total].Text =
                                            $"{evt.TimeEncoded.ToString(@"mm\:ss")} / {evt.TotalTimeToEncode.ToString(@"mm\:ss")}MB";
                                    });

                                    // Running on the UI thread
                                    customWorker.Progbar.Invoke((MethodInvoker)delegate {

                                        // set the progress on the progress bar
                                        customWorker.Progbar.Value = evt.ProgressPercentage;

                                        if (index != customWorker.Index)
                                        {
                                            customWorker.Index = index;

                                            // move the progress bar down one
                                            customWorker.Progbar.Top -= 18;
                                        }
                                    });
                                }

                                void encoder_LoadComplete(object obj, EventArgs evt)
                                {
                                    audio.LoadProgressChanged -= encoder_LoadProgressChanged;
                                    audio.LoadComplete -= encoder_LoadComplete;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // if something messes up tell the user and return
                            MessageBox.Show(ex.Message);
                            return;
                        }
                    }

                    void worker_RunWorkerCompleted(object o, RunWorkerCompletedEventArgs ev)
                    {
                        var customWorker = o as CustomBackgroundWorker;

                        // remove the progress bar
                        customWorker.Progbar.Dispose();

                        // get the index of a listview item by name
                        int index = getIndexByName(customWorker.Name);

                        // remove the listview item and stopwatch by the index
                        listView.Items.RemoveAt(index);
                        //stopwatches.RemoveAt(index);

                        // remove events
                        customWorker.DoWork -= worker_DoWork;
                        customWorker.RunWorkerCompleted -= worker_RunWorkerCompleted;
                    }
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
                    web.Progbar = LvAddItem(ref listView, fileName, true, "v");

                    // add events
                    web.DownloadProgressChanged += web_DownloadProgressChanged;
                    web.DownloadFileCompleted += web_DownloadFileCompleted;

                    // sets the name so i can search for the index
                    web.Name = fileName;

                    // the position in the listView to put the percentage downloaded
                    web.Index = listView.Items.Count - 1;

                    // download the video file
                    await web.DownloadFileTaskAsync(url, FilePath + fileName + codec);

                    // displaying the download progress
                    void web_DownloadProgressChanged(object o, DownloadProgressChangedEventArgs ev)
                    {
                        // set current webclient
                        var customWebClient = o as CustomWebClient;

                        // set the progress on the progress bar
                        customWebClient.Progbar.Value = ev.ProgressPercentage;

                        // searchs for the currents index
                        int index = getIndexByName(customWebClient.Name);
                        if (index != customWebClient.Index)
                        {
                            customWebClient.Index = index;

                            // move the progress bar down one
                            customWebClient.Progbar.Top -= 18;
                        }

                        // update the bytes received in the listView
                        listView.Items[customWebClient.Index].SubItems[(int)subItems.Total].Text =
                            (ev.BytesReceived / 1024f / 1042f).ToString("00.00") + " / " +
                            (ev.TotalBytesToReceive / 1024f / 1024f).ToString("00.00") + "MB";
                    }

                    // download complete 
                    void web_DownloadFileCompleted(object o, AsyncCompletedEventArgs ev)
                    {
                        // set current webclient
                        var customWebClient = o as CustomWebClient;

                        // remove the progress bar
                        customWebClient.Progbar.Dispose();

                        // removes downloaded element
                        listView.Items.RemoveAt(customWebClient.Index);

                        // remove events
                        customWebClient.DownloadProgressChanged -= web_DownloadProgressChanged;
                        customWebClient.DownloadFileCompleted -= web_DownloadFileCompleted;
                    }
                }
            }

            // getting the index of a listview item by name
            int getIndexByName(string name)
            {
                for (int i = 0; i < listView.Items.Count; i++)
                    if (listView.Items[i].SubItems[(int)subItems.fileName].Text == name)
                        return i;
                return 0;
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
                    txtLoading.Text = @"Loading...";
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

        // wraper function for adding items to the listView
        private static ProgressBar LvAddItem(ref ListView listView, string filename, bool progressBar, string media)
        {
            // make new item for the listview
            ListViewItem item = new ListViewItem(filename);

            // this is where the progress bar goes
            item.SubItems.Add(@"¯\_(ツ)_/¯");

            // Total colum
            item.SubItems.Add("00:00 / ??:??MB");

            // is audio or video
            item.SubItems.Add(media);

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

            return ProgBar;
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

            // clear pasted text
            TxtUrl.Text = "";
        }
    }
}
