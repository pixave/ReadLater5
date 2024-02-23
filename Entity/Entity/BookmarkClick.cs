using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class BookmarkClick
    {
        [Key]
        public int ID { get; set; }

        public int BookmarkID { get; set; }

        public DateTime ClickDateTime { get; set; }

    }
}
