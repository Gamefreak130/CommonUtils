namespace Gamefreak130.Common.Loggers
{
    using Sims3.UI;
    using System;

    public class ExceptionLogger : EventLogger<Exception>
    {
        private ExceptionLogger()
        {
        }

        internal static readonly ExceptionLogger sInstance = new();

        protected override void Notify() => StyledNotification.Show(new($"Error occurred in {sName}\n\nAn error log has been created in your user directory. Please send it to Gamefreak130 for further review.", StyledNotification.NotificationStyle.kSystemMessage));
    }
}
