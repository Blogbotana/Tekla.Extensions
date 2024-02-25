using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tekla.Structures;
using Tekla.Structures.Model;

namespace Tekla.Extension
{
    /// <summary>
    /// Helps to work with files in Tekla model
    /// </summary>
    public static class FileExtension
    {
        public const string AttributeFolder = "attributes";
        public static IReadOnlyCollection<string> GetFiles(string searchPattern)
        {
            HashSet<string> files = new();

            string projectPaths = string.Empty;
            string systemPaths = string.Empty;
            string firmPaths = string.Empty;
            _ = TeklaStructuresSettings.GetAdvancedOption("XS_SYSTEM", ref systemPaths);
            _ = TeklaStructuresSettings.GetAdvancedOption("XS_FIRM", ref firmPaths);
            _ = TeklaStructuresSettings.GetAdvancedOption("XS_PROJECT", ref projectPaths);

            GetFiles(files, searchPattern, projectPaths);
            GetFiles(files, searchPattern, systemPaths);
            GetFiles(files, searchPattern, firmPaths);

            string modelPath = Path.Combine(new Model().GetInfo().ModelPath, AttributeFolder);
            GetFiles(files, searchPattern, modelPath);
            var sortedSet = files.ToList();
            sortedSet.Sort();
            return sortedSet;
        }

        private static void GetFiles(HashSet<string> hashSet, string searchPattern, string currentPaths)
        {
            if (currentPaths.Length == 0)
                return;

            string[] paths = currentPaths.Split(';');
            foreach (string path in paths)
            {
                if (!Directory.Exists(path))
                    continue;

                string[] filesPath = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
                foreach (string file in filesPath)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    hashSet.Add(fileName);
                }
            }
        }
    }
}
