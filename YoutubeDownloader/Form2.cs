using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoutubeDownloader
{
    public partial class Form2 : Form
    {
        private List<string> list;

        private int listCount;

        public Form2()
        {
            InitializeComponent();

            TxtfileDir.Text = "";
        }

        // Select the file and display it on the listview
        private void BtnFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                TxtfileDir.Text = folderBrowserDialog1.SelectedPath;

                string[] AllFiles = Directory.GetFiles(TxtfileDir.Text);

                list = new List<string>();

                for (int i = 0; i < AllFiles.Length; i++)
                {
                    if (AllFiles[i].Contains(".mp3"))
                        list.Add(AllFiles[i]);
                }

                listCount = list.Count;

                AddArraytoListView();

                BtnRand.Enabled = true;
                BtnConfirm.Enabled = true;
                BtnClear.Enabled = true;
            }
        }

        // Randomize the list
        private void BtnRand_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            for (int i = 0; i < listCount * 2; i++)
            {
                int randNum1 = random.Next(0, listCount);
                int randNum2 = random.Next(0, listCount);

                string temp = list[randNum1];
                list[randNum1] = list[randNum2];
                list[randNum2] = temp;
            }

            AddArraytoListView();
        }

        // Display the list on the control
        private void AddArraytoListView()
        {
            listView1.Items.Clear();

            for (int i = 0; i < listCount; i++)
            {
                ListViewItem item = new ListViewItem(Path.GetFileName(list[i]));
                item.SubItems.Add((i + 1).ToString());
                listView1.Items.Add(item);
            }
        }

        // Apply the changes
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listCount; i++)
            {
                TagLib.File f = TagLib.File.Create(list[i]);
                f.Tag.Track = (uint)i + 1;
                f.Save();
            }
        }

        // Clear all the numbers
        private void BtnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listCount; i++)
            {
                TagLib.File f = TagLib.File.Create(list[i]);
                f.Tag.Track = 0;
                f.Save();
            }
        }

        // listview Event so the user can't change the colum width
        private void listView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
        }
    }
}
