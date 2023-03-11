using System.Collections;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.Types;

public class CrossRelationshipCollection<TRelatedModel, TMainModel> : ICollection<TRelatedModel>
    where TRelatedModel : IToManyRelation<TMainModel>
    where TMainModel : IToManyRelation<TRelatedModel>
{
    private readonly AppDbContext Context;
    private readonly TMainModel Main;

    public CrossRelationshipCollection(AppDbContext context, TMainModel main)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Main = main ?? throw new ArgumentNullException(nameof(main));
    }

    public void Add(TRelatedModel item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (Main.Relation.Contains(item) is false)
        {
            if (Context.Find(typeof(TRelatedModel), item.Id) is null)
                Context.Add(item);

            Main.Relation.Add(item);
            if (item.Relation.Contains(Main) is false)
                item.Relation.Add(Main);
        }
        Context.SaveChanges();
    }

    public void Clear()
    {
        foreach (var i in Main.Relation)
            i.Relation.Remove(Main);
        Main.Relation.Clear();
        Context.SaveChanges();
    }

    public bool Contains(TRelatedModel item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return Main.Relation.Contains(item);
    }

    public void CopyTo(TRelatedModel[] array, int arrayIndex)
        => Main.Relation.CopyTo(array, arrayIndex);

    public bool Remove(TRelatedModel item)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (Main.Relation.Remove(item))
        {
            item.Relation.Remove(Main);
            Context.SaveChanges();
            return true;
        }
        return false;
    }

    public int Count => Main.Relation.Count;
    public bool IsReadOnly => false;

    public IEnumerator<TRelatedModel> GetEnumerator()
        => Main.Relation.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => Main.Relation.GetEnumerator();
}