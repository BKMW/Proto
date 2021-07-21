using System;
using System.IO;
using System.Text;

namespace Logger
{
    public class Log : ILog
    {
        public void LogException(string message)
        {
            string fileName = string.Format("{0}_{1}.log", "Exception", DateTime.Now.ToString("YYYY-MM-dd"));
          //  string fileName1 = $"log_{DateTime.Now.ToString("YYYY-MM-dd")}.log";

            string logFilePath = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, fileName);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("----------------------------------------");
            sb.AppendLine(DateTime.Now.ToString());
            sb.AppendLine(message);
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.Write(sb.ToString());
                writer.Flush();
            }
        }
    }
}
