using System.Collections;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;

namespace VeNETCos.Codicon.UI.ViewModels;

public class ModelParentToChildrenRelationCollection<TOneModelView, TOneModel, TManyModelView, TManyModel> 
    : ICollection<TManyModel>
    where TOneModelView : class
    where TManyModelView : class
    where TOneModel : class, IID
    where TManyModel : class, IID, IToOneRelation<TOneModel>
{
    private readonly ParentToChildrenRelationshipCollection<TOneModel, TManyModel> collection;
    private readonly Dictionary<TManyModel, TManyModelView> viewModels = new();
    private readonly Func<TManyModel, TManyModelView> ModelFactory;

    public ModelParentToChildrenRelationCollection(
        ParentToChildrenRelationshipCollection<TOneModel, TManyModel> collection, 
        Func<TManyModel, TManyModelView> modelFactory
    )
    {
        ArgumentNullException.ThrowIfNull(modelFactory);
        this.collection = collection ?? throw new ArgumentNullException(nameof(collection));

        ModelFactory = modelFactory;

        foreach (var m in collection)
            viewModels.Add(m, modelFactory(m));
    }

    private void Update()
    {
        foreach (var m in viewModels)
            if (collection.Contains(m.Key) is false)
                viewModels.Remove(m.Key);
        foreach (var m in collection)
            if (viewModels.ContainsKey(m) is false)
                viewModels.Add(m, ModelFactory(m));
    }

    public void Add(TManyModel item)
    {
        if (viewModels.ContainsKey(item) is false)
            viewModels.Add(item, ModelFactory(item));
        if (collection.Contains(item) is false)
            collection.Add(item);
    }

    public void Clear()
    {
        collection.Clear();
        viewModels.Clear();
    }

    public bool Contains(TManyModel item)
    {
        Update();
        return viewModels.ContainsKey(item);
    }

    public void CopyTo(TManyModel[] array, int arrayIndex)
    {
        Update();
        viewModels.Keys.CopyTo(array, arrayIndex);
    }

    public bool Remove(TManyModel item)
    {
        viewModels.Remove(item);
        return collection.Remove(item);
    }

    public int Count => collection.Count;
    public bool IsReadOnly => false;

    public IEnumerator<TManyModel> GetEnumerator()
        => collection.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => collection.GetEnumerator();

    public IEnumerable<TManyModelView> ViewModels
    {
        get
        {
            Update();
            return viewModels.Values;
        }
    }
}
