using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;

namespace RevolaticTrailblazerBot
{
    public static class Parser
    {
        public static async Task ParseMessage(MessageCreateEventArgs e)
        {
            if (!e.Message.Content.StartsWith("!rhs")) return;
            var msg = e.Message.Content.ToLower().Substring(5);

            var res = "**Command not found**";

            if (msg == "commands" || msg == "help")
            {
                var builder = new StringBuilder();
                builder.AppendLine("**Bot commands:**");
                builder.AppendLine("1. !rhs <skill name>");
                builder.AppendLine("2. !rhs <clue level or all>");
                builder.AppendLine("3. !rhs <boss name>");
                builder.AppendLine("4. !rhs total");
                builder.AppendLine("5. !rhs points");
                builder.AppendLine("6. !rhs average");

                builder.AppendLine("!rhs help skills - Returns a list of skill names");
                builder.AppendLine("!rhs help bosses - Returns a list of boss names");

                res = builder.ToString();
            }

            if (msg.StartsWith("help") && msg != "help")
            {
                msg = msg.Substring(5);
                var builder = new StringBuilder();
                switch (msg)
                {
                    case "skills":
                    {
                        foreach (var x in Data.Skills)
                        {
                            builder.Append($"{x},");
                        }

                        builder.Length -= 1;
                        res = builder.ToString();
                        break;
                    }
                    case "bosses":
                    {
                        foreach (var x in Data.Bosses)
                        {
                            builder.Append($"{x},");
                        }

                        builder.Length -= 1;
                        res = builder.ToString();
                        break;
                    }
                }
            }

            if (msg.StartsWith("add"))
            {
                var player = msg.Substring(4);
                if (!Data.Usernames.Select(x => x.ToLower()).Contains(player.ToLower()))
                {
                    Data.Usernames.Add(player);
                    var hs = new HiscoresManager();
                    await hs.Get(player);
                    res = $"Added {player}";
                }
                else
                {
                    res = $"Account with name {player} has already been added";
                }
            }
            if (msg.StartsWith("remove"))
            {
                var player = msg.Substring(7);
                if (Data.Usernames.Select(x => x.ToLower()).Contains(player.ToLower()))
                {
                    var index = Data.Usernames.Select(x => x.ToLower()).ToList().IndexOf(player);
                    Data.Usernames.RemoveAt(index);
                    res = $"Removed {player}";
                }
                else
                {
                    res = $"No account with the name {player} found";
                }
            }

            if (msg == "usernames") res = string.Join(',', Data.Usernames);
            if (msg == "total") res = GetTotals();
            if (msg == "points") res = GetPoints();
            if (msg == "average") res = GetAverage();

            if (Data.Skills.Contains(msg)) res = GetLevels(msg);
            if (Data.Bosses.Contains(msg)) res = GetBosses(msg);
            if (Data.Clues.Contains(msg)) res = GetClues(msg);

            await e.Message.RespondAsync(res);
        }

        private static string GetPoints()
        {
            var builder = new StringBuilder();
            var count = 1;
            var players = Data.AllPlayers.Values.OrderByDescending(x => x.Points);
            builder.AppendLine("Total point ranking:" + Environment.NewLine);
            foreach (var player in players)
            {
                builder.AppendLine($"**{count + ".",-4}**  {player.Name}  *{player.Points}*");
                count += 1;
            }

            return builder.ToString();
        }

        public static string GetBosses(string boss)
        {
            var builder = new StringBuilder();
            var count = 1;

            var players = Data.AllPlayers.Values.OrderByDescending(x => x.Bosses[boss]);

            builder.AppendLine($"{char.ToUpper(boss[0]) + boss.Substring(1)} rankings:" + Environment.NewLine);

            foreach (var player in players)
            {
                if (player.Bosses[boss] == -1) continue;
                builder.AppendLine($"**{count + ".",-4}** {player.Name} *{player.Bosses[boss]}*");
                count += 1;
            }

            return builder.ToString();
        }

        public static string GetClues(string clue)
        {
            var builder = new StringBuilder();
            var count = 1;

            var players = Data.AllPlayers.Values.OrderByDescending(x => x.Bosses[clue]);

            builder.AppendLine($"{char.ToUpper(clue[0]) + clue.Substring(1)} rankings:" + Environment.NewLine);

            foreach (var player in players)
            {
                if (player.Clues[clue] == -1) continue;
                builder.AppendLine($"**{count + ".",-4}** {player.Name} *{player.Clues[clue]}*");
                count += 1;
            }

            return builder.ToString();
        }

        public static string GetLevels(string level)
        {
            var builder = new StringBuilder();
            var count = 1;

            var players = Data.AllPlayers.Values.OrderByDescending(x => x.Levels[level].Xp);

            builder.AppendLine($"{char.ToUpper(level[0]) + level.Substring(1)} rankings:" + Environment.NewLine);

            foreach (var player in players)
            {
                builder.AppendLine(
                    $"**{count + ".",-4}** {player.Name} **{player.Levels[level].Level}** *{player.Levels[level].Xp:n0}*");
                count += 1;
            }

            return builder.ToString();
        }

        public static string GetTotals()
        {
            var builder = new StringBuilder();
            var count = 1;
            var players = Data.AllPlayers.Values.OrderByDescending(x => x.TotalLevel.Level);
            builder.AppendLine("Total level ranking:" + Environment.NewLine);
            foreach (var player in players)
            {
                builder.AppendLine($"**{count + ".",-4}**  {player.Name}  *{player.TotalLevel.Level}*");
                count += 1;
            }
            return builder.ToString();
        }

        public static string GetAverage()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Average of all revolatic players:" + Environment.NewLine);

            foreach (var skill in Data.AveragePlayer.Levels)
            {
                builder.AppendLine($"**{skill.Key}**     {skill.Value.Level}     *{skill.Value.Xp:n0}*");
            }

            builder.AppendLine($"**Total**     {Data.AveragePlayer.TotalLevel.Level}     *{Data.AveragePlayer.TotalLevel.Xp:n0}*");
            return builder.ToString();
        }
    }
}
