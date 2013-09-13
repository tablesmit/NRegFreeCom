namespace NRegFreeCom
{
    internal class PathAppender
    {
        public static string Append(string paths, string directory)
        {
            var newPaths = paths;
            if (!newPaths.Contains(directory)) //TODO: normalize PATH before search, what is impact of duplication?
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