using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DSharpPlus;
using RevolaticTrailblazerBot.Models;

namespace RevolaticTrailblazerBot
{
    public class HiscoresManager
    {
        private HttpClient client { get; set; }
        public HiscoresManager()
        {
            client = new HttpClient
            {
                BaseAddress =
                    new Uri("https://secure.runescape.com/")
            };
        }

        public async Task Get(string username)
        {
            var x = await client.GetAsync($"m=hiscore_oldschool_seasonal/index_lite.ws?player={username.Replace(" ", "%20")}");
            var content = await x.Content.ReadAsStringAsync();

            var allStats = content.Split(Environment.NewLine.ToCharArray()).ToList();

            Data.AllPlayers.TryGetValue(username, out var player);
            player ??= new Player(username);

            Data.AllPlayers.TryAdd(username, player.MapLevels(allStats));
        }

        
    }
}
