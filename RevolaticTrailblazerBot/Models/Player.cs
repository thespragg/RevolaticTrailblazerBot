using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RevolaticTrailblazerBot.Models
{
    public class Player
    {
        public string Name { get; set; }
        public Stat TotalLevel { get; set; }
        public int Points { get; set; }
        public ConcurrentDictionary<string, Stat> Levels { get; set; }
        public ConcurrentDictionary<string, int> Clues { get; set; }
        public ConcurrentDictionary<string, int> Bosses { get; set; }
        public Player(string name)
        {
            Name = name;
            Levels = new ConcurrentDictionary<string, Stat>();
            Clues = new ConcurrentDictionary<string, int>();
            Bosses = new ConcurrentDictionary<string, int>();
        }

        public Player MapLevels(List<string> stats)
        {
            Levels = new ConcurrentDictionary<string, Stat>();
            Clues = new ConcurrentDictionary<string, int>();
            Bosses = new ConcurrentDictionary<string, int>();

            stats.RemoveAt(25);
            stats.RemoveAt(25);
            stats.RemoveAt(32);

            var total = stats[0].Split(',').ToList();
            TotalLevel = new Stat(int.Parse(total[1]), int.Parse(total[2]));

            var points = stats[24].Split(',').ToList();
            Points = int.Parse(points[1]);

            var skills = stats.GetRange(1, 23);
            var clues = stats.GetRange(25, 7);
            var bosses = stats.GetRange(32, 44);

            for (var i = 0; i < skills.Count; i++)
            {
                var split = skills[i].Split(',').ToList();
                Levels.TryAdd(Data.Skills[i], new Stat(int.Parse(split[1]), int.Parse(split[2])));
            }

            for (var i = 0; i < clues.Count; i++)
            {
                var split = clues[i].Split(',').ToList();
                Clues.TryAdd(Data.Clues[i], int.Parse(split[1]));
            }

            for (var i = 0; i < bosses.Count; i++)
            {
                var split = bosses[i].Split(',').ToList();
                Bosses.TryAdd(Data.Bosses[i], int.Parse(split[1]));
            }

            return this;
        }

        
    }

    public static class PlayerExtensions
    {
        public static string CompareLevels(this Player playerNew, Player playerOld)
        {
            var builder = new StringBuilder();

            if (playerOld == null) return "";
            foreach (var skill in playerNew.Levels)
            {
                if (skill.Value.Level > playerOld.Levels[skill.Key].Level)
                {
                    builder.AppendLine($"Clan average is now level {skill.Value.Level} in {skill.Key}");
                }
            }

            return builder.ToString();
        }
    }


    public class Stat
    {
        public int Level { get; set; }
        public int Xp { get; set; }

        public Stat(int level, int xp)
        {
            Level = level;
            Xp = xp;
        }
    }
}
