using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myoungblog.Models;

namespace myoungblog.Models
{
	public class Comment
	{
		public int Id { get; set; }
		public int BloggyId { get; set; }
		public string AuthorId { get; set; }
		[AllowHtml]
		public string Body { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Updated { get; set; }
		public string UpdatedReason { get; set; }

		public virtual Post Bloggy { get; set; }
		public virtual ApplicationUser Author { get; set; }
		public string Slug { get; set; }

	}
}
