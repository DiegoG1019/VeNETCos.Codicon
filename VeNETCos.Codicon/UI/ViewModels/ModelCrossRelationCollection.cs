using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;

namespace VeNETCos.Codicon.UI.ViewModels;

public class ModelCrossRelationCollection<TRelatedModelView, TRelatedModel, TMainModelView, TMainModel> : ICollection<TRelatedModelView>
    where TRelatedModelView : IToManyRelationModelView<TMainModel, TRelatedModel>
    where TMainModelView : IToManyRelationModelView<TRelatedModel, TMainModel>
    where TRelatedModel : IToManyRelation<TMainModel>
    where TMainModel : IToManyRelation<TRelatedModel>
{
    private readonly CrossRelationshipCollection<TRelatedModel, TMainModel> collection;
    private readonly Dictionary<TRelatedModel, TRelatedModelView> viewModels = new();
    private readonly Func<TRelatedModel, TRelatedModelView> ModelFactory;

    public ModelCrossRelationCollection(CrossRelationshipCollection<TRelatedModel, TMainModel> collection, Func<TRelatedModel, TRelatedModelView> modelFactory)
    {
        ArgumentNullException.ThrowIfNull(modelFactory);
        this.collection = collection ?? throw new ArgumentNullException(nameof(collection));

        ModelFactory = modelFactory;

        foreach (var m in collection)
            viewModels.Add(m, modelFactory(m));
    }

    public void Add(TRelatedModelView item)
    {
        if(viewModels.ContainsKey(item.Model)) return;
        collection.Add(item.Model);
        viewModels.TryAdd(item.Model, item);
    }

    public void Clear()
    {
        collection.Clear();
        viewModels.Clear();
    }

    public bool Contains(TRelatedModelView item)
        => viewModels.ContainsKey(item.Model);

    public void CopyTo(TRelatedModelView[] array, int arrayIndex)
        => viewModels.CopyTo(array, arrayIndex);

    public bool Remove(TRelatedModelView item) 
        => collection.Remove(item.Model) && viewModels.Remove(item);

    public int Count => viewModels.Count;
    public bool IsReadOnly => false;

    public IEnumerator<TRelatedModelView> GetEnumerator()
        => viewModels.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => viewModels.GetEnumerator();

    private void Update()
    {
        foreach (var m in collection)
            if (viewModels.ContainsKey(m) is false)
                viewModels.Add(m, ModelFactory(m));
    }
}
