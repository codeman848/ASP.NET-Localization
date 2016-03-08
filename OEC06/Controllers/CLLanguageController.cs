/*CLLanguageCOntroller.cs
 * a controller to handle the selected language and save and persist it
 * 
 *      Revision History:
 *          Cody Lefebvre:04.02.2015:Created
 *          Cody Lefebvre:04.03.2015:Created index and index postback actions
 *          Cody Lefebvre:04.04.2015:Created override Initialize and set language there
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEC06.Controllers
{
    public class CLLanguageController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //set the language for the system

            base.Initialize(requestContext);

            if (Request.Cookies["language"] != null)
            {
                //check for french
                if (Request.Cookies["language"].Value == "fr")
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture =
                        new System.Globalization.CultureInfo("fr");
                    System.Threading.Thread.CurrentThread.CurrentUICulture =
                        new System.Globalization.CultureInfo("fr");
                }
                //check for german
                if (Request.Cookies["language"].Value == "de")
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture =
                        new System.Globalization.CultureInfo("de");
                    System.Threading.Thread.CurrentThread.CurrentUICulture =
                        new System.Globalization.CultureInfo("de");
                }
            }
                //else its english
            else
            {
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    new System.Globalization.CultureInfo("en");
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo("en");
            }
        }
        // GET: CLLanguage
        public ActionResult Index()
        {
            //create language option for select list
            SelectListItem en = new SelectListItem() { Text = "English", Value = "en", Selected = true };
            SelectListItem de = new SelectListItem() { Text = "German", Value = "de" };
            SelectListItem fr = new SelectListItem() { Text = "Français", Value = "fr" };
            SelectList Languages = new SelectList(new SelectListItem[] { en, de, fr},
                          "Value", "Text");
            ViewBag.Languages = Languages;
            // better return-point determination
           Response.Cookies.Add(new HttpCookie("returnURL", Request.UrlReferrer.PathAndQuery));
            return View();
        }

        // save selected language in a cookie and redirect
        [HttpPost]
        public void Index(string Languages)
        {
            Response.Cookies.Add(new HttpCookie("language", Languages));
            if (Request.Cookies["returnURL"] != null)
                Response.Redirect(Request.Cookies["returnURL"].Value);
            else
                Response.Redirect("/");
        }
    }
}