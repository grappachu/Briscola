using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Grappachu.Briscola.Web.Models
{
    public enum GameType
    {
        [Description("")]
        NullGame,

        [Description("Scontro diretto")]
        DirectMatch,

        [Description("Torneo tra robot")]
        RobotTournament
    }
}