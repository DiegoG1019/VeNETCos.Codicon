using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.Types;
public class ParentToChildrenRelationshipCollection<TOneModel, TManyModel> : ICollection<TManyModel>
    where TOneModel : class, IID
    where TManyModel : class, IID, IToOneRelation<TOneModel>
{
    private readonly IOneToManyRelation<TOneModel, TManyModel> one;
    private readonly AppDbContext context;

    public ParentToChildrenRelationshipCollection(AppDbContext context, IOneToManyRelation<TOneModel, TManyModel> one)
    {
        this.one = one ?? throw new ArgumentNullException(nameof(one));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Add(TManyModel item)
    {
        if (context.Find(typeof(TManyModel), item.Id) is null)
            context.Add(item);
        if (one.Many.Contains(item) is false)
            one.Many.Add(item);
    }

    public void Clear()
    {
        foreach (var i in one.Many)
            i.Related = null;
        one.Many.Clear();
    }

    public bool Contains(TManyModel item) 
        => one.Many.Contains(item);

    public void CopyTo(TManyModel[] array, int arrayIndex)
        => one.Many.CopyTo(array, arrayIndex);

    public bool Remove(TManyModel item)
    {
        if (one.Many.Remove(item))
        {
            if (item.Related == this)
                item.Related = null;
            return true;
        }
        return false;
    }

    public int Count => one.Many.Count;
    public bool IsReadOnly => false;

    public IEnumerator<TManyModel> GetEnumerator()
        => one.Many.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => one.Many.GetEnumerator();
}
