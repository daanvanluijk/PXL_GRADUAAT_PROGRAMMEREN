using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shpielerij.SQL.Results
{
    public class ObjectsResult<T> : BaseResult
    {
        public T[] Objects;
    }
}
