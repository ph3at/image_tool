using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTool
{
    internal class ITPOptions
    {
        private string exePath = "..\\picview.exe";
        private string parameters = "/filetype itp /SubCategory bc7 /MipMap 99 /SetTexSize 0 /AutoQuit 1";
        private const string fileOptions = "/srcpath \"{0}\" /destpath \"{1}\" ";

        private string outputPath = "..\\itp_output";

        public string ExePath { get { return exePath; } }
        public string Parameters { get { return parameters; } }
        public string OutputPath { get { return outputPath; } }


        static private string GetSavePath()
        {
            string path = String.Format("{0}/PH3ImageTool", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            Directory.CreateDirectory(path);
            return String.Format("{0}/itp_settings.txt", path);
        }

        private void Save()
        {
            string outTxt = exePath + "\n" + parameters + "\n" + outputPath;
            File.WriteAllText(GetSavePath(), outTxt);
        }
        public void Load()
        {
            try
            {
                var lines = File.ReadAllLines(GetSavePath());
                exePath = lines[0];
                parameters = lines[1];
                outputPath = lines[2];
            } catch { } // if loading options fails we just stick with defaults
        }

        public void ApplyAndSave(string exePath, string parameters, string outputPath)
        {
            this.exePath = exePath;
            this.parameters = parameters;
            this.outputPath = outputPath;
            Save();
        }

        internal string BuildArgs(string pngfn, string itpfn)
        {
            string fileParms = String.Format(fileOptions, pngfn, itpfn);
            return fileParms + parameters;
        }
    }
}
