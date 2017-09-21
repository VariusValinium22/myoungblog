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
	public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Blogs
		public ActionResult Index(int? page)
		{
			int pageSize = 3;
			int pageNumber = (page ?? 1);
			if (Request.IsAuthenticated && User.IsInRole("Admin"))
			{
				return View(db.Posts.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
			}
			return View(db.Posts.Where(p => p.Published == true).OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
		}
		[HttpPost]
		public ActionResult Index(string searchStr, int? page)
		{
			int pageSize = 3;
			int pageNumber = (page ?? 1);
			ViewBag.Search = searchStr;//This is a way to make the initial search input into a var
			SearchHelper search = new SearchHelper();
			var blogList = search.IndexSearch(searchStr);

			if (Request.IsAuthenticated && User.IsInRole("Admin"))
			{
				return View(blogList.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
			}
			return View(blogList.Where(p => p.Published == true).OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
			}

		// GET: Blogs/Details/5
		public ActionResult Details(string Slug)
		{
			if (String.IsNullOrWhiteSpace(Slug))
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Post blog = db.Posts.FirstOrDefault(p => p.Slug == Slug);
			if (blog == null)
			{
				return HttpNotFound();
			}
			return View(blog);
		}

		// GET: Blogs/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Blogs/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,Title,Body,MediaURL,Published")] Post blog)
		{
			if (ModelState.IsValid)
			{
				var Slug = StringUtilities.URLFriendly(blog.Title);
				if (String.IsNullOrWhiteSpace(Slug))
				{
					ModelState.AddModelError("Title", "Invalid title");
					return View(blog);
				}
				if (db.Posts.Any(p => p.Slug == Slug))
				{
					ModelState.AddModelError("Title", "The title must be unique");
					return View(blog);
				}

				blog.Slug = Slug;
				blog.Created = DateTime.Now;
				db.Posts.Add(blog);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(blog);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin")]
		public ActionResult Create([Bind(Include = "Id,Name,Post")] Post post, HttpPostedFileBase image)
		{
			if (image != null && image.ContentLength > 0)
			{
				var ext = Path.GetExtension(image.FileName).ToLower();//this validates the image
				if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
					ModelState.AddModelError("image", "Invalid");
			}
			if (ModelState.IsValid)
			{
				var filePath = "/Assets/Pics/";// used for media url, creating the path of the file to the database
				var absPath = Server.MapPath("~" + filePath);
				post.Created = System.DateTime.Now;
				post.MediaUrl = filePath + image.FileName;
				image.SaveAs(Path.Combine(absPath, image.FileName));
				db.Posts.Add(post);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(post);
		}
		// GET: Blogs/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Post blog = db.Posts.Find(id);
			if (blog == null)
			{
				return HttpNotFound();
			}
			return View(blog);
		}

		// POST: Blogs/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Title,Body,MediaUrl,Published")] Post blog)
		{
			if (ModelState.IsValid)
			{
				db.Entry(blog).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(blog);
		}

		// GET: Blogs/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Post blog = db.Posts.Find(id);
			if (blog == null)
			{
				return HttpNotFound();
			}
			return View(blog);
		}

		// POST: Blogs/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Post blog = db.Posts.Find(id);
			db.Posts.Remove(blog);
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
