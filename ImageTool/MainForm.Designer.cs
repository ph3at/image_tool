namespace ImageTool
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonSaveOutput = new System.Windows.Forms.Button();
            this.buttonLoadOutputSpec = new System.Windows.Forms.Button();
            this.buttonNavPrev = new System.Windows.Forms.Button();
            this.buttonNavNext = new System.Windows.Forms.Button();
            this.buttonSaveAndNext = new System.Windows.Forms.Button();
            this.buttonNavJump = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 32);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(1584, 704);
            this.flowLayoutPanel.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 739);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1584, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            this.statusStrip.Paint += new System.Windows.Forms.PaintEventHandler(this.statusStrip_Paint);
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // buttonSaveOutput
            // 
            this.buttonSaveOutput.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonSaveOutput.Location = new System.Drawing.Point(665, 3);
            this.buttonSaveOutput.Name = "buttonSaveOutput";
            this.buttonSaveOutput.Size = new System.Drawing.Size(81, 23);
            this.buttonSaveOutput.TabIndex = 3;
            this.buttonSaveOutput.Text = "Save Output";
            this.buttonSaveOutput.UseVisualStyleBackColor = true;
            this.buttonSaveOutput.Click += new System.EventHandler(this.buttonSaveOutput_Click);
            // 
            // buttonLoadOutputSpec
            // 
            this.buttonLoadOutputSpec.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonLoadOutputSpec.Location = new System.Drawing.Point(839, 3);
            this.buttonLoadOutputSpec.Name = "buttonLoadOutputSpec";
            this.buttonLoadOutputSpec.Size = new System.Drawing.Size(123, 23);
            this.buttonLoadOutputSpec.TabIndex = 4;
            this.buttonLoadOutputSpec.Text = "Load Output Spec";
            this.buttonLoadOutputSpec.UseVisualStyleBackColor = true;
            this.buttonLoadOutputSpec.Click += new System.EventHandler(this.buttonLoadOutputSpec_Click);
            // 
            // buttonNavPrev
            // 
            this.buttonNavPrev.Location = new System.Drawing.Point(12, 3);
            this.buttonNavPrev.Name = "buttonNavPrev";
            this.buttonNavPrev.Size = new System.Drawing.Size(53, 23);
            this.buttonNavPrev.TabIndex = 5;
            this.buttonNavPrev.Text = "< Prev";
            this.buttonNavPrev.UseVisualStyleBackColor = true;
            this.buttonNavPrev.Click += new System.EventHandler(this.buttonNavPrev_Click);
            // 
            // buttonNavNext
            // 
            this.buttonNavNext.Location = new System.Drawing.Point(71, 3);
            this.buttonNavNext.Name = "buttonNavNext";
            this.buttonNavNext.Size = new System.Drawing.Size(53, 23);
            this.buttonNavNext.TabIndex = 6;
            this.buttonNavNext.Text = "Next >";
            this.buttonNavNext.UseVisualStyleBackColor = true;
            this.buttonNavNext.Click += new System.EventHandler(this.buttonNavNext_Click);
            // 
            // buttonSaveAndNext
            // 
            this.buttonSaveAndNext.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.buttonSaveAndNext.Location = new System.Drawing.Point(752, 3);
            this.buttonSaveAndNext.Name = "buttonSaveAndNext";
            this.buttonSaveAndNext.Size = new System.Drawing.Size(81, 23);
            this.buttonSaveAndNext.TabIndex = 7;
            this.buttonSaveAndNext.Text = "Save && Next";
            this.buttonSaveAndNext.UseVisualStyleBackColor = true;
            this.buttonSaveAndNext.Click += new System.EventHandler(this.buttonSaveAndNext_Click);
            // 
            // buttonNavJump
            // 
            this.buttonNavJump.Location = new System.Drawing.Point(130, 3);
            this.buttonNavJump.Name = "buttonNavJump";
            this.buttonNavJump.Size = new System.Drawing.Size(127, 23);
            this.buttonNavJump.TabIndex = 8;
            this.buttonNavJump.Text = "Texture Navigation";
            this.buttonNavJump.UseVisualStyleBackColor = true;
            this.buttonNavJump.Click += new System.EventHandler(this.buttonNavJump_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.Location = new System.Drawing.Point(1469, 3);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(103, 23);
            this.buttonHelp.TabIndex = 9;
            this.buttonHelp.Text = "Help && About";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.buttonHelp_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 761);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonNavJump);
            this.Controls.Add(this.buttonSaveAndNext);
            this.Controls.Add(this.buttonNavNext);
            this.Controls.Add(this.buttonNavPrev);
            this.Controls.Add(this.buttonLoadOutputSpec);
            this.Controls.Add(this.buttonSaveOutput);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.flowLayoutPanel);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "PH3 ImageTool";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private Button buttonSaveOutput;
        private Button buttonLoadOutputSpec;
        private Button buttonNavPrev;
        private Button buttonNavNext;
        private Button buttonSaveAndNext;
        private Button buttonNavJump;
        private Button buttonHelp;
    }
}