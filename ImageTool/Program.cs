namespace ImageTool
{
    internal static class Program
    {
        public readonly static string VERSION = "0.5";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if(args.Length > 0) Directory.SetCurrentDirectory(args[0]);
            var folderlist = Directory.GetDirectories(".").Select(x => x.Replace(".\\", "")).ToArray();
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm(folderlist));
        }
    }
}