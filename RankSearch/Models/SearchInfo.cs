using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RankSearch.Models
{
    public class CompanyInfo
    {
        [Required]
        public virtual string SearchString { get; set; }

        [DataType(DataType.Url)]
        public virtual Uri CompanyURL { get; set; }

        public virtual int Rank { get; set; }
    }
}