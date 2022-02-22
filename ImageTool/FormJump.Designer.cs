namespace ImageTool
{
    partial class FormJump
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
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.labelFilter = new System.Windows.Forms.Label();
            this.listBox = new System.Windows.Forms.ListBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonGo = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxSearchAssoc = new System.Windows.Forms.CheckBox();
            this.checkBoxFilterOutput = new System.Windows.Forms.CheckBox();
            this.checkBoxFilterRedraw = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilter.Location = new System.Drawing.Point(93, 12);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(258, 23);
            this.textBoxFilter.TabIndex = 0;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.FilterChanged);
            // 
            // labelFilter
            // 
            this.labelFilter.Location = new System.Drawing.Point(12, 12);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(75, 23);
            this.labelFilter.TabIndex = 1;
            this.labelFilter.Text = "Text Filter:";
            this.labelFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // listBox
            // 
            this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox.ContextMenuStrip = this.contextMenuStrip;
            this.listBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox.ItemHeight = 40;
            this.listBox.Location = new System.Drawing.Point(12, 66);
            this.listBox.Name = "listBox";
            this.listBox.ScrollAlwaysVisible = true;
            this.listBox.Size = new System.Drawing.Size(555, 564);
            this.listBox.TabIndex = 2;
            this.listBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_DrawItem);
            this.listBox.DoubleClick += new System.EventHandler(this.listBox_DoubleClick);
            this.listBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox_MouseDown);
            this.listBox.Resize += new System.EventHandler(this.listBox_Resize);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOpenFolder});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(202, 48);
            // 
            // toolStripMenuItemOpenFolder
            // 
            this.toolStripMenuItemOpenFolder.Name = "toolStripMenuItemOpenFolder";
            this.toolStripMenuItemOpenFolder.Size = new System.Drawing.Size(201, 22);
            this.toolStripMenuItemOpenFolder.Text = "Open Containing Folder";
            this.toolStripMenuItemOpenFolder.Click += new System.EventHandler(this.toolStripMenuItemOpenFolder_Click);
            // 
            // buttonGo
            // 
            this.buttonGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGo.Location = new System.Drawing.Point(492, 634);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(75, 23);
            this.buttonGo.TabIndex = 3;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.Location = new System.Drawing.Point(12, 634);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // checkBoxSearchAssoc
            // 
            this.checkBoxSearchAssoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxSearchAssoc.AutoSize = true;
            this.checkBoxSearchAssoc.Checked = true;
            this.checkBoxSearchAssoc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSearchAssoc.Location = new System.Drawing.Point(365, 14);
            this.checkBoxSearchAssoc.Name = "checkBoxSearchAssoc";
            this.checkBoxSearchAssoc.Size = new System.Drawing.Size(202, 19);
            this.checkBoxSearchAssoc.TabIndex = 5;
            this.checkBoxSearchAssoc.Text = "Also search in associated textures";
            this.checkBoxSearchAssoc.UseVisualStyleBackColor = true;
            this.checkBoxSearchAssoc.CheckedChanged += new System.EventHandler(this.FilterChanged);
            // 
            // checkBoxFilterOutput
            // 
            this.checkBoxFilterOutput.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.checkBoxFilterOutput.AutoSize = true;
            this.checkBoxFilterOutput.Location = new System.Drawing.Point(121, 41);
            this.checkBoxFilterOutput.Name = "checkBoxFilterOutput";
            this.checkBoxFilterOutput.Size = new System.Drawing.Size(131, 19);
            this.checkBoxFilterOutput.TabIndex = 6;
            this.checkBoxFilterOutput.Text = "Filter \"With Output\"";
            this.checkBoxFilterOutput.ThreeState = true;
            this.checkBoxFilterOutput.UseVisualStyleBackColor = true;
            this.checkBoxFilterOutput.CheckStateChanged += new System.EventHandler(this.FilterChanged);
            // 
            // checkBoxFilterRedraw
            // 
            this.checkBoxFilterRedraw.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.checkBoxFilterRedraw.AutoSize = true;
            this.checkBoxFilterRedraw.Location = new System.Drawing.Point(283, 41);
            this.checkBoxFilterRedraw.Name = "checkBoxFilterRedraw";
            this.checkBoxFilterRedraw.Size = new System.Drawing.Size(177, 19);
            this.checkBoxFilterRedraw.TabIndex = 7;
            this.checkBoxFilterRedraw.Text = "Filter \"With Redraw Regions\"";
            this.checkBoxFilterRedraw.ThreeState = true;
            this.checkBoxFilterRedraw.UseVisualStyleBackColor = true;
            this.checkBoxFilterRedraw.CheckStateChanged += new System.EventHandler(this.FilterChanged);
            // 
            // FormJump
            // 
            this.AcceptButton = this.buttonGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(579, 661);
            this.ControlBox = false;
            this.Controls.Add(this.checkBoxFilterRedraw);
            this.Controls.Add(this.checkBoxFilterOutput);
            this.Controls.Add(this.checkBoxSearchAssoc);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.labelFilter);
            this.Controls.Add(this.textBoxFilter);
            this.Name = "FormJump";
            this.Text = "Jump to Texture";
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox textBoxFilter;
        private Label labelFilter;
        private ListBox listBox;
        private Button buttonGo;
        private Button buttonCancel;
        private CheckBox checkBoxSearchAssoc;
        private CheckBox checkBoxFilterOutput;
        private CheckBox checkBoxFilterRedraw;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem toolStripMenuItemOpenFolder;
    }
}