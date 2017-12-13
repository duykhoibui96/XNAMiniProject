using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MiniProject2D.Information
{
    class RecordLoader
    {
        public static RecordLoader Instance;

        public class Record
        {
            private string playerName;
            private int score;

            public string PlayerName
            {
                get { return playerName; }
                set { playerName = value; }
            }

            public int Score
            {
                get { return score; }
                set { score = value; }
            }

            public Record(string playerName, int score)
            {
                this.playerName = playerName;
                this.score = score;
            }
        }

        private List<Record> recordList;
        private string filePath;

        static RecordLoader()
        {
            Instance = new RecordLoader();
        }

        private RecordLoader()
        {
            this.recordList = new List<Record>();
            filePath = "Records.txt";

        }

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }


        public bool LoadRecords()
        {
            var newRecordList = new List<Record>();

            string line;
            char[] token = new char[]
            {
                '-'
            };


            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(filePath);
                while ((line = file.ReadLine()) != null)
                {
                    var items = line.Split(token, StringSplitOptions.None);
                    if (items.Length != 2)
                        return false;
                    string playerName = items[0];
                    int playerScore;
                    if (Int32.TryParse(items[1], out playerScore))
                    {
                        newRecordList.Add(new Record(playerName, playerScore));
                    }
                    else
                    {
                        return false;
                    }

                }

                file.Close();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
           
            recordList = newRecordList.OrderByDescending(record => record.Score).ToList();

            return true;
        }

        public Record[] GetTop(int num)
        {
            return recordList.Count > num ? recordList.Take(5).ToArray() : recordList.ToArray();
        }

        public void WriteRecord(string playerName, int playerScore)
        {
            Console.WriteLine(filePath);
            var newRecord = new Record(playerName, playerScore);
            recordList.Add(newRecord);
            recordList = recordList.OrderByDescending(record => record.Score).ToList();

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
            {
                file.WriteLine(newRecord.PlayerName + "-" + newRecord.Score);
            }
        }

    }
}
