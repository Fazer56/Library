using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Models
{
	public class ItemLibrarianViewModel
	{
		public Librarian Librarian { get; set; }
		public Author Author { get; set; }
		public Item Item { get; set; }
	}
}