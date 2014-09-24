using ArrangeGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ArrangeGame.Controllers
{
    public class ArrangeGameController : ApiController
    {
        private DBContext db = new DBContext();

        [HttpGet]
        public IList<TopPlayer> TopPlayers()
        {
            return MemoryDB.GetTopPlayers().Keys;
        }

        // POST api/<controller>
        [HttpPost]
        async public Task<HttpResponseMessage> SaveGame(GameResultViewModel result)
        {
            var modelStateErrors = ModelState.Values.ToList();

            List<string> errors = new List<string>();

            foreach (var s in modelStateErrors)
                foreach (var e in s.Errors)
                    if (e.ErrorMessage != null && e.ErrorMessage.Trim() != "")
                        errors.Add(e.ErrorMessage);

            var cacheProvider = new InMemoryCache();
            var game = (Game)HttpContext.Current.Cache[result.GameId.ToString()];
            if (game == null)
                errors.Add("The game has expired.");
            if (game == null || game.Ip != HttpContext.Current.Request.UserHostAddress
                || game.GameId != result.GameId
                || (DateTime.UtcNow - game.DateCreated).TotalMilliseconds < result.Time)
            {
                errors.Add("This is not a valid game.");
            }

            if (errors.Count == 0)
            {
                //try
                //{
                    // store the result
                    db.games.Add(new GameHistory
                    {
                        Ip = game.Ip,
                        Player = result.Player,
                        Time = result.Time,
                        GameId = result.GameId,
                        Moves = null //TODO
                    });
                    // remove the game from cache
                    cacheProvider.Remove(game.GameId.ToString());

                    await db.SaveChangesAsync();
                    MemoryDB.AddTopPlayers(new TopPlayer { Name = result.Player, Time = result.Time }, game.GameId);
                    return Request.CreateResponse(HttpStatusCode.Accepted);
                //}
                //catch
                //{
                //    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                //}
            }
            else
            {
                return Request.CreateResponse<List<string>>(HttpStatusCode.BadRequest, errors);
            }
        }

        // PUT api/<controller>/5
        [HttpPut]
        async public Task<HttpResponseMessage> SavePlayer(GameResultViewModel player)
        {
            List<string> errors = new List<string>();
             var game = db.games.Where(g => g.GameId == player.GameId).SingleOrDefault();
                  
            if(game == null)
                errors.Add("No such game found");
            // no errors
            if (errors.Count == 0)
            {
                try
                {
                    game.Player = player.Player;
                    await db.SaveChangesAsync();

                    // update cache for top players list
                    foreach (var item in MemoryDB.GetTopPlayers())
                        if (item.Value == game.GameId)
                            item.Key.Name = game.Player;
                    
                    return Request.CreateResponse(HttpStatusCode.Accepted);
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                return Request.CreateResponse<List<string>>(HttpStatusCode.BadRequest, errors);
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}