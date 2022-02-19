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
            this.buttonChangeBG = new System.Windows.Forms.Button();
            this.buttonSaveOutput = new System.Windows.Forms.Button();
            this.buttonLoadOutputSpec = new System.Windows.Forms.Button();
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
            // buttonChangeBG
            // 
            this.buttonChangeBG.Location = new System.Drawing.Point(12, 3);
            this.buttonChangeBG.Name = "buttonChangeBG";
            this.buttonChangeBG.Size = new System.Drawing.Size(75, 23);
            this.buttonChangeBG.TabIndex = 2;
            this.buttonChangeBG.Text = "Change BG";
            this.buttonChangeBG.UseVisualStyleBackColor = true;
            this.buttonChangeBG.Click += new System.EventHandler(this.buttonChangeBG_Click);
            // 
            // buttonSaveOutput
            // 
            this.buttonSaveOutput.Location = new System.Drawing.Point(93, 3);
            this.buttonSaveOutput.Name = "buttonSaveOutput";
            this.buttonSaveOutput.Size = new System.Drawing.Size(106, 23);
            this.buttonSaveOutput.TabIndex = 3;
            this.buttonSaveOutput.Text = "Save Output";
            this.buttonSaveOutput.UseVisualStyleBackColor = true;
            this.buttonSaveOutput.Click += new System.EventHandler(this.buttonSaveOutput_Click);
            // 
            // buttonLoadOutputSpec
            // 
            this.buttonLoadOutputSpec.Location = new System.Drawing.Point(205, 3);
            this.buttonLoadOutputSpec.Name = "buttonLoadOutputSpec";
            this.buttonLoadOutputSpec.Size = new System.Drawing.Size(123, 23);
            this.buttonLoadOutputSpec.TabIndex = 4;
            this.buttonLoadOutputSpec.Text = "Load Output Spec";
            this.buttonLoadOutputSpec.UseVisualStyleBackColor = true;
            this.buttonLoadOutputSpec.Click += new System.EventHandler(this.buttonLoadOutputSpec_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 761);
            this.Controls.Add(this.buttonLoadOutputSpec);
            this.Controls.Add(this.buttonSaveOutput);
            this.Controls.Add(this.buttonChangeBG);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.flowLayoutPanel);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "PH3 ImageTool";
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
        private Button buttonChangeBG;
        private Button buttonSaveOutput;
        private Button buttonLoadOutputSpec;
    }
}