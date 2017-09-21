using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using myoungblog.Models;
using myoungblog.Models.Helpers.StringUtilites;
using PagedList;
using PagedList.Mvc;

namespace myoungblog.Controllers
{
	[RequireHttps]
	public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Comments
		//public ActionResult Index(int? page)
		//{
		//	int pageSize = 3;
		//	int pageNumber = (page ?? 1);
		//	if (Request.IsAuthenticated && User.IsInRole("Admin"))
		//	{
		//		return View(db.Posts.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
		//	}
		//	return View(db.Posts.Where(p => p.Published == true).OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
		//}
		//[HttpPost]
		//public ActionResult Index(string searchStr, int? page)
		//{
		//	int pageSize = 3;
		//	int pageNumber = (page ?? 1);
		//	ViewBag.Search = searchStr;//This is a way to make the initial search input into a var
		//	SearchHelper search = new SearchHelper();
		//	var blogList = search.IndexSearch(searchStr);

		//	if (Request.IsAuthenticated && User.IsInRole("Admin"))
		//	{
		//		return View(blogList.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
		//	}
		//	return View(blogList.Where(p => p.Published == true).OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
		//}

		//// GET: Comments/Details/5
		//public ActionResult Details(string Slug)
		//{
		//	if (String.IsNullOrWhiteSpace(Slug))
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}
		//	Post comment = db.Posts.FirstOrDefault(p => p.Slug == Slug);
		//	if (comment == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	return View(comment);
		//}

		//// GET: Comments/Create
		//public ActionResult Create()
  //      {
  //          return View();
  //      }

		// POST: Comments/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,Body")] Comment comment)
		{
			if (ModelState.IsValid)
			{
				var Slug = StringUtilities.URLFriendly(comment.Body);
				if (String.IsNullOrWhiteSpace(Slug))
				{
					ModelState.AddModelError("Title", "Invalid title");
					return View(comment);
				}
				if (db.Posts.Any(p => p.Slug == Slug))
				{
					ModelState.AddModelError("Title", "The title must be unique");
					return View(comment);
				}

				comment.Slug = Slug;
				comment.Created = DateTime.Now;
				db.Comments.Add(comment);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(comment);
		}
			//[ValidateAntiForgeryToken]
			//[Authorize(Roles = "Admin")]
			//public ActionResult Create([Bind(Include = "Id,Name,Price,Description")] Post post, HttpPostedFileBase image)
			//{
			//	if (image != null && image.ContentLength > 0)
			//	{
			//		var ext = Path.GetExtension(image.FileName).ToLower();//this validates the image
			//		if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
			//			ModelState.AddModelError("image", "Invalid");
			//	}
			//	if (ModelState.IsValid)
			//	{
			//		var filePath = "/Assets/Pics/";// used for media url, creating the path of the file to the database
			//		var absPath = Server.MapPath("~" + filePath);
			//		post.Created = System.DateTime.Now;
			//		post.MediaUrl = filePath + image.FileName;
			//		image.SaveAs(Path.Combine(absPath, image.FileName));
			//		db.Posts.Add(post);
			//		db.SaveChanges();
			//		return RedirectToAction("Index");
			//	}

			//	return View(post);
			//}

			// GET: Comments/Edit/5
			public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Body")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
