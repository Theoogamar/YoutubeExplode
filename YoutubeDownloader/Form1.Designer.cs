namespace YoutubeDownloader
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnPaste = new System.Windows.Forms.Button();
            this.TxtUrl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Thumbnail = new System.Windows.Forms.PictureBox();
            this.txtBoxVidName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comBoxVideo = new System.Windows.Forms.ComboBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.txtLoading = new System.Windows.Forms.Label();
            this.listView = new System.Windows.Forms.ListView();
            this.fileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.total = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.btnAudio = new System.Windows.Forms.Button();
            this.barFirst = new System.Windows.Forms.TrackBar();
            this.barLast = new System.Windows.Forms.TrackBar();
            this.txtFirst = new System.Windows.Forms.Label();
            this.txtLast = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Thumbnail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barFirst)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barLast)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPaste
            // 
            this.btnPaste.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnPaste.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPaste.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPaste.ForeColor = System.Drawing.SystemColors.Control;
            this.btnPaste.Location = new System.Drawing.Point(15, 41);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(159, 39);
            this.btnPaste.TabIndex = 0;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = false;
            this.btnPaste.Click += new System.EventHandler(this.BtnPaste_Click);
            // 
            // TxtUrl
            // 
            this.TxtUrl.AutoSize = true;
            this.TxtUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtUrl.ForeColor = System.Drawing.SystemColors.Control;
            this.TxtUrl.Location = new System.Drawing.Point(48, 17);
            this.TxtUrl.Name = "TxtUrl";
            this.TxtUrl.Size = new System.Drawing.Size(196, 15);
            this.TxtUrl.TabIndex = 1;
            this.TxtUrl.Text = "https://www.youtube.com/watch?v=";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(12, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Link:";
            // 
            // Thumbnail
            // 
            this.Thumbnail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Thumbnail.Location = new System.Drawing.Point(448, 0);
            this.Thumbnail.Name = "Thumbnail";
            this.Thumbnail.Size = new System.Drawing.Size(256, 256);
            this.Thumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Thumbnail.TabIndex = 3;
            this.Thumbnail.TabStop = false;
            // 
            // txtBoxVidName
            // 
            this.txtBoxVidName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtBoxVidName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBoxVidName.Enabled = false;
            this.txtBoxVidName.ForeColor = System.Drawing.SystemColors.Control;
            this.txtBoxVidName.Location = new System.Drawing.Point(12, 97);
            this.txtBoxVidName.Name = "txtBoxVidName";
            this.txtBoxVidName.Size = new System.Drawing.Size(431, 20);
            this.txtBoxVidName.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(91, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Audio";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(317, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Video";
            // 
            // comBoxVideo
            // 
            this.comBoxVideo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comBoxVideo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoxVideo.Enabled = false;
            this.comBoxVideo.ForeColor = System.Drawing.SystemColors.Control;
            this.comBoxVideo.FormattingEnabled = true;
            this.comBoxVideo.Location = new System.Drawing.Point(244, 139);
            this.comBoxVideo.Name = "comBoxVideo";
            this.comBoxVideo.Size = new System.Drawing.Size(190, 21);
            this.comBoxVideo.TabIndex = 8;
            this.comBoxVideo.SelectedIndexChanged += new System.EventHandler(this.ComBoxVideo_SelectedIndexChanged);
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDownload.Enabled = false;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.ForeColor = System.Drawing.SystemColors.Control;
            this.btnDownload.Location = new System.Drawing.Point(107, 204);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(227, 44);
            this.btnDownload.TabIndex = 9;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // txtLoading
            // 
            this.txtLoading.AutoSize = true;
            this.txtLoading.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoading.ForeColor = System.Drawing.SystemColors.Control;
            this.txtLoading.Location = new System.Drawing.Point(180, 48);
            this.txtLoading.Name = "txtLoading";
            this.txtLoading.Size = new System.Drawing.Size(73, 20);
            this.txtLoading.TabIndex = 10;
            this.txtLoading.Text = "Loading";
            this.txtLoading.Visible = false;
            // 
            // listView
            // 
            this.listView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fileName,
            this.progress,
            this.total});
            this.listView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView.ForeColor = System.Drawing.SystemColors.Control;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 256);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(704, 185);
            this.listView.TabIndex = 11;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.listView1_ColumnWidthChanging);
            // 
            // fileName
            // 
            this.fileName.Text = "File Name";
            this.fileName.Width = 450;
            // 
            // progress
            // 
            this.progress.Text = "Progress";
            this.progress.Width = 100;
            // 
            // total
            // 
            this.total.Text = "Total";
            this.total.Width = 150;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 250;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // btnAudio
            // 
            this.btnAudio.BackColor = System.Drawing.SystemColors.Control;
            this.btnAudio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAudio.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAudio.Location = new System.Drawing.Point(28, 139);
            this.btnAudio.Name = "btnAudio";
            this.btnAudio.Size = new System.Drawing.Size(181, 23);
            this.btnAudio.TabIndex = 12;
            this.btnAudio.UseVisualStyleBackColor = false;
            this.btnAudio.Click += new System.EventHandler(this.btnAudio_Click);
            // 
            // barFirst
            // 
            this.barFirst.Enabled = false;
            this.barFirst.Location = new System.Drawing.Point(0, 165);
            this.barFirst.Maximum = 100;
            this.barFirst.Name = "barFirst";
            this.barFirst.Size = new System.Drawing.Size(220, 45);
            this.barFirst.TabIndex = 13;
            this.barFirst.Scroll += new System.EventHandler(this.barFirst_Scroll);
            // 
            // barLast
            // 
            this.barLast.Enabled = false;
            this.barLast.Location = new System.Drawing.Point(229, 165);
            this.barLast.Maximum = 100;
            this.barLast.Name = "barLast";
            this.barLast.Size = new System.Drawing.Size(220, 45);
            this.barLast.TabIndex = 14;
            this.barLast.Value = 100;
            this.barLast.Scroll += new System.EventHandler(this.barLast_Scroll);
            // 
            // txtFirst
            // 
            this.txtFirst.AutoSize = true;
            this.txtFirst.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirst.ForeColor = System.Drawing.SystemColors.Control;
            this.txtFirst.Location = new System.Drawing.Point(8, 213);
            this.txtFirst.Name = "txtFirst";
            this.txtFirst.Size = new System.Drawing.Size(39, 16);
            this.txtFirst.TabIndex = 15;
            this.txtFirst.Text = "00:00";
            this.txtFirst.Visible = false;
            // 
            // txtLast
            // 
            this.txtLast.AutoSize = true;
            this.txtLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLast.ForeColor = System.Drawing.SystemColors.Control;
            this.txtLast.Location = new System.Drawing.Point(395, 213);
            this.txtLast.Name = "txtLast";
            this.txtLast.Size = new System.Drawing.Size(39, 16);
            this.txtLast.TabIndex = 16;
            this.txtLast.Text = "00:00";
            this.txtLast.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(704, 441);
            this.Controls.Add(this.txtLast);
            this.Controls.Add(this.txtFirst);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.barFirst);
            this.Controls.Add(this.btnAudio);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.txtLoading);
            this.Controls.Add(this.comBoxVideo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxVidName);
            this.Controls.Add(this.Thumbnail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtUrl);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.barLast);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Youtube Downloader";
            ((System.ComponentModel.ISupportInitialize)(this.Thumbnail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barFirst)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barLast)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Label TxtUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox Thumbnail;
        private System.Windows.Forms.TextBox txtBoxVidName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comBoxVideo;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Label txtLoading;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader fileName;
        private System.Windows.Forms.ColumnHeader progress;
        private System.Windows.Forms.ColumnHeader total;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnAudio;
        private System.Windows.Forms.TrackBar barFirst;
        private System.Windows.Forms.TrackBar barLast;
        private System.Windows.Forms.Label txtFirst;
        private System.Windows.Forms.Label txtLast;
    }
}

