using System;

namespace Eriador.Modules.HKNews.Models.Data.Entity
{
    public class NewsItem
    {
		public int Id { get; set; }

		public string Title { get; set; }

		public string Link { get; set; }

		public string Body { get; set; }

		public virtual NewsPaper NewsPaper { get; set; }
	}
}