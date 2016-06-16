using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Library.Entities.CustomEntities
{
    public class ResponseContainer<T> : Response
    {
        public T Data { get; set; }
    }
}
