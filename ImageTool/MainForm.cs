using System.Text.Json;

namespace ImageTool
{
    public partial class MainForm : Form
    {
        string[] folderlist;
        Dictionary<string, Image> thumbnails = new Dictionary<string, Image>();
        Dictionary<string, string> assocs = new Dictionary<string, string>();
        Dictionary<string, string> names = new Dictionary<string, string>();
        Dictionary<string, bool> hasAlpha = new Dictionary<string, bool>();
        Dictionary<string, bool> hasOutput = new Dictionary<string, bool>();
        Dictionary<string, bool> hasRedraw = new Dictionary<string, bool>();

        string curFolder;
        ImageController controller;
        ImageView? outputView;
        FormJump jumpForm;

        ITPOptions itpOptions = new ITPOptions();
        ITPForm itpForm;

        internal CheckBox checkBoxRepeatTexture;
        internal CheckBox checkBoxMirrorTexture;
        internal CheckBox checkBoxShowSelectionsOnOtherImages;
        internal CheckBox checkBoxVisualizeAlpha;
        internal NumericUpDown numericUpDownVisualizeAlpha;

        public Dictionary<string, Image> Thumbnails { get => thumbnails; }
        public Dictionary<string, string> Assocs { get => assocs; }
        public Dictionary<string, string> Names { get => names; }
        public Dictionary<string, bool> HasOutput { get => hasOutput; }
        public Dictionary<string, bool> HasRedraw { get => hasRedraw; }
        public Dictionary<string, bool> HasAlpha { get => hasAlpha; }

        public string CurFolder { get { return curFolder; } }
        public string[] FolderList { get { return folderlist; } }

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
            statusStrip.Items.Add(new ToolStripSeparator());
            {
                checkBoxRepeatTexture = new CheckBox();
                checkBoxRepeatTexture.Checked = false;
                checkBoxRepeatTexture.Text = "Repeat Texture";
                statusStrip.Items.Add(new ToolStripControlHost(checkBoxRepeatTexture));
            }
            {
                checkBoxMirrorTexture = new CheckBox();
                checkBoxMirrorTexture.Checked = false;
                checkBoxMirrorTexture.Text = "Mirror Texture";
                statusStrip.Items.Add(new ToolStripControlHost(checkBoxMirrorTexture));
            }
            bool handleEvent = true;
            EventHandler textureDelegate = delegate(object? sender, System.EventArgs eventArgs) {
                if (handleEvent)
                {
                    handleEvent = false;
                    if (sender == checkBoxMirrorTexture) checkBoxRepeatTexture.Checked = false;
                    else checkBoxMirrorTexture.Checked = false;
                    Refresh();
                }
                handleEvent = true;
            };
            checkBoxRepeatTexture.CheckedChanged += textureDelegate;
            checkBoxMirrorTexture.CheckedChanged += textureDelegate;
            statusStrip.Items.Add(new ToolStripSeparator());
            {
                Button buttonChangeBg = new Button();
                buttonChangeBg.Text = "Change BG";
                statusStrip.Items.Add(new ToolStripControlHost(buttonChangeBg));
                buttonChangeBg.Click += delegate {
                    controller.ChangeBG();
                    Refresh();
                };
            }
            {
                EventHandler alphaVisDelegate = delegate (object? sender, System.EventArgs eventArgs)
                {
                    controller.RedrawOutputImage();
                };

                checkBoxVisualizeAlpha = new CheckBox();
                checkBoxVisualizeAlpha.Checked = true;
                checkBoxVisualizeAlpha.Text = "Visualize Alpha";
                checkBoxVisualizeAlpha.Padding = new Padding(10, 0, 0, 0);
                statusStrip.Items.Add(new ToolStripControlHost(checkBoxVisualizeAlpha));
                checkBoxVisualizeAlpha.CheckedChanged += alphaVisDelegate;

                numericUpDownVisualizeAlpha = new NumericUpDown();
                numericUpDownVisualizeAlpha.Value = 25;
                numericUpDownVisualizeAlpha.Minimum = 0;
                numericUpDownVisualizeAlpha.Maximum = 255;
                statusStrip.Items.Add(new ToolStripControlHost(numericUpDownVisualizeAlpha));
                numericUpDownVisualizeAlpha.ValueChanged += alphaVisDelegate;
            }

            this.folderlist = folderlist;
            curFolder = "";

            itpOptions.Load();
            itpForm = new ITPForm(this, itpOptions);

            // just to silence warning
            jumpForm = new FormJump(this, folderlist, "");
        }

        int GetCurFolderId() {
            return Array.IndexOf(folderlist, curFolder);
        }

        public void LoadFolder(string folder)
        {
            curFolder = folder;
            controller.Dispose();
            controller = new ImageController(this);

            var fn = curFolder;
            var images = Directory.EnumerateFiles(fn).Where(f => f.EndsWith(".png")).ToArray();
            Array.Sort(images);
            
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
            jumpForm.BringToFront();
        }

        private void buttonITP_Click(object sender, EventArgs e)
        {
            itpForm.Show();
            itpForm.BringToFront();
        }

        private void buttonSaveOutput_Click(object sender, EventArgs e)
        {
            controller.SaveOutput(curFolder);

            if (itpOptions.AutoSave)
            {
                itpForm.SaveCurImgToITP();
            }

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
                while (!loadingForm.Visible) ; // ugh

                // load thumbnails
                for (int i = 0; i < folderlist.Length; i++)
                {
                    var imgfn = folderlist[i] + "/original_psp.png";
                    if (File.Exists(imgfn))
                    {
                        // use filestream instead of Image.FromFile so the Image doesn't *hold the file handle* WTH MS
                        using (FileStream fs = new FileStream(imgfn, FileMode.Open))
                        {
                            thumbnails.Add(folderlist[i], Image.FromStream(fs));
                        }
                    }
                    loadingForm.Invoke(delegate {
                        loadingForm.SetProgress(i / (float)folderlist.Length);
                        loadingForm.Refresh();
                    });
                }
                // done
                loadingForm.Invoke(delegate { loadingForm.Close(); });
            }));
            thumbThread.Start();

            loadingForm.ShowDialog();

            ReloadMetainformation();

            LoadFolder(folderlist.FirstOrDefault(""));

            jumpForm = new FormJump(this, folderlist, curFolder);
        }

        internal void ReloadMetainformation()
        {
            assocs.Clear();
            names.Clear();
            hasOutput.Clear();
            hasRedraw.Clear();

            // load assocs
            var assoclines = File.ReadAllLines("assoc.txt");
            foreach (var assocline in assoclines)
            {
                var elems = assocline.Split(" ");
                string rest = assocline.Replace(elems[0], "").Trim();
                assocs.Add(elems[0], rest);
            }
            // load names
            var namelines = File.ReadAllLines("names.txt");
            foreach (var nameline in namelines)
            {
                var elems = nameline.Split(" ");
                names.Add(elems[0], elems[1]);
            }
            // load alpha
            var alphalines = File.ReadAllLines("alpha.txt");
            foreach (var alphaline in alphalines)
            {
                var elems = alphaline.Split(" ");
                bool alpha = Boolean.Parse(elems[1]);
                hasAlpha.Add(elems[0], alpha);
            }
            // check output
            foreach (string folder in folderlist)
            {
                var specfn = ImageController.GetOutputSpecFn(folder);
                bool hasSpec = File.Exists(specfn);
                hasOutput.Add(folder, hasSpec);
                bool thisHasRedraw = false;
                if (hasSpec)
                {
                    var jsonString = File.ReadAllText(ImageController.GetOutputSpecFn(folder));
                    var outputSpec = JsonSerializer.Deserialize<OutputSpec>(jsonString);
                    thisHasRedraw = outputSpec.RedrawRects.Count > 0;
                }
                hasRedraw.Add(folder, thisHasRedraw);
            }
        }

        private void buttonAlphaFix_Click(object sender, EventArgs e)
        {
            var fn = curFolder;
            var images = Directory.EnumerateFiles(fn).Where(f => f.EndsWith(".png")).ToArray();
            Array.Sort(images);

            foreach (var image in images)
            {
                if (Path.GetFileNameWithoutExtension(image) == "output") continue;
                if (Path.GetFileNameWithoutExtension(image).Contains("original")) continue;
                FileStream fs = new FileStream(image, FileMode.Open);
                Bitmap bitmap = new Bitmap(fs);
                fs.Close();
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        var p = bitmap.GetPixel(x, y);
                        int alpha = p.A;
                        int thresh = (int)numericUpDownVisualizeAlpha.Value;
                        alpha = Math.Clamp(Math.Clamp((alpha - thresh), 0, 255) * 255 / (255 - thresh - 1), 0, 255);
                        bitmap.SetPixel(x, y, Color.FromArgb(alpha, p.R, p.G, p.B));
                    }
                }
                if (!File.Exists(image + ".bak"))
                {
                    File.Move(image, image + ".bak", true);
                }
                bitmap.Save(image);
                bitmap.Dispose();
            }
            LoadFolder(curFolder);
        }

        private void buttonRestoreInputs_Click(object sender, EventArgs e)
        {
            var fn = curFolder;
            var images = Directory.EnumerateFiles(fn).Where(f => f.EndsWith(".png")).ToArray();
            Array.Sort(images);

            foreach (var image in images)
            {
                if (File.Exists(image + ".bak"))
                {
                    File.Move(image + ".bak", image, true);
                }

            }
            LoadFolder(curFolder);
        }
    }
}