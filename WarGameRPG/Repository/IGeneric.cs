using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarGameRPG.Repository
{
    internal interface IGeneric<T>
    {
        bool Add(T entity);
        bool Edit(T obj);
    }
}
