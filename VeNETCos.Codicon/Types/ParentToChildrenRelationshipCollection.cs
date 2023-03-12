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
    public Guid MainId { get; }
    public Func<AppDbContext, Guid, IOneToManyRelation<TOneModel, TManyModel>> EnumerateQuery { get; }
    public Func<AppDbContext, Guid, IOneToManyRelation<TOneModel, TManyModel>> ModifyQuery { get; }
    public Func<AppDbContext, Guid, TManyModel> RelatedQuery { get; }
    public Func<AppDbContext, TManyModel, bool> CheckIfExists { get; }

    public ParentToChildrenRelationshipCollection(
        Guid mainId, 
        Func<AppDbContext, Guid, IOneToManyRelation<TOneModel, TManyModel>> enumerateQuery,
        Func<AppDbContext, Guid, IOneToManyRelation<TOneModel, TManyModel>> modifyQuery,
        Func<AppDbContext, Guid, TManyModel> relatedQuery,
        Func<AppDbContext, TManyModel, bool> checkIfExists
    )
    {
        MainId = mainId;
        CheckIfExists = checkIfExists ?? throw new ArgumentNullException(nameof(checkIfExists));
        EnumerateQuery = enumerateQuery ?? throw new ArgumentNullException(nameof(enumerateQuery));
        RelatedQuery = relatedQuery ?? throw new ArgumentNullException(nameof(relatedQuery));
        ModifyQuery = modifyQuery ?? throw new ArgumentNullException(nameof(modifyQuery));
    }

    public void Add(Guid itemId)
    {
        using (AppServices.GetDbContext(out var context))
        {
            var model = RelatedQuery(context, itemId);
            InternalAdd(model, context);
        }
    }

    public void Add(TManyModel item)
    {
        ArgumentNullException.ThrowIfNull(item);

        using (AppServices.GetDbContext(out var context))
            InternalAdd(item, context);
    }

    private void InternalAdd(TManyModel item, AppDbContext context)
    {
        if (CheckIfExists(context, item) is false)
            context.Add(item);

        var one = ModifyQuery(context, MainId);
        if (one.Many.Contains(item) is false)
            one.Many.Add(item);

        context.SaveChanges();
    }

    public void Clear()
    {
        using (AppServices.GetDbContext(out var context))
        {
            var one = EnumerateQuery(context, MainId);
            foreach (var i in one.Many)
                i.Related = null;
            one.Many.Clear();

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

    public bool Contains(TManyModel item)
    {
        ArgumentNullException.ThrowIfNull(item);
        using (AppServices.GetDbContext(out var context))
            return InternalContains(item, context);
    }

    private bool InternalContains(TManyModel item, AppDbContext context)
    {
        var one = EnumerateQuery(context, MainId);
        return one.Many.Contains(item);
    }

    public void CopyTo(TManyModel[] array, int arrayIndex)
    {
        using (AppServices.GetDbContext(out var context))
        {
            var one = EnumerateQuery(context, MainId);
            one.Many.CopyTo(array, arrayIndex);
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

    public bool Remove(TManyModel item)
    {
        ArgumentNullException.ThrowIfNull(item);

        using (AppServices.GetDbContext(out var context))
            return InternalRemove(item, context);
    }

    private bool InternalRemove(TManyModel item, AppDbContext context)
    {
        var one = EnumerateQuery(context, MainId);
        if (one.Many.Remove(item))
        {
            if (item.Related == this)
                item.Related = null;
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
            {
                var one = EnumerateQuery(context, MainId);
                return one.Many.Count;
            }
        }
    }

    public bool IsReadOnly => false;

    public IEnumerator<TManyModel> GetEnumerator()
    {
        using (AppServices.GetDbContext(out var context))
        {
            var main = EnumerateQuery(context, MainId);
            foreach (var relation in main.Many)
                yield return relation;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
