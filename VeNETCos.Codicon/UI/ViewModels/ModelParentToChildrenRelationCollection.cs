using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;

namespace VeNETCos.Codicon.UI.ViewModels;

public class ModelParentToChildrenRelationCollection<TOneModelView, TOneModel, TManyModelView, TManyModel> 
    : ICollection<TManyModelView>, INotifyCollectionChanged
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
        bool changed = false;
        foreach (var m in viewModels)
            if (collection.Contains(m.Key) is false)
            {
                viewModels.Remove(m.Key);
                changed = true;
            }
        foreach (var m in collection)
            if (viewModels.ContainsKey(m.Id) is false)
            {
                viewModels.Add(m.Id, ModelFactory(m));
                changed = true;
            }

        if (changed)
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public void Add(TManyModelView item)
    {
        if (viewModels.ContainsKey(item.ModelId) is false)
        {
            using (AppServices.GetDbContext(out var context))
            {
                var m = collection.RelatedQuery(context, item.ModelId);
                viewModels.Add(item.ModelId, ModelFactory(m));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        if (collection.Contains(item.ModelId) is false)
            collection.Add(item.ModelId);
    }

    public void Clear()
    {
        collection.Clear();
        viewModels.Clear();
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
}
