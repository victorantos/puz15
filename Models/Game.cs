using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArrangeGame.Models
{
    public class Game
    {
        public Game()
        {
            this.DateCreated = DateTime.UtcNow;
        }
        public Guid GameId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Ip { get; set; }

    }

    public class TopPlayer
    {
        public string Name { get; set; }
        public int Time { get; set; }
    }
}