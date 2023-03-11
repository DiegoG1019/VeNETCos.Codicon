using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeNETCos.Codicon.Database.Models;
public interface IOneToManyRelation<TOne, TMany>
    where TMany : class, IID, IToOneRelation<TOne>
    where TOne : class, IID
{
    ICollection<TMany> Many { get; }
    TOne One { get; }
}

public interface IToOneRelation<TRelated>
{
    TRelated? Related { get; set; }
}

public interface IID
{
    Guid Id { get; }
}
