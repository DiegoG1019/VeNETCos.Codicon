using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeNETCos.Codicon.Database.Models;
public interface IToManyRelation<TModel>
{
    public Guid Id { get; }
    public ICollection<TModel> Relation { get; }
}
