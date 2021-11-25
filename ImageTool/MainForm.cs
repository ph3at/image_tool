namespace ImageTool
{
    public partial class MainForm : Form
    {
        string[] folderlist;
        int curFolder = 0;
        ImageController controller;
        ImageView? outputView;

        public MainForm(string[] folderlist)
        {
            InitializeComponent();

            controller = new ImageController(this);

            CheckBox checkBoxShowSelectionsOnOtherImages = new CheckBox();
            checkBoxShowSelectionsOnOtherImages.Checked = true;
            checkBoxShowSelectionsOnOtherImages.Text = "Show selections on all images";
            statusStrip.Items.Add(new ToolStripControlHost(checkBoxShowSelectionsOnOtherImages));
            checkBoxShowSelectionsOnOtherImages.CheckedChanged += delegate { controller.ShowSelectionsOnAll = checkBoxShowSelectionsOnOtherImages.Checked; };

            this.folderlist = folderlist;
            LoadFolder();
        }

        public void LoadFolder()
        {
            var fn = folderlist[curFolder];
            var images = Directory.EnumerateFiles(fn).Where(f => f.EndsWith(".png"));
            
            flowLayoutPanel.Controls.Clear();
            foreach (var image in images)
            {
                if (Path.GetFileNameWithoutExtension(image) == "output") continue;
                flowLayoutPanel.Controls.Add(new ImageView(image, controller));
            }
            outputView = new ImageView(controller);
            flowLayoutPanel.Controls.Add(outputView);
            flowLayoutPanel.Update();

            MainForm_Resize(this, new EventArgs());
        }

        private void statusStrip_Paint(object sender, PaintEventArgs e)
        {
            toolStripStatusLabel.Text = controller.GetStatusString();
        }

        internal void RefreshStatus()
        {
            statusStrip.Refresh();
        }

        internal void RefreshOutput()
        {
            if(outputView != null) outputView.Refresh();
        }

        private void buttonChangeBG_Click(object sender, EventArgs e)
        {
            controller.ChangeBG();
            Refresh();
        }

        private void buttonSaveOutput_Click(object sender, EventArgs e)
        {
            controller.OutputImage.Save(folderlist[curFolder] + "/output.png");
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            flowLayoutPanel.SuspendLayout();

            const int NUM_ROWS = 2; // fix to two rows for now
            const int SPACING = 5;

            var num = flowLayoutPanel.Controls.Count;
            var numPerRow = (int)Math.Ceiling(num / (float)NUM_ROWS);
            foreach (Control c in flowLayoutPanel.Controls)
            {
                c.Width = (flowLayoutPanel.Width / numPerRow) - SPACING*2;
                c.Height = (flowLayoutPanel.Height / NUM_ROWS) - SPACING*2;
                c.Margin = new Padding(SPACING);
            }
            if(numPerRow*NUM_ROWS != num && outputView != null)
            {
                outputView.Width *= 2;
                outputView.Width += SPACING*2;
            }

            flowLayoutPanel.ResumeLayout();
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}