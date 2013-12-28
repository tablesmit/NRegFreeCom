namespace NRegFreeCom
{
    internal class PathAppender
    {
        public static string Append(string paths, string directory)
        {
            var newPaths = paths;
            //we do not normalize PATH before search, can add already existing path
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