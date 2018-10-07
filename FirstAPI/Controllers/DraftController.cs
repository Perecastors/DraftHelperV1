using FirstAPI.DbContext;
using FirstAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstAPI.Controllers
{
    [Authorize]
    public class DraftController : Controller
    {
        // GET: Draft
        public ActionResult Draft()
        {
            return View();
        }

        public JsonResult ComputeDraft(DraftInfos draftInfos)
        {
            var matchupDal = new DALMatchup();
            var dal = new DAL();

            var toplaner = dal.getPlayerByName("Vince");
            var jungler = dal.getPlayerByName("Zokato");
            var mid = dal.getPlayerByName("Decay");
            var adc = dal.getPlayerByName("Neylan");
            var support = dal.getPlayerByName("Chypriote");

            var listAnswerTop = matchupDal.getAnswersForDraftByPlayer(toplaner.PlayerId, draftInfos);
            var listAnswerungle = matchupDal.getAnswersForDraftByPlayer(jungler.PlayerId, draftInfos);
            var listAnswerMid = matchupDal.getAnswersForDraftByPlayer(mid.PlayerId, draftInfos);
            var listAnswerAdc = matchupDal.getAnswersForDraftByPlayer(adc.PlayerId, draftInfos);
            var listAnswerSupport = matchupDal.getAnswersForDraftByPlayer(support.PlayerId, draftInfos);

            var listAllAnswers = new List<List<Champion>>();
            listAllAnswers.Add(listAnswerTop);
            listAllAnswers.Add(listAnswerungle);
            listAllAnswers.Add(listAnswerMid);
            listAllAnswers.Add(listAnswerAdc);
            listAllAnswers.Add(listAnswerSupport);

            return Json(listAllAnswers, JsonRequestBehavior.AllowGet);
        }
    }
}