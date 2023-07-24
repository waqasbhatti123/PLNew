using System;
using System.Text;
using System.IO;

namespace RMS
{
    public class CreateLog
    {
        public void AddToLogFile(StringBuilder Message, string path, string source, DateTime date)
        {
            if (File.Exists(path))
            {
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine("---------------------------------------");
                    writer.WriteLine("-------------------START---------------");
                    writer.WriteLine("---------------------------------------");
                    writer.WriteLine("DateTime : " + date.ToString());
                    writer.WriteLine("Source : " + source);
                    writer.WriteLine(Message);
                    writer.WriteLine("---------------------------------------");
                    writer.WriteLine("-------------------END-----------------");
                    writer.WriteLine("---------------------------------------");
                    writer.WriteLine("");
                    writer.Close();
                }
            }
            else
            {
                StreamWriter writer = File.CreateText(path);
                writer.WriteLine("---------------------------------------");
                writer.WriteLine("-------------------START---------------");
                writer.WriteLine("---------------------------------------");
                writer.WriteLine("DateTime : " + date.ToString());
                writer.WriteLine("Source : " + source);
                writer.WriteLine(Message);
                writer.WriteLine("---------------------------------------");
                writer.WriteLine("-------------------END-----------------");
                writer.WriteLine("---------------------------------------");
                writer.WriteLine("");
                writer.Close();
            }
        }
    }
}