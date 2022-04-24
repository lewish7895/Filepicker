using Sharprompt;

namespace Fileprompt
{
    public class Filepicker
    {
        /// <summary>
        /// Initialize filepicker user selection
        /// </summary>
        /// <returns>File (including directory)</returns>
        public static string Select()
        {
            return Select(Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Initialize filepicker user selection with filtered filetype
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static string Select(IEnumerable<string> filters)
        {
            return Select(Directory.GetCurrentDirectory(), filters);
        }

        /// <summary>
        /// Initialize filepicker user selection with selected directory and filtered filetype
        /// </summary>
        /// <param name="startingDirectory">Select's starting directory</param>
        /// <param name="filter">File type filter (only allows selection with selected)</param>
        /// <returns></returns>
        public static string Select(string startingDirectory, IEnumerable<String>? filters = null)
        {
            var directory = startingDirectory;
            var chosen = false;
            var returnFile = "";
            while (!chosen)
            {
                var printList = new List<string>() { ".." };
                var directoryDirs = Directory.GetDirectories(directory).ToList().Select(q => $"\\{q.Split("\\")[q.Split("\\").Length - 1]}");
                printList.AddRange(directoryDirs);
                var directoryFiles = Directory.GetFiles(directory).ToList().Select(q => q.Split("\\")[q.Split("\\").Length - 1]);
                printList.AddRange(directoryFiles);
                var chosenFilename = Prompt.Select($">>> {directory}", printList, defaultValue: "..");
                if (chosenFilename == "..")
                    directory = String.Join("\\", directory.Split("\\").SkipLast(1));
                else if (chosenFilename.Contains("\\"))
                    directory = $"{directory}{chosenFilename}";
                else
                {
                    if (filters != null && !filters.Contains(chosenFilename.Split(".")[chosenFilename.Split(".").Length]))
                        Console.WriteLine($"Chosen file MUST be of type {String.Join(",", $"\"{filters}\"")}. Please select another file.");
                    else
                    {
                        returnFile = $"{directory}\\{chosenFilename}";
                        chosen = true;
                    }
                }
            }
            return returnFile;
        }
    }
}