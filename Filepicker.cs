using Sharprompt;

namespace Fileprompt
{
    public class Filepicker
    {
        /// <summary>
        /// Initialize filepicker user selection
        /// </summary>
        /// <returns>File (including directory)</returns>
        public static string? Select() => Select(Directory.GetCurrentDirectory());

        /// <summary>
        /// Overload for Select - use filters and defaultSelection
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="defaultSelection"></param>
        /// <returns></returns>
        public static string? Select(string[]? filters = null, string? defaultSelection = null) => Select(Directory.GetCurrentDirectory(), filters: filters, defaultSelection: defaultSelection);

        /// <summary>
        /// Initialize filepicker user selection with filtered filetype
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static string? Select(string[] filters) => Select(Directory.GetCurrentDirectory(), filters);

        /// <summary>
        /// Use filepicker from default directory, with a selection to auto-highlight
        /// </summary>
        /// <param name="selection">Select by first in directory containing this string</param>
        /// <returns></returns>
        public static string? WithDefaultSelection(string selection) => Select(Directory.GetCurrentDirectory(), defaultSelection: selection);
        public static string? Select(string directory, string selection) => Select(directory, defaultSelection: selection);

        /// <summary>
        /// Initialize filepicker user selection with selected directory and filtered filetype
        /// </summary>
        /// <param name="startingDirectory">Select's starting directory</param>
        /// <param name="filter">File type filter (only shows files with this filetype)</param>
        /// <returns></returns>
        public static string? Select(string startingDirectory, string[]? filters = null, string? defaultSelection = null)
        {
            var directory = startingDirectory.Replace("/", "\\");
            if (directory[directory.Length - 1] == '\\')
                directory = directory.Remove(directory.Length - 1, 1);
            var chosen = false;
            string? returnFile = null;

            var formedFilters = filters == null ? null : filters.Select(f => f.Replace(".", "")).ToList();

            while (!chosen)
            {
                //get directories within this dir, as directory name - not path
                var directoryDirs = Directory.GetDirectories(directory + "\\").ToList().Select(q => $"\\{q.Split("\\")[q.Split("\\").Length - 1]}");

                //add ".." (prev dir) to top if the cd is not in top-level dir, like C:/
                var printList = directory.Split("\\").Length > 1 ? new List<string>() { ".." } : new List<string>();
                printList.Add(">>> Refresh Directory");
                printList.AddRange(directoryDirs);

                var directoryFiles = Directory.GetFiles(directory + "\\").ToList().Select(q => q.Split("\\")[q.Split("\\").Length - 1]);

                if (formedFilters != null && formedFilters.Count() > 0)
                    directoryFiles = directoryFiles.Where(f => formedFilters.Contains(f.Split(".")[f.Split(".").Length - 1]));

                printList.AddRange(directoryFiles);
                printList.Add(">>> Cancel Fileprompt");
                if (defaultSelection != null)
                {
                    if (!printList.Any(l => l.Contains(defaultSelection)))
                        defaultSelection = null;
                    else
                        defaultSelection = printList.First(l => l.Contains(defaultSelection));
                }
                var chosenFilename = Prompt.Select($">>> {directory}\\ ", printList, 25, defaultSelection ?? "..");
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