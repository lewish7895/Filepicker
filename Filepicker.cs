using Sharprompt;

namespace Fileprompt
{
    public class Filepicker
    {
        /// <summary>
        /// Initialize filepicker user selection
        /// </summary>
        /// <returns>File (including directory)</returns>
        public static string? Select()
        {
            return Select(Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Initialize filepicker user selection with filtered filetype
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static string? Select(string[] filters)
        {
            return Select(Directory.GetCurrentDirectory(), filters);
        }

        /// <summary>
        /// Initialize filepicker user selection with selected directory and filtered filetype
        /// </summary>
        /// <param name="startingDirectory">Select's starting directory</param>
        /// <param name="filter">File type filter (only shows files with this filetype)</param>
        /// <returns></returns>
        public static string? Select(string startingDirectory, string[]? filters = null)
        {
            var directory = startingDirectory;
            var chosen = false;
            string? returnFile = null;

            var formedFilters = filters == null ? null : filters.Select(f => f.Replace(".", "")).ToList();

            while (!chosen)
            {
                var directoryDirs = Directory.GetDirectories(directory + "\\").ToList().Select(q => $"\\{q.Split("\\")[q.Split("\\").Length - 1]}");

                var printList = directory.Split("\\").Length > 1 ? new List<string>() { ".." } : new List<string>();
                printList.Add(">>> Refresh Directory");
                printList.AddRange(directoryDirs);

                var directoryFiles = Directory.GetFiles(directory).ToList().Select(q => q.Split("\\")[q.Split("\\").Length - 1]);

                if (formedFilters != null && formedFilters.Count() > 0)
                    directoryFiles = directoryFiles.Where(f => formedFilters.Contains(f.Split(".")[f.Split(".").Length - 1]));

                printList.AddRange(directoryFiles);
                printList.Add(">>> Cancel Fileprompt");
                var chosenFilename = Prompt.Select($">>> {directory}\\ ", printList, 25, "..");
                if (chosenFilename == "..")
                    directory = String.Join("\\", directory.Split("\\").SkipLast(1));
                else if (chosenFilename == ">>> Cancel Fileprompt")
                    return null;
                else if (chosenFilename.Contains("\\"))
                    directory = $"{directory}{chosenFilename}";
                else if (chosenFilename != ">>> Refresh Directory")
                {
                    returnFile = $"{directory}\\{chosenFilename}";
                    chosen = true;
                }
            }
            return returnFile;
        }
    }
}