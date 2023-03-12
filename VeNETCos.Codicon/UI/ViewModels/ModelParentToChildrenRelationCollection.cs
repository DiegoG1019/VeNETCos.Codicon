using System.Collections;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;

namespace VeNETCos.Codicon.UI.ViewModels;

public class ModelParentToChildrenRelationCollection<TOneModelView, TOneModel, TManyModelView, TManyModel> 
    : ICollection<TManyModelView>
    where TOneModelView : class
    where TManyModelView : class, IModelView<TManyModel>
    where TOneModel : class, IID
    where TManyModel : class, IID, IToOneRelation<TOneModel>
{
    private readonly ParentToChildrenRelationshipCollection<TOneModel, TManyModel> collection;
    private readonly Dictionary<Guid, TManyModelView> viewModels = new();
    private readonly Func<TManyModel, TManyModelView> ModelFactory;

    public ModelParentToChildrenRelationCollection(
        ParentToChildrenRelationshipCollection<TOneModel, TManyModel> collection, 
        Func<TManyModel, TManyModelView> modelFactory
    )
    {
        ArgumentNullException.ThrowIfNull(modelFactory);
        this.collection = collection ?? throw new ArgumentNullException(nameof(collection));

        ModelFactory = modelFactory;
    }

    public void Update()
    {
        foreach (var m in viewModels)
            if (collection.Contains(m.Key) is false)
                viewModels.Remove(m.Key);
        foreach (var m in collection)
            if (viewModels.ContainsKey(m.Id) is false)
                viewModels.Add(m.Id, ModelFactory(m));
    }

    public void Add(TManyModelView item)
    {
        if (viewModels.ContainsKey(item.ModelId) is false)
        {
            using (AppServices.GetDbContext(out var context))
            {
                var m = collection.RelatedQuery(context, item.ModelId);
                viewModels.Add(item.ModelId, ModelFactory(m));
            }
        }

        if (collection.Contains(item.ModelId) is false)
            collection.Add(item.ModelId);
    }

    public void Clear()
    {
        collection.Clear();
        viewModels.Clear();
    }

    public bool Contains(TManyModelView item)
    {
        Update();
        return viewModels.ContainsKey(item.ModelId);
    }

    public void CopyTo(TManyModelView[] array, int arrayIndex)
    {
        Update();
        viewModels.Values.CopyTo(array, arrayIndex);
    }

    public bool Remove(TManyModelView item)
    {
        viewModels.Remove(item.ModelId);
        return collection.Remove(item.ModelId);
    }

    public int Count => collection.Count;
    public bool IsReadOnly => false;

    public IEnumerator<TManyModelView> GetEnumerator()
    {
        Update();
        return viewModels.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        Update();
        return viewModels.Values.GetEnumerator();
    }
}
