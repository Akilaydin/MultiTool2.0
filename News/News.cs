using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGenerator.News {
    class News {

            public string status { get; set; }
            public int totalResults { get; set; }
            public List<Article> articles { get; set; }
        
    }
}
