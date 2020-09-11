using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Grappachu.Briscola.Exceptions;
using Grappachu.Briscola.Web.Models;
using Grappachu.Briscola.Web.Services;

namespace Grappachu.Briscola.Web.Controllers
{
    public class BriscolaController : Controller
    {
        private static BriscolaService briscolaService;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        // GET: Briscola
        [HttpGet]
        public ActionResult StartNewGame()
        {
            log.Info("Chiamata action StartNewGame");

            briscolaService = new BriscolaService();

            StartNewGame startNewGame = briscolaService.InitGame();

            return View(startNewGame);
        }

        [HttpPost]
        public ActionResult ContinueNewGame(StartNewGame startNewGame)
        {
            log.Info("Chiamata action ContinueNewGame");

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        startNewGame = briscolaService.StartGame(startNewGame);
                        
                        log.Info("Dati corretti. Partita iniziata");

                        if (startNewGame.CurrentGameType != GameType.DirectMatch)
                        {
                            log.Info("Risultato finale diretto");

                            return RedirectToAction("GameResult", startNewGame);
                        }

                        return View(startNewGame);
                    }
                    catch (BriscolaException ex)
                    {
                        ModelState.AddModelError("", ex.Message);

                        startNewGame.GameTypes = briscolaService.GetGameTypes();

                        log.Error("Eccezione pagina StartNewGame");

                        return View("StartNewGame", startNewGame);
                    }
                }
                return RedirectToAction("StartNewGame", "Briscola");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Shared");
            }
        }
        public ActionResult GameResult(StartNewGame startNewGame)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        GameResult gameResult = briscolaService.Result(startNewGame);

                        briscolaService.Play(gameResult);

                        log.Info("Dati corretti. Partita conclusa");

                        return View(gameResult);
                    }
                    catch (BriscolaException ex)
                    {
                        ModelState.AddModelError("", ex.Message);

                        startNewGame.FirstListOfRobots = briscolaService.GetAllRobots();
                        startNewGame.SecondListOfRobots = briscolaService.GetAllRobots();
                        startNewGame.ThirdListOfRobots = briscolaService.GetAllRobots();
                        startNewGame.FourthListOfRobots = briscolaService.GetAllRobots();

                        log.Error("Eccezione pagina ContinueNewGame", ex);

                        return View("ContinueNewGame", startNewGame);
                    }
                }
                return RedirectToAction("ContinueNewGame", "Briscola");
            }
            catch (Exception ex)
            {
                log.Fatal("Fatal error", ex);

                return View("Error", ex);
            }
        }
    }
}