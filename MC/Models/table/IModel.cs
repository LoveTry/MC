using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.Models
{
    interface IModel<T>
    {
        T FindByProperty(string property, object value);
    }
}
