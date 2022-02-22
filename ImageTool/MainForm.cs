using System.Text.Json;

namespace ImageTool
{
    public partial class MainForm : Form
    {
        string[] folderlist;
        Dictionary<string, Image> thumbnails = new Dictionary<string, Image>();
        Dictionary<string, string> assocs = new Dictionary<string, string>();
        Dictionary<string, bool> hasOutput = new Dictionary<string, bool>();
        Dictionary<string, bool> hasRedraw = new Dictionary<string, bool>();

        string curFolder;
        ImageController controller;
        ImageView? outputView;
        FormJump jumpForm;

        internal CheckBox checkBoxRepeatTexture;
        internal CheckBox checkBoxShowSelectionsOnOtherImages;

        public Dictionary<string, Image> Thumbnails { get => thumbnails; }
        public Dictionary<string, string> Assocs { get => assocs; }
        public Dictionary<string, bool> HasOutput { get => hasOutput; }
        public Dictionary<string, bool> HasRedraw { get => hasRedraw; }

        public MainForm(string[] folderlist)
        {
            InitializeComponent();

            controller = new ImageController(this);

            {
                checkBoxShowSelectionsOnOtherImages = new CheckBox();
                checkBoxShowSelectionsOnOtherImages.Checked = true;
                checkBoxShowSelectionsOnOtherImages.Text = "Show selections on all images";
                statusStrip.Items.Add(new ToolStripControlHost(checkBoxShowSelectionsOnOtherImages));
                checkBoxShowSelectionsOnOtherImages.CheckedChanged += delegate { Refresh(); };
            }
            {
                checkBoxRepeatTexture = new CheckBox();
                checkBoxRepeatTexture.Checked = false;
                checkBoxRepeatTexture.Text = "Repeat Texture";
                statusStrip.Items.Add(new ToolStripControlHost(checkBoxRepeatTexture));
                checkBoxRepeatTexture.CheckedChanged += delegate { Refresh(); };
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

            // just to silence warning
            jumpForm = new FormJump(this, folderlist, "");
        }

        int GetCurFolderId() {
            return Array.IndexOf(folderlist, curFolder);
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
            jumpForm.Refresh();
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
            jumpForm.NavPrev();
        }

        private void buttonNavNext_Click(object sender, EventArgs e)
        {
            jumpForm.NavNext();
        }

        private void buttonNavJump_Click(object sender, EventArgs e)
        {
            jumpForm.Show();
        }

        private void buttonSaveOutput_Click(object sender, EventArgs e)
        {
            controller.SaveOutput(curFolder);
            hasOutput[curFolder] = true;
            hasRedraw[curFolder] = controller.HasRedrawRects;
            jumpForm.Refresh();
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

        private void buttonClearOutput_Click(object sender, EventArgs e)
        {
            controller.ClearOutput(curFolder);
            hasOutput[curFolder] = false;
            hasRedraw[curFolder] = false;
            jumpForm.Refresh();
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
Left/Right - Navigate prev/next textures
Ctrl+F - Jump to texture
Ctrl+S - Save output
Ctrl+Space - Save & next

Let Peter know if there are any missing features which would improve your workflow.";
            MessageBox.Show(string.Format(text, Program.VERSION), "PH3 Imagetool", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Left))
            {
                buttonNavPrev_Click(this, new EventArgs());
                return true;
            }
            if (keyData == (Keys.Right))
            {
                buttonNavNext_Click(this, new EventArgs());
                return true;
            }
            if (keyData == (Keys.Control | Keys.F))
            {
                buttonNavJump_Click(this, new EventArgs());
                return true;
            }
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
                while (!loadingForm.Visible); // ugh

                // load thumbnails
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
                // load assocs
                var assoclines = File.ReadAllLines("assoc.txt");
                foreach(var assocline in assoclines)
                {
                    var elems = assocline.Split(" ");
                    string rest = assocline.Replace(elems[0], "").Trim();
                    assocs.Add(elems[0], rest);
                }
                // check output
                foreach (string folder in folderlist)
                {
                    var specfn = controller.GetOutputSpecFn(folder);
                    bool hasSpec = File.Exists(specfn);
                    hasOutput.Add(folder, hasSpec);
                    bool thisHasRedraw = false;
                    if (hasSpec)
                    {
                        var jsonString = File.ReadAllText(controller.GetOutputSpecFn(folder));
                        var outputSpec = JsonSerializer.Deserialize<OutputSpec>(jsonString);
                        thisHasRedraw = outputSpec.RedrawRects.Count > 0;
                    }
                    hasRedraw.Add(folder, thisHasRedraw);
                }
                // done
                loadingForm.Invoke(delegate { loadingForm.Close(); });
            }));
            thumbThread.Start();

            loadingForm.ShowDialog();

            LoadFolder(folderlist.FirstOrDefault(""));

            jumpForm = new FormJump(this, folderlist, curFolder);
        }
    }
}