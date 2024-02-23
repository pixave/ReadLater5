using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Dashboard
    {
        [Key]
        public string id { get; set; }
        public string username{ get; set; }
        public int numbookmarks { get; set; }
        public int numclicks { get;set; }
        public List<DashboardBookmark> bookmarks { get; set; }

        public Dashboard() {
            this.id = "";
            this.username = "";
            this.bookmarks = new List<DashboardBookmark>();
            this.numbookmarks = 0;
            this.numclicks = 0;
        }

    }
}
