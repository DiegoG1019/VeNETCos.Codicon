//using System.Collections;
//using VeNETCos.Codicon.Database.Models;
//using VeNETCos.Codicon.Types;

//namespace VeNETCos.Codicon.UI.ViewModels;

//public class ModelSingleRelationCollection<TRelatedModelView, TRelatedModel, TMainModelView, TMainModel> : ICollection<TRelatedModelView>
//    where TMainModelView : IToManyRelationModelView<TRelatedModel, TMainModel>
//    where TRelatedModelView : IModelView<TRelatedModel>
//    where TRelatedModel : IID
//    where TMainModel : IToManyRelation<TRelatedModel>, IID
//{
//    private readonly Dictionary<Guid, TRelatedModelView> viewModels = new();
//    private readonly Func<TRelatedModel, TRelatedModelView> ModelFactory;
//    private readonly Guid MainModelId;

//    public ModelSingleRelationCollection(Guid modelId, Func<TRelatedModel, TRelatedModelView> modelFactory)
//    {
//        ArgumentNullException.ThrowIfNull(modelFactory);

//        ModelFactory = modelFactory;
//        MainModelId = modelId;

//        using (AppServices.GetDbContext(out var context))
//        {
//            var main = context
//            foreach (var m in model.Relation)
//                viewModels.Add(m, modelFactory(m));
//        }
//    }
//    public void Add(TRelatedModelView item)
//    {
//        if (viewModels.ContainsKey(item.ModelId) is false)
//            viewModels.Add(item.ModelId, item);
//        if (MainModel.Relation.Contains(item.ModelId) is false)
//            MainModel.Relation.Add(item.Model);
//    }

//    public void Clear()
//    {
//        viewModels.Clear();
//        MainModel.Relation.Clear();
//    }

//    public bool Contains(TRelatedModelView item)
//    {
//        Update();
//        return viewModels.ContainsKey(item.Model);
//    }

//    public void CopyTo(TRelatedModelView[] array, int arrayIndex)
//    {
//        Update();
//        viewModels.Values.CopyTo(array, arrayIndex);
//    }

//    public bool Remove(TRelatedModelView item)
//    {
//        viewModels.Remove(item.Model);
//        return MainModel.Relation.Remove(item.Model);
//    }

//    public int Count => MainModel.Relation.Count;
//    public bool IsReadOnly => false;

//    public IEnumerator<TRelatedModelView> GetEnumerator()
//    {
//        Update();
//        return viewModels.Values.GetEnumerator();
//    }

//    IEnumerator IEnumerable.GetEnumerator()
//    {
//        Update();
//        return viewModels.Values.GetEnumerator();
//    }

//    private void Update()
//    {
//        foreach (var m in viewModels)
//            if (MainModel.Relation.Contains(m.Key) is false)
//                viewModels.Remove(m.Key);
//        foreach (var m in MainModel.Relation)
//            if (viewModels.ContainsKey(m) is false)
//                viewModels.Add(m, ModelFactory(m));
//    }
//}
