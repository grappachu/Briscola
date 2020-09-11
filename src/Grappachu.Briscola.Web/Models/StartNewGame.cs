using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Grappachu.Briscola.Players;

namespace Grappachu.Briscola.Web.Models
{
    public class StartNewGame
    {
        [Display(Name = "Scegli il tipo gioco ti piacerebbe fare")]
        public GameType CurrentGameType { get; set; }

        public IEnumerable<SelectListItem> GameTypes { get; set; }

        [Range(0, 1000, ErrorMessage = "Il campo non può essere negativo o superiore a 1000")]
        [Display(Name = "Quante partite vuoi giocare? (default = 100)")]
        public int NumberOfGames { get; set; }

        [Range(0, 10000, ErrorMessage = "Il campo non può essere negativo o superiore a 10000")]
        [Display(Name = "Quante partite vuoi giocare per ogni match? (default = 1000)")]
        public int NumberOfMatchesPerGame { get; set; }


        [Display(Name = "Scegli il numero di giocatori")]
        public int? NumberOfPlayers { get; set; }
        
        public string FirstCurrentRobot { get; set; }
        public string SecondCurrentRobot { get; set; }
        public string ThirdCurrentRobot { get; set; }
        public string FourthCurrentRobot { get; set; }


        [Display(Name = "Primo robot squadra 1")]
        public IEnumerable<SelectListItem> FirstListOfRobots { get; set; }

        [Display(Name = "Secondo robot squadra 1")]
        public IEnumerable<SelectListItem> SecondListOfRobots { get; set; }

        [Display(Name = "Primo robot squadra 2")]
        public IEnumerable<SelectListItem> ThirdListOfRobots { get; set; }
        
        [Display(Name = "Secondo robot squadra 2")]
        public IEnumerable<SelectListItem> FourthListOfRobots { get; set; }
    }
}