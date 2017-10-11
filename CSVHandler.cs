using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSVHandler
{
    class CSVHandler
    {
        public char delimiter = '\t';
        public char escape = '\0'; // By default don't use escape characters.

        private List<List<string>> data = null;
        private List<string> columnNames = null;
        private int curRow = 0;

        public CSVHandler(string filename, bool firstRowIsColumnNames = true)
        {
            this.load(filename, firstRowIsColumnNames);
        }

        public CSVHandler()
        {
        }

        public void load(string filename, bool firstRowIsColumnNames = true)
        {
            this.data = new List<List<string>>();
            this.columnNames = null;
            this.curRow = 0;

            using (System.IO.StreamReader file = new System.IO.StreamReader(filename))
            {
                if (firstRowIsColumnNames)
                {
                    this.columnNames = this.parseRow(file.ReadLine());
                    for (int i = 1; i < this.columnNames.Count; i++)
                        while (this.columnNames.IndexOf(this.columnNames[i], 0, i) >= 0)
                            this.columnNames[i] += "_";
                }

                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    if (line.Length > 0)
                        this.data.Add(this.parseRow(line));
                }
            }
        }

        public List<string> parseRow(string line)
        {
            if (line.Length > 0)
                line += this.delimiter; // Makes parsing easier.
            List<string> result = new List<string>();
            int leftBound = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == this.escape)
                {
                    line = line.Remove(i, 1);
                    continue;
                }
                if (line[i] == this.delimiter)
                {
                    result.Add(line.Substring(leftBound, i - leftBound));
                    leftBound = i + 1;
                }
            }
            return result;
        }

        public Dictionary<string, string> getNextRow()
        {
            return this.getNthRow(this.curRow++);
        }

        public Dictionary<string, string> getNthRow(int n)
        {
            if (n >= this.data.Count)
                return null; // End of data.

            Dictionary<string, string> row = new Dictionary<string, string>();
            for (int i = 0; i < this.data[n].Count; i++)
            {
                if (this.columnNames != null)
                    row.Add(this.columnNames[i], this.data[n][i]);
                else
                    row.Add("" + i, this.data[n][i]);
            }
            return row;
        }

        public bool endOfData()
        {
            return this.curRow >= this.data.Count;
        }

        public void setCurrentRow(Dictionary<string, string> rowData)
        {
            this.setNthRow(this.curRow - 1, rowData);
        }

        public void setNthRow(int n, Dictionary<string, string> rowData)
        {
            for (int i = 0; i < this.data[n].Count; i++)
            {
                if (this.columnNames != null)
                    this.data[n][i] = rowData[this.columnNames[i]];
                else
                    this.data[n][i] = rowData["" + i];
            }
        }

        // Save contents of current CSV to a file.
        public void save(string filename)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filename))
            {
                if (this.columnNames != null)
                {
                    for (int i = 0; i < this.columnNames.Count; i++)
                    {
                        if (i > 0)
                            sw.Write(this.delimiter);
                        sw.Write(this.columnNames[i]);
                    }
                    sw.WriteLine();
                }
                for (int i = 0; i < this.data.Count; i++)
                {
                    for (int j = 0; j < this.data[i].Count; j++)
                    {
                        if (j > 0)
                            sw.Write(this.delimiter);
                        sw.Write(this.data[i][j]);
                    }
                    sw.WriteLine();
                }
                sw.Flush();
            }
        }
    }
}
