using Sharprompt;

namespace Filepicker
{
    public class Filepicker
    {
        /// <summary>
        /// Init user select cli
        /// </summary>
        /// <returns>File (including directory)</returns>
        public static string Select()
        {
            return Select(Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Init user select cli with directory
        /// </summary>
        /// <param name="startingDirectory">Select's starting directory</param>
        /// <param name="filter">File type filter (only allows selection with selected)</param>
        /// <returns></returns>
        public static string Select(string startingDirectory, string? filter = null)
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
                    if (filter != null && !chosenFilename.Contains(filter))
                        Console.WriteLine($"Chosen file MUST be of type \"{filter}\". Please select another file.");
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