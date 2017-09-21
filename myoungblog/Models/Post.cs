using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace myoungblog.Models
{
	public class Post
	{
		public Post() //Constructor
		{
			Comments = new HashSet<Comment>(); // Comments is the name of the virtual property faster way to search through the table
		}
		public int Id { get; set; }
		public string Title { get; set; }
		[AllowHtml]
		public string Body { get; set; }
		public DateTime Created { get; set; }
		public DateTime? Updated { get; set; }
		public string MediaUrl { get; set; }
		public bool Published { get; set; } //when bool is true it will be visual to the public, false only private
		public string Slug { get; set; }
		
		public virtual ICollection<Comment> Comments { get; set; } //concentrates all properties of other table so it is not a liniar search
	}
}