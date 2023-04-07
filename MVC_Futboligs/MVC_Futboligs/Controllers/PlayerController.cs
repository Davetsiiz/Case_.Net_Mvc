using MVC_Futboligs.Concrete;
using MVC_Futboligs.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVC_Futboligs.Controllers
{
    public class PlayerController : Controller
    {
        //Context
        DbContextFutboligs db = new DbContextFutboligs();

        //ListPage
        public ActionResult Index()
        {
            
            string qu = "Select Id,Name,Photo,Age,TeamId from Player order by Name";
            var players = db.Database.SqlQuery<Player>(qu).ToList();
            return View(players);

        }

        //ajax-script-controller (sayfa yenilemesi için)
        public ActionResult GetUpdateData()
        {

            string qu = "Select Id,Name,Photo,Age,TeamId from Player order by Name";
            var data = db.Database.SqlQuery<Player>(qu).ToList();
            //return Json(data, JsonRequestBehavior.AllowGet);
            
            return Content(JsonConvert.SerializeObject(data), "application/json");
        }

        //fotoğraf ekleme scripti barındırıyor.
        [HttpGet]
        public ActionResult PlayerAdd()
        {
            ViewBag.tId = new SelectList(db.TeamsS, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult PlayerAdd(Player play)
        {
            var paramphoto = "";
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    string newimage = Path.GetFileName(Request.Files[0].FileName);
                    string imagepath = Path.Combine(Server.MapPath("/Images/"), newimage);
                    Request.Files[0].SaveAs(imagepath);
                    paramphoto = "/Images/" + newimage;
                }
            }
            ViewBag.tId = new SelectList(db.TeamsS, "Id", "Name", play.TeamId);
            string qu = "Insert into Player (Name, Photo, Age, TeamId) values (@Name, @Photo, @Age, @TeamId)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Name",play.Name),
                new SqlParameter("@Age", play.Age),
                new SqlParameter("@TeamId", play.TeamId),
                new SqlParameter("@Photo", paramphoto)
            };

            db.Database.ExecuteSqlCommand(qu, parameters);
            return RedirectToAction("Index");
        }

        //direkt siliyor linq sorgusu hata verdi?? // aynı hatayı TeamsController da vermedi????
        public ActionResult PlayerDelete(int id)
        {
            string qu = "Delete from Player where ID=@Id";
            db.Database.ExecuteSqlCommand(qu, new SqlParameter("@ID", id));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult PlayerEdit(int id) 
        {
            ViewBag.tId = new SelectList(db.TeamsS, "Id", "Name");
            string qu = "Select Id,Name,Photo,Age,TeamId from Player where ID=@Id";
            new SqlParameter("@Id", Convert.ToInt32(id.ToString()));
            var play = db.Database.SqlQuery<Player>(qu, new SqlParameter("@Id", id)).First(); ;   
            return View(play);
        }
        [HttpPost]
        public ActionResult PlayerEdit(Player play)
        {
            var paramphoto = "";
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    string newimage = Path.GetFileName(Request.Files[0].FileName);
                    string imagepath = Path.Combine(Server.MapPath("/Images/"), newimage);
                    Request.Files[0].SaveAs(imagepath);
                    paramphoto = "/Images/" + newimage;
                }
            }
            ViewBag.tId = new SelectList(db.TeamsS, "Id", "Name", play.TeamId);
            string qu = "Update  Player set Name=@Name, Photo=@Photo, Age=@Age, TeamId=@TeamId where ID=@Id";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Name",play.Name),
                new SqlParameter("@Id",Convert.ToInt32(play.Id.ToString())),
                new SqlParameter("@Age", play.Age),
                new SqlParameter("@TeamId", play.TeamId),
                new SqlParameter("@Photo", paramphoto)
            };

            db.Database.ExecuteSqlCommand(qu, parameters);
            return RedirectToAction("Index");
        }
    }
}

