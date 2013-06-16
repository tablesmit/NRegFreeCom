using System;
using System.Diagnostics;

namespace NRegFreeCom
{
    internal static class Tracing
    {
        private static readonly string Name = typeof(Tracing).Namespace;
        private static TraceSource _source;

        public static TraceSource Source
        {
            get { return _source; }
            set { _source = value; }
        }

        static Tracing()
        {
            _source = new TraceSource(Name);
        }




        public static void Verbose(string message)
        {
            _source.TraceEvent(TraceEventType.Verbose, 0, message);

        }

        public static void Verbose(string message, params object[] arguments)
        {

            Verbose(String.Format(message, arguments));

        }

        public static void Warning(string message)
        {
            _source.TraceEvent(TraceEventType.Warning, 0, message);
        }

        public static void Warning(string message, params object[] arguments)
        {
            Warning(String.Format(message, arguments));
        }

        public static void Error(string message)
        {
            _source.TraceEvent(TraceEventType.Error, 0, message);
        }

        public static void Error(Exception error)
        {
            Error(error.ToString());
        }

        public static void Error(string message, params object[] arguments)
        {
            Error(String.Format(message, arguments));
        }
    }
}