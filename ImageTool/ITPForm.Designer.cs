namespace ImageTool
{
    partial class ITPForm
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
            this.labelExe = new System.Windows.Forms.Label();
            this.textBoxExe = new System.Windows.Forms.TextBox();
            this.labelOptions = new System.Windows.Forms.Label();
            this.textBoxOptions = new System.Windows.Forms.TextBox();
            this.labelOutput = new System.Windows.Forms.Label();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.buttonCurImage = new System.Windows.Forms.Button();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.buttonStoreAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelExe
            // 
            this.labelExe.Location = new System.Drawing.Point(12, 9);
            this.labelExe.Name = "labelExe";
            this.labelExe.Size = new System.Drawing.Size(157, 23);
            this.labelExe.TabIndex = 3;
            this.labelExe.Text = "PicView Executable Path:";
            this.labelExe.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxExe
            // 
            this.textBoxExe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxExe.Location = new System.Drawing.Point(175, 9);
            this.textBoxExe.Name = "textBoxExe";
            this.textBoxExe.Size = new System.Drawing.Size(458, 23);
            this.textBoxExe.TabIndex = 2;
            // 
            // labelOptions
            // 
            this.labelOptions.Location = new System.Drawing.Point(12, 38);
            this.labelOptions.Name = "labelOptions";
            this.labelOptions.Size = new System.Drawing.Size(157, 23);
            this.labelOptions.TabIndex = 5;
            this.labelOptions.Text = "PicView Options:";
            this.labelOptions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxOptions
            // 
            this.textBoxOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOptions.Location = new System.Drawing.Point(175, 38);
            this.textBoxOptions.Name = "textBoxOptions";
            this.textBoxOptions.Size = new System.Drawing.Size(458, 23);
            this.textBoxOptions.TabIndex = 4;
            // 
            // labelOutput
            // 
            this.labelOutput.Location = new System.Drawing.Point(12, 67);
            this.labelOutput.Name = "labelOutput";
            this.labelOutput.Size = new System.Drawing.Size(157, 23);
            this.labelOutput.TabIndex = 7;
            this.labelOutput.Text = "Output Path:";
            this.labelOutput.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.Location = new System.Drawing.Point(175, 67);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(458, 23);
            this.textBoxOutput.TabIndex = 6;
            // 
            // buttonCurImage
            // 
            this.buttonCurImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCurImage.Location = new System.Drawing.Point(463, 163);
            this.buttonCurImage.Name = "buttonCurImage";
            this.buttonCurImage.Size = new System.Drawing.Size(170, 23);
            this.buttonCurImage.TabIndex = 0;
            this.buttonCurImage.Text = "Store Current Output to ITP";
            this.buttonCurImage.UseVisualStyleBackColor = true;
            this.buttonCurImage.Click += new System.EventHandler(this.buttonCurImage_Click);
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveSettings.Location = new System.Drawing.Point(463, 96);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(170, 23);
            this.buttonSaveSettings.TabIndex = 10;
            this.buttonSaveSettings.Text = "Save && Apply Settings";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // buttonStoreAll
            // 
            this.buttonStoreAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStoreAll.Location = new System.Drawing.Point(12, 161);
            this.buttonStoreAll.Name = "buttonStoreAll";
            this.buttonStoreAll.Size = new System.Drawing.Size(280, 23);
            this.buttonStoreAll.TabIndex = 1;
            this.buttonStoreAll.Text = "Store all available outputs to ITP (Slow!)";
            this.buttonStoreAll.UseVisualStyleBackColor = true;
            this.buttonStoreAll.Click += new System.EventHandler(this.buttonStoreAll_Click);
            // 
            // ITPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 196);
            this.Controls.Add(this.buttonStoreAll);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.buttonCurImage);
            this.Controls.Add(this.labelOutput);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.labelOptions);
            this.Controls.Add(this.textBoxOptions);
            this.Controls.Add(this.labelExe);
            this.Controls.Add(this.textBoxExe);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ITPForm";
            this.Text = "ITP Handling";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label labelExe;
        private TextBox textBoxExe;
        private Label labelOptions;
        private TextBox textBoxOptions;
        private Label labelOutput;
        private TextBox textBoxOutput;
        private Button buttonCurImage;
        private Button buttonSaveSettings;
        private Button buttonStoreAll;
    }
}