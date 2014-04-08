namespace NRegFreeCom
{
    public class PathAppender
    {
        /// <summary>
        /// Adds <paramref name="directory"/> to PATH  variable. Thread unsafe. Does not normalizes   <paramref name="directory"/> or PATH before search, can add already existing path
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string Append(string paths, string directory)
        {
            var newPaths = paths;
            
            if (!newPaths.Contains(directory)) 
            {
                const string delimeter = ";";
                newPaths = newPaths.TrimEnd(' ', '\t');
                if (!newPaths.EndsWith(delimeter))
                {
                    newPaths = newPaths + delimeter;
                }
                newPaths = newPaths + directory + delimeter;
            }
            return newPaths;
        }
    }
}