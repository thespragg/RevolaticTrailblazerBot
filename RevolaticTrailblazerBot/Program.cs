using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using RevolaticTrailblazerBot.Models;

namespace RevolaticTrailblazerBot
{
    class Program
    {
        private static DiscordClient discord;
        private static Config config;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting bot");
            new Thread(() =>
            {
                RepeatActionEvery(async () => await CheckHiScores(),
                    TimeSpan.FromMinutes(15)).Wait();
            }).Start();

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "config.json");
            config = JsonSerializer.Deserialize<Config>(await File.ReadAllTextAsync(path));

            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = config.Token,
                TokenType = TokenType.Bot
            });
            discord.MessageCreated += Parser.ParseMessage; ;

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }

        public static async Task RepeatActionEvery(Action action,
            TimeSpan interval)
        {
            while (true)
            {
                action();
                Task task = Task.Delay(interval);

                try
                {
                    await task;
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }

        public static async Task CheckHiScores()
        {
            Console.WriteLine("Updating hiscores");
            var manager = new HiscoresManager();
            var players = new List<string>(Data.Usernames);
            foreach (var player in players)
            {
                try
                {
                    await manager.Get(player);
                }
                catch{};
            }

            var totLevel = 0;
            var totXp = 0;

            var newAverage = new Player("Average");

            foreach (var skill in Data.Skills)
            {
                var xp = Data.AllPlayers.Values.Sum(x => x.Levels[skill].Xp) / Data.AllPlayers.Count;
                var level = GetLevel(xp);

                totXp += xp;
                totLevel += level;

                newAverage.Levels[skill] = new Stat(level, xp);
            }

            newAverage.TotalLevel = new Stat(totLevel, totXp);
            var message = newAverage.CompareLevels(Data.AveragePlayer);
            if (string.IsNullOrEmpty(message))
            {
                Data.AveragePlayer = newAverage;
                return;
            }
            var channel = await discord.GetChannelAsync(config.Channel);
            await channel.SendMessageAsync(message);

            Data.AveragePlayer = newAverage;
        }

        public static int GetLevel(int experience)
        {
            int index;

            for (index = 0; index < Data.XP.Count; index++)
            {
                if (index == Data.XP.Count - 1) return 99;
                if (Data.XP[index + 1] > experience)
                    break;
            }

            return index;
        }
    }
}
