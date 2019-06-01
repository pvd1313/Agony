using System.Collections.Generic;
using System.Reflection;
using System;

namespace Agony.Common
{
    public static class Logger
    {
        private static readonly HashSet<string> stackTraceCash = new HashSet<string>();

        public static void ExceptionOnce(Exception exception)
        {
            var stackTrace = exception.StackTrace;
            if (stackTraceCash.Contains(stackTrace)) return;

            stackTraceCash.Add(stackTrace);
            var caller = Assembly.GetCallingAssembly().GetName().Name;
            var message = CreateExceptionString(exception);
            Console.WriteLine($"[{caller}] Error: {message}");
        }

        public static void ExceptionOnce(Exception exception, string title)
        {
            var stackTrace = exception.StackTrace;
            if (stackTraceCash.Contains(stackTrace)) return;

            stackTraceCash.Add(stackTrace);
            var caller = Assembly.GetCallingAssembly().GetName().Name;
            var message = CreateExceptionString(exception);
            Console.WriteLine($"[{caller}] Error: {title} {message}");
        }

        public static void Exception(Exception exception)
        {
            var caller = Assembly.GetCallingAssembly().GetName().Name;
            var message = CreateExceptionString(exception);
            Console.WriteLine($"[{caller}] Error: {message}");
        }

        public static void Exception(Exception exception, string title)
        {
            var caller = Assembly.GetCallingAssembly().GetName().Name;
            var message = CreateExceptionString(exception);
            Console.WriteLine($"[{caller}] Error: {title} {message}");
        }

        private static string CreateExceptionString(Exception exception) => string.Format("{0}: '{1}'\n{2}", exception.GetType(), exception.Message, exception.StackTrace);

        public static void Error(string message)
        {
            var caller = Assembly.GetCallingAssembly().GetName().Name;
            Console.WriteLine($"[{caller}] Error: {message}");
        }

        public static void Warning(string message)
        {
            var caller = Assembly.GetCallingAssembly().GetName().Name;
            Console.WriteLine($"[{caller}] Warning: {message}");
        }

        public static void Message(string message)
        {
            var caller = Assembly.GetCallingAssembly().GetName().Name;
            Console.WriteLine($"[{caller}] {message}");
        }
    }
}