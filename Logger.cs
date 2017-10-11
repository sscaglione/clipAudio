using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Logger
{
    class Logger
    {
        private StreamWriter file;
        private string delimiter = "\t";
        private string escapedDelimiter = "\\t";

        public Logger(string filename, params string[] headerEntries)
        {
            //this.subjectID = subjectID;
            file = new StreamWriter(filename);
            if (headerEntries.Length > 0)
            {
                string line = "";
                //string line = "subjectID" + this.delimiter + "timeGMT" + this.delimiter + "timestamp";
                for (int i = 0; i < headerEntries.Length; i++)
                    if (i == 0)
                        line += headerEntries[i];
                    else
                        line += this.delimiter + headerEntries[i];
                this.write(line);
            }
        }

        private void write(string line)
        {
            file.WriteLine(line);
            file.Flush();
        }

        public void log(params string[] entries)
        {
            string line = "";
            /*string line = this.subjectID + this.delimiter + TimeUtil.TimeUtil.getCurrentTimeString() +
                this.delimiter + DateTime.Now.Ticks;*/
            for (int i = 0; i < entries.Length; i++)
                if (i == 0)
                    line = entries[i];
                else
                    line += this.delimiter + entries[i];
            this.write(line);
        }

        public string escape(string entry)
        {
            return entry.Replace("\n", "\\n").Replace(this.delimiter, this.escapedDelimiter);
        }

        public void close()
        {
            file.Close();
        }
    }
}
