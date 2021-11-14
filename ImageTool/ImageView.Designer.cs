namespace ImageTool
{
    partial class ImageView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelImg = new System.Windows.Forms.Panel();
            this.labelName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panelImg
            // 
            this.panelImg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelImg.Location = new System.Drawing.Point(0, 0);
            this.panelImg.Name = "panelImg";
            this.panelImg.Size = new System.Drawing.Size(512, 512);
            this.panelImg.TabIndex = 0;
            this.panelImg.Paint += new System.Windows.Forms.PaintEventHandler(this.panelImg_Paint);
            this.panelImg.DoubleClick += new System.EventHandler(this.panelImg_DoubleClick);
            this.panelImg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelImg_MouseDown);
            this.panelImg.MouseLeave += new System.EventHandler(this.panelImg_MouseLeave);
            this.panelImg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelImg_MouseMove);
            this.panelImg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelImg_MouseUp);
            // 
            // labelName
            // 
            this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelName.Location = new System.Drawing.Point(0, 515);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(512, 21);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Name";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ImageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.panelImg);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "ImageView";
            this.Size = new System.Drawing.Size(512, 600);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panelImg;
        private Label labelName;
    }
}
