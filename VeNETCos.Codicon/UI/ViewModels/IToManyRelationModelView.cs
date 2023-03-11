using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.UI.ViewModels;

public interface IToManyRelationModelView<TRelatedModel, TMainModel> 
{
    public IToManyRelation<TRelatedModel> RelationModel { get; }
    public TMainModel Model { get; }
}

public interface IToOneRelationModelView<TMainModel, TRelated>
{
    public IToOneRelation<TRelated> RelationModel { get; }
    public TMainModel Model { get; }
}
