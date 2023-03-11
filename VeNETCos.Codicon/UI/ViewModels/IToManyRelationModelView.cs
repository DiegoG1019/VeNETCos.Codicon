using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.UI.ViewModels;

public interface IToManyRelationModelView<TRelatedModel, TMainModel> 
{
    public IToManyRelation<TRelatedModel> RelationModel { get; }
    public TMainModel Model { get; }
}
