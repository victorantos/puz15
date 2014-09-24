using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArrangeGame.Controllers
{
    public class GameResultViewModel
    {
        //in millis
        public int Time { get; set; }
        public string Player { get; set; }
        public Guid GameId { get; set; }
    }
}
