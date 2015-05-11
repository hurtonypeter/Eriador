using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eriador.Modules.News.Models
{
    public class NewsPaperViewModel
    {
        public int Id { get; set; }

        public string Editor { get; set; }

        public string RPublisher { get; set; }

        public string REditor { get; set; }

        public string Title { get; set; }

        public List<NewsPaperItemViewModel> News { get; set; } = new List<NewsPaperItemViewModel>();
    }

    public class NewsPaperItemViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string Body { get; set; }
    }
}
