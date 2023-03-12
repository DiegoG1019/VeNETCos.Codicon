using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.UI.ViewModels;

public interface IToManyRelationModelView<TRelatedModel, TMainModel>
    : IModelView<TMainModel>
    where TMainModel : IToManyRelation<TRelatedModel>, IID
    where TRelatedModel : IID
{
}

public interface IModelView<TModel>
    where TModel : IID
{
    public Guid ModelId { get; }
}

public interface IToOneRelationModelView<TMainModel, TRelated>
    : IModelView<TMainModel>
    where TMainModel : IToOneRelation<TRelated>, IID
    where TRelated : IID
{
}
