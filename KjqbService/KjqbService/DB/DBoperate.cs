using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KjqbService.DB
{
    public class DBoperate
    {
        protected KjqbServiceEntities context;
        public DBoperate()
        {
            context = new KjqbServiceEntities();
        }
        

    }
}