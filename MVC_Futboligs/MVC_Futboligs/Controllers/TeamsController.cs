using Microsoft.Ajax.Utilities;
using MVC_Futboligs.Concrete;
using MVC_Futboligs.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Helpers;
using System.Web.Management;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;


namespace MVC_Futboligs.Controllers
{
    public class TeamsController : Controller
    {
        DbContextFutboligs db = new DbContextFutboligs();

        // GET: Teams
        public ActionResult Index()
        {
            string qu = "Select Id, Name,Logo, Coach from Teams order by Name";
            var teams = db.Database.SqlQuery<Teams>(qu).ToList();
            return View(teams);
        }



        public ActionResult DeleteTeam(int id)
        {
            var team = db.TeamsS.FirstOrDefault(s => s.Id == id);
            if (team != null)
            {
                string qu = "Delete from Teams where ID=@Id";
                db.Database.ExecuteSqlCommand(qu, new SqlParameter("@ID", id));
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult AddTeam()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddTeam(Teams teams)
        {
            var paramlogo = "";
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    string newimage = Path.GetFileName(Request.Files[0].FileName);
                    string imagepath = Path.Combine(Server.MapPath("/Images/"), newimage);
                    Request.Files[0].SaveAs(imagepath);
                    paramlogo = "/Images/" + newimage;
                }
            }

            string qu = "Insert into Teams (Name, Logo, Coach) values (@Name, @Logo, @Coach)";
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@Name", teams.Name),
        new SqlParameter("@Coach", teams.Coach),
        new SqlParameter("@Logo", paramlogo)
            };

            db.Database.ExecuteSqlCommand(qu, parameters);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditTeam(int id)
        {
            var teams = db.TeamsS.Find(id);
            return View(teams);
        }



        [HttpPost]
        public ActionResult EditTeam(Teams teams, HttpPostedFileBase file)
        {
            var paramlogo = "";
            if (file != null)
            {
                string newimage = Path.GetFileName(Request.Files[0].FileName);
                string imagepath = Path.Combine(Server.MapPath("/Images/"), newimage);
                Request.Files[0].SaveAs(imagepath);
                paramlogo = "/Images/" + newimage;
                string qu = "Update Teams set Name=@Name,Logo=@Logo,Coach=@Coach where Id=@Id";
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@Id",Convert.ToInt32(teams.Id.ToString())),
                new SqlParameter("@Name",teams.Name),
                new SqlParameter("@Logo",paramlogo),
                new SqlParameter("@Coach",teams.Coach),
                };
                db.Database.ExecuteSqlCommand(qu, parameters);
                return RedirectToAction("Index");
            }

            else if (file == null)
            {
                string qu = "Update Teams set Name=@Name,Coach=@Coach where Id=@Id";
                SqlParameter[] parameters = new SqlParameter[]
               {
                new SqlParameter("@Id",Convert.ToInt32(teams.Id.ToString())),
                new SqlParameter("@Name",teams.Name),
                new SqlParameter("@Coach",teams.Coach),
               };
                db.Database.ExecuteSqlCommand(qu, parameters);
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Index");




        }

        public ActionResult GetUpdateData()
        {

            string qu = "Select Id, Name,Logo, Coach from Teams order by Name";
            var data = db.Database.SqlQuery<Teams>(qu).ToList();
            //return Json(data, JsonRequestBehavior.AllowGet);
            return Content(JsonConvert.SerializeObject(data), "application/json");
        }

    }
}