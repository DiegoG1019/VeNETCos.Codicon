using System.Collections;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.Types;

public class CrossRelationshipCollection<TRelatedModel, TMainModel> : ICollection<TRelatedModel>
    where TRelatedModel : IToManyRelation<TMainModel>
    where TMainModel : IToManyRelation<TRelatedModel>
{
    private readonly Guid MainId;
    private readonly Func<AppDbContext, Guid, TMainModel> Query;
    private readonly Func<AppDbContext, Guid, TRelatedModel> RelatedQuery;

    public CrossRelationshipCollection(Guid mainId, Func<AppDbContext, Guid, TMainModel> query, Func<AppDbContext, Guid, TRelatedModel> relatedQuery)
    {
        MainId = mainId;
        Query = query ?? throw new ArgumentNullException(nameof(query));
        RelatedQuery = relatedQuery ?? throw new ArgumentNullException(nameof(relatedQuery));
    }

    public void Add(Guid itemId)
    {
        using (AppServices.GetDbContext(out var context))
        {
            var model = RelatedQuery(context, itemId);
            InternalAdd(model, context);
        }
    }

    public void Add(TRelatedModel item)
    {
        ArgumentNullException.ThrowIfNull(item);

        using (AppServices.GetDbContext(out var context))
            InternalAdd(item, context);
    }

    private void InternalAdd(TRelatedModel item, AppDbContext context)
    {
        var main = Query(context, MainId);
        if (main.Relation.Contains(item) is false)
        {
            if (context.Find(typeof(TRelatedModel), item.Id) is null)
                context.Add(item);

            main.Relation.Add(item);
            if (item.Relation.Contains(main) is false)
                item.Relation.Add(main);
        }
        context.SaveChanges();
    }

    public void Clear()
    {
        using (AppServices.GetDbContext(out var context))
        {
            var main = Query(context, MainId);
            foreach (var i in main.Relation)
                i.Relation.Remove(main);
            main.Relation.Clear();
            context.SaveChanges();
        }
    }

    public bool Contains(Guid itemId)
    {
        using (AppServices.GetDbContext(out var context))
        {
            var model = RelatedQuery(context, itemId);
            return InternalContains(model, context);
        }
    }

    public bool Contains(TRelatedModel item)
    {
        ArgumentNullException.ThrowIfNull(item);
        using (AppServices.GetDbContext(out var context))
            return InternalContains(item, context);
    }

    private bool InternalContains(TRelatedModel item, AppDbContext context)
    {
        var main = Query(context, MainId);
        return main.Relation.Contains(item);
    }

    public void CopyTo(TRelatedModel[] array, int arrayIndex)
    {
        using (AppServices.GetDbContext(out var context))
        {
            var main = Query(context, MainId);
            main.Relation.CopyTo(array, arrayIndex);
        }
    }

    public bool Remove(Guid itemId)
    {
        using (AppServices.GetDbContext(out var context))
        {
            var model = RelatedQuery(context, itemId);
            return InternalRemove(model, context);
        }
    }

    public bool Remove(TRelatedModel item)
    {
        ArgumentNullException.ThrowIfNull(item);

        using (AppServices.GetDbContext(out var context))
            return InternalRemove(item, context);
    }

    private bool InternalRemove(TRelatedModel item, AppDbContext context)
    {
        var main = Query(context, MainId);
        if (main.Relation.Remove(item))
        {
            item.Relation.Remove(main);
            context.SaveChanges();
            return true;
        }
        return false;
    }

    public int Count
    {
        get
        {
            using (AppServices.GetDbContext(out var context))
                return Query(context, MainId).Relation.Count;
        }
    }

    public bool IsReadOnly => false;

    public IEnumerator<TRelatedModel> GetEnumerator()
    {
        using (AppServices.GetDbContext(out var context))
        {
            var main = Query(context, MainId);
            foreach (var relation in main.Relation)
                yield return relation;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}