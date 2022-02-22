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

        Font font, smallFont;

        public FormJump(MainForm mainForm, string[] folderList, string selectedFolder)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            rootFolderList = folderList.Select(x => x.ToString()).ToArray();
            this.folderList = folderList;

            font = new Font(Font.Name, 14);
            smallFont = new Font(Font.Name, 7);

            listBox.Items.AddRange(folderList);
            listBox.SelectedItem = selectedFolder;
        }

        private void applySelect()
        {
            mainForm.LoadFolder((string)listBox.SelectedItem);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            applySelect();
            Hide();
        }

        private void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            string folder = folderList[e.Index];

            e.Graphics.FillRectangle(selected ? SystemBrushes.Highlight : SystemBrushes.ControlLight, e.Bounds);
            var bot = e.Bounds.Y;
            e.Graphics.DrawLine(SystemPens.ControlDarkDark, e.Bounds.X, bot, e.Bounds.Width, bot);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.LineAlignment = StringAlignment.Center;
            var stringRect = e.Bounds;
            stringRect.X = 40;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            e.Graphics.DrawString(folderList[e.Index], font, Brushes.Black, stringRect, stringFormat);

            if (mainForm.HasOutput[folder]) {
                stringRect.X = listBox.Width-62;
                e.Graphics.DrawString("☑", font, Brushes.Black, stringRect, stringFormat);
            }

            stringRect.X = (int)e.Graphics.MeasureString(folderList[e.Index], font).Width + 45;
            stringRect.Width = listBox.Width - stringRect.X - 80;
            e.Graphics.DrawString(mainForm.Assocs[folder], smallFont, Brushes.Black, stringRect, stringFormat);

            var s = listBox.ItemHeight;
            var img = mainForm.Thumbnails[folder];
            var imgRect = new Rectangle(0, e.Bounds.Y, Math.Min(s*img.Width/img.Height, s), s);
            e.Graphics.DrawImage(img,  imgRect);
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            folderList = rootFolderList.Where(f => 
                f.Contains(textBoxFilter.Text) || (checkBoxSearchAssoc.Checked && mainForm.Assocs[f].Contains(textBoxFilter.Text))
            ).ToArray();
            listBox.Items.Clear();
            listBox.Items.AddRange(folderList);
        }

        private void listBox_Resize(object sender, EventArgs e)
        {
            listBox.Refresh();
        }

        private void listBox_DoubleClick(object sender, EventArgs e)
        {
            applySelect();
        }

        internal void NavPrev()
        {
            var idx = listBox.SelectedIndex;
            if (idx > 0) listBox.SelectedIndex--;
            applySelect();
        }

        private void checkBoxSearchAssoc_CheckedChanged(object sender, EventArgs e)
        {
            textBoxFilter_TextChanged(sender, e);
        }

        internal void NavNext()
        {
            var idx = listBox.SelectedIndex;
            if (idx < listBox.Items.Count-1) listBox.SelectedIndex++;
            applySelect();
        }
    }
}
