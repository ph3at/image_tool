// Form for the user to set options related to post processing.
//
// Copyright(C) 2022 Peter Thoman / PH3 GmbH
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ImageTool
{
    public partial class PostProcForm : Form
    {
        MainForm mainForm;
        PostProcOptions options;

        internal PostProcForm(MainForm mainForm, PostProcOptions itpOptions)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            options = itpOptions;

            textBoxExe.Text = options.ExePath;
            textBoxOptions.Text = options.Parameters;
            textBoxOutput.Text = options.OutputPath;
            checkBoxAutoStore.Checked = options.AutoSave;
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            options.ApplyAndSave(textBoxExe.Text, textBoxOptions.Text, textBoxOutput.Text, checkBoxAutoStore.Checked);
        }

        private void RunPicView(string infn, string outfn)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = options.ExePath;
            start.Arguments = options.BuildArgs(Path.GetFullPath(infn), Path.GetFullPath(outfn));
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;

            // Run the external process & wait for it to finish
            using (Process? proc = Process.Start(start))
            {
                if (proc != null) proc.WaitForExit();
                else MessageBox.Show(String.Format("Could not launch {} with {}.", start.FileName, start.Arguments), "ITP Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string BuildFullFn(string path, string id, string extension = "itp")
        {
            return String.Format("{0}/{1}_{2}.{3}", path, id, mainForm.Names[id], extension);
        }

        public void SaveCurImgToITP()
        {
            Directory.CreateDirectory(options.OutputPath);

            string id = mainForm.CurFolder;
            string pngfn = ImageController.GetOutputImgFn(id);
            string itpfn = BuildFullFn(options.OutputPath, id);

            RunPicView(pngfn, itpfn);
        }

        private void buttonCurImage_Click(object sender, EventArgs e)
        {
            SaveCurImgToITP();
        }

        public static void EmptyDir(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach (FileInfo file in directory.GetFiles()) file.Delete();
        }

        private void buttonStoreAll_Click(object sender, EventArgs e)
        {
            var cursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            string tmpPath = Path.GetTempPath() + "/PH3ImageTool/";
            Directory.CreateDirectory(tmpPath);
            EmptyDir(tmpPath);

            // copy all outputs to tmp folder
            foreach (string id in mainForm.FolderList)
            {
                try
                {
                    string src = ImageController.GetOutputImgFn(id);
                    string dst = BuildFullFn(tmpPath, id, "png");
                    File.Copy(src, dst, true);
                } catch { }
            }
            RunPicView(tmpPath, options.OutputPath);

            Cursor.Current = cursor;
        }

        private void ITPForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event, preventing dispose
        }
    }
}
