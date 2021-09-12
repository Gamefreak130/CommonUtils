namespace Gamefreak130.Common.Loggers
{
    using System;

    public class ExceptionLogger : EventLogger<Exception>
    {
        private ExceptionLogger()
        {
        }

        internal static readonly ExceptionLogger sInstance = new();

        protected override string WriteNotification() => $"Error occurred in {sName}\n\nAn error log has been created in your user directory. Please send it to Gamefreak130 for further review.";
    }
}
