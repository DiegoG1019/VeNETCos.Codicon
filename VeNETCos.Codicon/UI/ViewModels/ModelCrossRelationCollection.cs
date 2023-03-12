﻿using System;
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
    where TRelatedModel : IToManyRelation<TMainModel>, IID
    where TMainModel : IToManyRelation<TRelatedModel>, IID
{
    private readonly CrossRelationshipCollection<TRelatedModel, TMainModel> collection;
    private readonly Dictionary<Guid, TRelatedModelView> viewModels = new();
    private readonly Func<TRelatedModel, TRelatedModelView> ModelFactory;

    public ModelCrossRelationCollection(CrossRelationshipCollection<TRelatedModel, TMainModel> collection, Func<TRelatedModel, TRelatedModelView> modelFactory)
    {
        ArgumentNullException.ThrowIfNull(modelFactory);
        this.collection = collection ?? throw new ArgumentNullException(nameof(collection));

        ModelFactory = modelFactory;

        foreach (var m in collection)
            viewModels.Add(m.Id, modelFactory(m));
    }

    public void Add(TRelatedModelView item)
    {
        if(viewModels.ContainsKey(item.ModelId)) return;
        collection.Add(item.ModelId);
        viewModels.TryAdd(item.ModelId, item);
    }

    public void Clear()
    {
        collection.Clear();
        viewModels.Clear();
    }

    public bool Contains(TRelatedModelView item)
        => viewModels.ContainsKey(item.ModelId);

    public void CopyTo(TRelatedModelView[] array, int arrayIndex)
    {
        Update();
        viewModels.Values.CopyTo(array, arrayIndex);
    }

    public bool Remove(TRelatedModelView item)
    {
        Update();
        return collection.Remove(item.ModelId) && viewModels.Remove(item.ModelId, out _);
    }

    public int Count => viewModels.Count;
    public bool IsReadOnly => false;

    public IEnumerator<TRelatedModelView> GetEnumerator()
    {
        Update();
        return viewModels.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        Update();
        return viewModels.GetEnumerator();
    }

    private void Update()
    {
        foreach (var m in viewModels)
            if (collection.Contains(m.Key) is false)
                viewModels.Remove(m.Key);
        foreach (var m in collection)
            if (viewModels.ContainsKey(m.Id) is false)
                viewModels.Add(m.Id, ModelFactory(m));
    }
}
