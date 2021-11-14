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
                flowLayoutPanel.Controls.Add(new ImageView(image, controller));
            }
            outputView = new ImageView(controller);
            flowLayoutPanel.Controls.Add(outputView);
            flowLayoutPanel.Update();
        }

        private void statusStrip_Paint(object sender, PaintEventArgs e)
        {
            toolStripStatusLabel.Text = controller.GetMousePosString();
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
    }
}