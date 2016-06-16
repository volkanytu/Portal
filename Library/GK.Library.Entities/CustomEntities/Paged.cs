using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Entities.CustomEntities
{
    public class Paged<T>
    {
        public Paged()
        {
            ItemList = new List<T>();
        }

        public List<T> ItemList { get; set; }
        public int ItemCount { get; set; }
    }
}
