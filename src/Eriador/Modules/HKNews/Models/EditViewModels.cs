using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eriador.Modules.HKNews.Models
{
    public class NewsPaperViewModel
    {
        public string Editor { get; set; }

        public string RPublisher { get; set; }

        public string REditor { get; set; }

        public string Title { get; set; }

        public List<NewsPaperItemViewModel> News { get; set; } = new List<NewsPaperItemViewModel>();
    }

    public class NewsPaperItemViewModel
    {
        public string Title { get; set; }

        public string Link { get; set; }

        public string Body { get; set; }
    }
}
