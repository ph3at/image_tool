using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageTool
{
    public partial class FormJump : Form
    {
        MainForm mainForm;
        string[] rootFolderList;
        string[] folderList;
        Dictionary<string, bool> hasOutput;
        Dictionary<string, Image> thumbnails;

        Font font;

        public FormJump(MainForm mainForm, string[] folderList, string selectedFolder, Dictionary<string, Image> thumbnails)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            rootFolderList = folderList.Select(x => x.ToString()).ToArray();
            this.folderList = folderList;
            this.thumbnails = thumbnails;

            font = new Font(Font.Name, 14);

            hasOutput = new Dictionary<string, bool>();
            foreach(string folder in folderList)
            {
                hasOutput.Add(folder, File.Exists(folder + "/output.png"));
            }

            listBox.Items.AddRange(folderList);
            listBox.SelectedItem = selectedFolder;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            if(listBox.SelectedItems.Count == 1)
            {
                mainForm.LoadFolder((string)listBox.SelectedItem);
            }
            Close();
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            string folder = folderList[e.Index];

            e.Graphics.FillRectangle(selected ? SystemBrushes.Highlight : SystemBrushes.ControlLight, e.Bounds);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.LineAlignment = StringAlignment.Center;
            var stringRect = e.Bounds;
            stringRect.X = 40;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            e.Graphics.DrawString(folderList[e.Index], font, Brushes.Black, stringRect, stringFormat);

            if (hasOutput[folder]) {
                stringRect.X = listBox.Width-62;
                e.Graphics.DrawString("☑", font, Brushes.Black, stringRect, stringFormat);
            }

            var s = listBox.ItemHeight;
            var img = thumbnails[folder];
            var imgRect = new Rectangle(0, e.Bounds.Y, Math.Min(s*img.Width/img.Height, s), s);
            e.Graphics.DrawImage(img,  imgRect);
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            folderList = rootFolderList.Where(f => f.Contains(textBoxFilter.Text)).ToArray();
            listBox.Items.Clear();
            listBox.Items.AddRange(folderList);
        }

        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            buttonGo_Click(sender, e);
        }
    }
}
