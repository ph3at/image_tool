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
    public partial class ITPForm : Form
    {
        MainForm mainForm;
        ITPOptions options;

        internal ITPForm(MainForm mainForm, ITPOptions itpOptions)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            options = itpOptions;

            textBoxExe.Text = options.ExePath;
            textBoxOptions.Text = options.Parameters;
            textBoxOutput.Text = options.OutputPath;
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            options.ApplyAndSave(textBoxExe.Text, textBoxOptions.Text, textBoxOutput.Text);
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

        private void buttonCurImage_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(options.OutputPath);

            string id = mainForm.CurFolder;
            string pngfn = ImageController.GetOutputImgFn(id);
            string itpfn = BuildFullFn(options.OutputPath, id);

            RunPicView(pngfn, itpfn);
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
    }
}
