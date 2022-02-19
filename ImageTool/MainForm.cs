namespace ImageTool
{
    public partial class MainForm : Form
    {
        string[] folderlist;
        Dictionary<string,Image> thumbnails;
        string curFolder;
        ImageController controller;
        ImageView? outputView;

        public MainForm(string[] folderlist)
        {
            InitializeComponent();

            controller = new ImageController(this);

            {
                CheckBox checkBoxShowSelectionsOnOtherImages = new CheckBox();
                checkBoxShowSelectionsOnOtherImages.Checked = true;
                checkBoxShowSelectionsOnOtherImages.Text = "Show selections on all images";
                statusStrip.Items.Add(new ToolStripControlHost(checkBoxShowSelectionsOnOtherImages));
                checkBoxShowSelectionsOnOtherImages.CheckedChanged += delegate { controller.ShowSelectionsOnAll = checkBoxShowSelectionsOnOtherImages.Checked; };
            }
            {
                CheckBox checkBoxRepeatTexture = new CheckBox();
                checkBoxRepeatTexture.Checked = false;
                checkBoxRepeatTexture.Text = "Repeat Texture";
                statusStrip.Items.Add(new ToolStripControlHost(checkBoxRepeatTexture));
                checkBoxRepeatTexture.CheckedChanged += delegate { controller.RepeatTexture = checkBoxRepeatTexture.Checked; };
            }
            {
                Button buttonChangeBg = new Button();
                buttonChangeBg.Text = "Change BG";
                statusStrip.Items.Add(new ToolStripControlHost(buttonChangeBg));
                buttonChangeBg.Click += delegate {
                    controller.ChangeBG();
                    Refresh();
                };
            }

            this.folderlist = folderlist;
            curFolder = "";
            thumbnails = new Dictionary<string, Image>();
        }

        int GetCurFolderId() {
            return Array.IndexOf(folderlist, curFolder);
        }
        string GetNextFolder() {
            return folderlist[Math.Min(GetCurFolderId()+1, folderlist.Length-1)];
        }
        string GetPrevFolder()
        {
            return folderlist[Math.Max(GetCurFolderId() - 1, 0)];
        }

        public void LoadFolder(string folder)
        {
            curFolder = folder;
            controller = new ImageController(this);

            var fn = curFolder;
            var images = Directory.EnumerateFiles(fn).Where(f => f.EndsWith(".png"));
            
            flowLayoutPanel.Controls.Clear();
            foreach (var image in images)
            {
                if (Path.GetFileNameWithoutExtension(image) == "output") continue;
                var view = new ImageView(image, controller);
                controller.AddView(view);
                flowLayoutPanel.Controls.Add(view);
            }
            outputView = new ImageView(controller);
            flowLayoutPanel.Controls.Add(outputView);
            flowLayoutPanel.Update();

            controller.LoadOutput(curFolder);
            Text = String.Format("PH3 ImageTool - {0} [{1}/{2}]", curFolder, GetCurFolderId()+1, folderlist.Length);
            MainForm_Resize(this, new EventArgs());

            Refresh();
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

        private void MainForm_Resize(object sender, EventArgs e)
        {
            flowLayoutPanel.SuspendLayout();

            const int NUM_ROWS = 2; // fix to two rows for now
            const int SPACING = 2;

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

        // Buttons, left to right

        private void buttonNavPrev_Click(object sender, EventArgs e)
        {
            LoadFolder(GetPrevFolder());
        }

        private void buttonNavNext_Click(object sender, EventArgs e)
        {
            LoadFolder(GetNextFolder());
        }

        private void buttonNavJump_Click(object sender, EventArgs e)
        {
            var form = new FormJump(this, folderlist, curFolder, thumbnails);
            form.ShowDialog();
        }

        private void buttonSaveOutput_Click(object sender, EventArgs e)
        {
            controller.SaveOutput(curFolder);
        }

        private void buttonSaveAndNext_Click(object sender, EventArgs e)
        {
            buttonSaveOutput_Click(sender, e);
            buttonNavNext_Click(sender, e);
        }

        private void buttonLoadOutputSpec_Click(object sender, EventArgs e)
        {
            controller.LoadOutput(curFolder);
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            string text = @"PH3 ImageTool {0}

Controls - Mouse:
LMB drag - pan images
Wheel - zoom (centered on cursor)
RMB drag - define selection area from this source
MMB drag - define redraw area
RMB/MMB click - delete respective areas

Controls - Keyboard:
Ctrl+S - Save Output
Ctrl+Space - Save & Next

Let Peter know if there are any missing features which would improve your workflow.";
            MessageBox.Show(string.Format(text, Program.VERSION), "PH3 Imagetool", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                buttonSaveOutput_Click(this, new EventArgs());
                return true;
            }
            if (keyData == (Keys.Control | Keys.Space))
            {
                buttonSaveAndNext_Click(this, new EventArgs());
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            var loadingForm = new LoadingForm();
            loadingForm.StartPosition = FormStartPosition.CenterParent;

            var thumbThread = new Thread(new ThreadStart(delegate
            {
                while (!loadingForm.Visible);

                for (int i = 0; i < folderlist.Length; i++)
                {
                    var imgfn = folderlist[i] + "/original_psp.png";
                    if (File.Exists(imgfn))
                    {
                        thumbnails.Add(folderlist[i], Image.FromFile(imgfn));
                    }
                    loadingForm.Invoke(delegate {
                        loadingForm.SetProgress(i / (float)folderlist.Length); 
                        loadingForm.Refresh(); 
                    });
                }
                loadingForm.Invoke(delegate { loadingForm.Close(); });
            }));
            thumbThread.Start();

            loadingForm.ShowDialog();

            LoadFolder(folderlist.FirstOrDefault(""));
        }
    }
}