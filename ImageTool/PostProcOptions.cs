// Option handling (including load/store) for post processing.
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTool
{
    internal class PostProcOptions
    {
        private string exePath = "..\\picview.exe";
        private string parameters = "/srcpath \"{0}\" /destpath \"{1}\" /filetype itp /SubCategory bc7 /MipMap 99 /SetTexSize 0 /AutoQuit 1";
        private string outputPath = "..\\itp_output";
        private bool autoSave = false;

        public string ExePath { get { return exePath; } }
        public string Parameters { get { return parameters; } }
        public string OutputPath { get { return outputPath; } }
        public bool AutoSave { get { return autoSave; } }


        static private string GetSavePath()
        {
            string path = String.Format("{0}/PH3ImageTool", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            Directory.CreateDirectory(path);
            return String.Format("{0}/itp_settings.txt", path);
        }

        private void Save()
        {
            string outTxt = exePath + "\n" + parameters + "\n" + outputPath + "\n" + autoSave.ToString();
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
                autoSave = bool.Parse(lines[3]);
            } catch { } // if loading options fails we just stick with defaults
        }

        public void ApplyAndSave(string exePath, string parameters, string outputPath, bool autoSave)
        {
            this.exePath = exePath;
            this.parameters = parameters;
            this.outputPath = outputPath;
            this.autoSave = autoSave;
            Save();
        }

        internal string BuildArgs(string pngfn, string itpfn)
        {
            return String.Format(parameters, pngfn, itpfn);
        }
    }
}
