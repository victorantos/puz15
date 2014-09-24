using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArrangeGame.Models
{
    public static class MemoryDB
    {
        const int MaxTopPlayers = 10;
        public static SortedList<TopPlayer, Guid> GetTopPlayers()
        {
            var cacheProvider = new InMemoryCache();
            var newList = new SortedList<TopPlayer, Guid>(MaxTopPlayers, new TopPlayersComparer());

            var topPlayers = cacheProvider.Get<SortedList<TopPlayer, Guid>>("topPlayers", () => newList);
            if(topPlayers.Count == 0)
            {
                // get from db
                var db = new DBContext();
                var topList = (from g in db.games
                              orderby g.Time
                              select new
                              {
                                  Key = new TopPlayer
                                  {
                                      Time = g.Time,
                                      Name = g.Player
                                  },
                                  Value = g.GameId
                              }).AsEnumerable()
                              .Select(d=> new KeyValuePair<TopPlayer, Guid>(d.Key, d.Value)).Take(10);

                foreach (var i in topList)
                    topPlayers[i.Key] = i.Value;
            }
            return topPlayers;
        }

        public static void AddTopPlayers(TopPlayer player, Guid gameId)
        {
            var topPlayers = GetTopPlayers();
            topPlayers.Add(player, gameId);
        }

        public static Guid GenerateGame()
        {
            var gameId = Guid.NewGuid();
            
            var cacheProvider = new InMemoryCache();
            cacheProvider.Get(gameId.ToString(), () => new Game { GameId = gameId, Ip = HttpContext.Current.Request.UserHostAddress });

            return gameId;
        }

       

        public class TopPlayersComparer : IComparer<TopPlayer>
        {
            public int Compare(TopPlayer x, TopPlayer y)
            {
                return x.Time.CompareTo(y.Time);
            }
        }
    }
}