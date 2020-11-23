using System.Collections.Concurrent;
using System.Collections.Generic;
using RevolaticTrailblazerBot.Models;

namespace RevolaticTrailblazerBot
{
    public static class Data
    {
        public static List<string> Skills = new List<string>()
        {
            "attack","defence","strength","hitpoints","range","prayer","mage","cooking","wc","fletching","fishing","fm","crafting","smithing","mining","herblore","agility","thieving","slayer","farming","rc","hunter","con"
        };

        public static List<string> Clues = new List<string>()
        {
            "all","beginner","easy","medium","hard","elite","master"
        };

        public static List<string> Bosses = new List<string>()
        {
            "sire","hydra","barrows","bryophyta","callisto","cerberus","cox","cox challenge","chaos elemental","chaos fanatic","zilyana","corp","prime","rex","supreme","crazy archeologist","deranged archeologist","graardor","giant mole","grotesque guardians","hespori","kq","kbd","kraken","kree","kril","mimic","nightmare","obor","sarachnis","scorpia","skotizo","gauntlet","corrupted gauntlet","tob","therm","zuk","jad","venenatis","vet'ion","vorkath","wintertodt","zalcano","zulrah"
        };

        public static List<string> Usernames = new List<string>()
        {
            "thespragg","papaspragg","woodywoody64","hatakushi","Enjoy It","PkingsNub","Ur Mxn Amigo","What A Nub","SirKill","What A Dupe","Im A Bean","barclaycard","jesters alt","oxct3","yewganda","tl reactor15","rise aga1nst","iterrance"
        };

        public static List<int> XP = new List<int>()
        {
            0, 0, 83, 174, 276, 388, 512, 650, 801, 969, 1154, 1358, 1584, 1833, 2107, 2411, 2746, 3115, 3523, 3973,
            4470, 5018, 5624, 6291, 7028, 7842, 8740, 9730, 10824, 12031, 13363, 14833, 16456, 18247, 20224, 22406,
            24815, 27473, 30408, 33648, 37224, 41171, 45529, 50339, 55649, 61512, 67983, 75127, 83014, 91721, 101333,
            111945, 123660, 136594, 150872, 166636, 184040, 203254, 224466, 247886, 273742, 302288, 333804, 368599,
            407015, 449428, 496254, 547953, 605032, 668051, 737627, 814445, 899257, 992895, 1096278, 1210421, 1336443,
            1475581, 1629200, 1798808, 1986068, 2192818, 2421087, 2673114, 2951373, 3258594, 3597792, 3972294, 4385776,
            4842295, 5346332, 5902831, 6517253, 7195629, 7944614, 8771558, 9684577, 10692629, 11805606, 13034431
        };

        public static Player AveragePlayer = null;

        public static ConcurrentDictionary<string, Player> AllPlayers = new ConcurrentDictionary<string, Player>();
    }
}
