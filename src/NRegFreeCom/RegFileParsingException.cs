using System;

namespace NRegFreeCom
{
    [Serializable]
    public class RegFileParsingException : Exception
    {
        public RegFileParsingException(string message) : base(message){}
        public RegFileParsingException(string message, Exception exception):base(message,exception){}
    }
}