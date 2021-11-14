namespace ImageTool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var folderlist = File.ReadAllLines("list.txt");
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm(folderlist));
        }
    }
}