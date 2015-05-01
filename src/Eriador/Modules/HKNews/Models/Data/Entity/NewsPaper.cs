using Eriador.Models.Data.Entity;
using System;
using System.Collections.Generic;

namespace Eriador.Modules.HKNews.Models.Data.Entity
{
    public class NewsPaper
    {
		public int Id { get; set; }

		public User Editor { get; set; }

		public string REditor { get; set; }

		public string RPublisher { get; set; }

		public DateTime Created { get; set; }

		public DateTime LastEdited { get; set; }

		public DateTime Sent { get; set; }

		public string Title { get; set; }

		public virtual ICollection<NewsItem> NewsItems { get; set; } 
	}
}