namespace Gamefreak130.Common.Loggers
{
    using Sims3.Gameplay.Utilities;
    using System;
    using System.Text;

    /// <summary>Logger for one-time events.</summary>
    /// <remarks>Any received input to log is immediately converted to a string and written to a new log file,
    /// along with timestamps and the rest of the standard log info.</remarks>
    public abstract class EventLogger<T> : Logger<T>
    {
        public override void Log(T input) => WriteLog(new(input.ToString()));

        protected override void WriteLog(StringBuilder content, string fileName)
        {
            StringBuilder log = new();
            log.AppendLine("Logged At:");
            log.AppendLine($" Sim Time: {SimClock.CurrentTime()}");
            log.AppendLine(" Real Time: " + DateTime.Now + Environment.NewLine);
            log.Append(content);
            base.WriteLog(content, fileName);
        }
    }
}
