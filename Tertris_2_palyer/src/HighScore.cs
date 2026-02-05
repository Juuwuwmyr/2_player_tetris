using System;
using System.Collections.Generic;
using System.IO;

namespace Tertris_2_palyer
{
    public class HighScore : IComparable<HighScore>
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }

        public HighScore(string playerName, int score, DateTime date)
        {
            PlayerName = playerName;
            Score = score;
            Date = date;
        }

        public int CompareTo(HighScore other)
        {
            return other.Score.CompareTo(this.Score);
        }

        public override string ToString()
        {
            return $"{PlayerName} - {Score} ({Date:MM/dd/yyyy})";
        }
        public static void SaveToFile(string path, List<HighScore> scores)
        {
            List<string> lines = new List<string>();

            foreach (var score in scores)
            {
                lines.Add($"{score.PlayerName}|{score.Score}|{score.Date:MM/dd/yyyy}");
            }

            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllLines(path, lines);
        }

        public static List<HighScore> LoadFromFile(string path)
        {
            List<HighScore> scores = new List<HighScore>();

            if (!File.Exists(path))
                return scores;

            foreach (var line in File.ReadAllLines(path))
            {
                string[] parts = line.Split('|');
                if (parts.Length != 3) continue;

                string name = parts[0];
                int score = int.Parse(parts[1]);
                DateTime date = DateTime.Parse(parts[2]);

                scores.Add(new HighScore(name, score, date));
            }

            scores.Sort();
            return scores;
        }
    }
}
