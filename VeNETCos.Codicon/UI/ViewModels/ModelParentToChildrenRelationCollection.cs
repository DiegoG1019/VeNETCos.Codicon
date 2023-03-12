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

    public void Add(TManyModelView item)
    {
        if (viewModels.ContainsKey(item.Model) is false)
            viewModels.Add(item.Model, ModelFactory(item.Model));
        if (collection.Contains(item.Model) is false)
            collection.Add(item.Model);
    }

    public void Clear()
    {
        collection.Clear();
        viewModels.Clear();
    }

    public bool Contains(TManyModelView item)
    {
        Update();
        return viewModels.ContainsKey(item.Model);
    }

    public void CopyTo(TManyModelView[] array, int arrayIndex)
    {
        Update();
        viewModels.Values.CopyTo(array, arrayIndex);
    }

    public bool Remove(TManyModelView item)
    {
        viewModels.Remove(item.Model);
        return collection.Remove(item.Model);
    }

    public int Count => collection.Count;
    public bool IsReadOnly => false;

    public IEnumerator<TManyModelView> GetEnumerator()
        => viewModels.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => viewModels.Values.GetEnumerator();
}
