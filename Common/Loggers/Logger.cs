namespace Gamefreak130.Common.Loggers
{
    using Sims3.Gameplay;
    using Sims3.Gameplay.Utilities;
    using Sims3.SimIFace;
    using Sims3.UI;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using Environment = System.Environment;

    public abstract class Logger<T>
    {
        static Logger()
        {
            Assembly assembly = typeof(Logger<T>).Assembly;
            sName = assembly.GetName().Name;
            sModVersion = (Attribute.GetCustomAttribute(assembly, typeof(AssemblyFileVersionAttribute)) as AssemblyFileVersionAttribute).Version;
            sGameVersionData = GameUtils.GetGenericString(GenericStringID.VersionData).Split('\n');
            sAssemblyNames = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies())
                                               .ConvertAll(assembly => assembly.GetName().Name);
            sAssemblyNames.Sort();
        }

        protected static readonly string sName;

        private static readonly string sModVersion;

        private static readonly string[] sGameVersionData;

        private static readonly List<string> sAssemblyNames;

        public abstract void Log(T input);

        protected virtual void WriteLog(StringBuilder content)
        {
            uint fileHandle = 0;
            try
            {
                Simulator.CreateExportFile(ref fileHandle, $"ScriptError_{sName}_{DateTime.Now:M-d-yyyy_hh-mm-ss}__");
                if (fileHandle != 0)
                {
                    CustomXmlWriter xmlWriter = new(fileHandle);
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteToBuffer(GenerateXmlWrapper(content));
                    xmlWriter.FlushBufferToFile();
                }
                Notify();
            }
            finally
            {
                if (fileHandle != 0)
                {
                    Simulator.CloseScriptErrorFile(fileHandle);
                }
            }
        }

        private string GenerateXmlWrapper(StringBuilder content)
        {
            StringBuilder xmlBuilder = new();
            xmlBuilder.AppendLine($"<{sName}>");
            xmlBuilder.AppendLine($"<ModVersion value=\"{sModVersion}\"/>");
            xmlBuilder.AppendLine($"<GameVersion value=\"{sGameVersionData[0]} ({sGameVersionData[5]}) ({sGameVersionData[7]})\"/>");
            xmlBuilder.AppendLine($"<InstalledPacks value=\"{GameUtils.sProductFlags}\"/>");
            // The logger expects the content to have a new line at the end of it
            // More new lines are appended here to create exactly one line of padding before and after the XML tags
            xmlBuilder.AppendLine("<Content>" + Environment.NewLine);
            xmlBuilder.Append(content.Replace("&", "&amp;"));
            xmlBuilder.AppendLine(Environment.NewLine + "</Content>");
            xmlBuilder.AppendLine("<LoadedAssemblies>");
            xmlBuilder.Append(GenerateAssemblyList());
            xmlBuilder.AppendLine("</LoadedAssemblies>");
            xmlBuilder.Append($"</{sName}>");
            return xmlBuilder.ToString();
        }

        private StringBuilder GenerateAssemblyList()
        {
            StringBuilder result = new();
            foreach (string name in sAssemblyNames)
            {
                result.AppendLine(" " + name);
            }
            return result;
        }

        private void Notify() => Notify(WriteNotification());

        private void Notify(string notification)
        {
            if (!string.IsNullOrEmpty(notification))
            {
                if (GameStates.IsInWorld())
                {
                    StyledNotification.Show(new(notification, StyledNotification.NotificationStyle.kSystemMessage));
                }
                else
                {
                    SimpleMessageDialog.Show(sName, notification);
                }
            }
        }

        protected virtual string WriteNotification()
            => string.Empty;
    }
}
