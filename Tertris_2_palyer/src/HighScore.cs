using System;

namespace Tertris_2_palyer
{
    public class HighScore : IComparable<HighScore>
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
        
        public HighScore(string playerName, int score)
        {
            PlayerName = playerName;
            Score = score;
            Date = DateTime.Now;
        }
        
        public int CompareTo(HighScore other)
        {
            return other.Score.CompareTo(this.Score);
        }
        
        public override string ToString()
        {
            return $"{PlayerName}: {Score} points ({Date:MM/dd HH:mm})";
        }
    }
}