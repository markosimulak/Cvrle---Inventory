using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cvrle
{
    interface IProizvod
    {
        bool Add(Proizvod proizvod);

        void Remove(Proizvod proizvod);
    }
}
