using Core.Domain;
using Core.Features;
using Core.Services;

public class ViewFactory : IViewFactory
{
    public void CreateMeshNodeView(MeshNode meshNode)
    {
        ServiceLocator.Get<INodeObjectManager>().Instantiate(meshNode, out var meshObject);

        IViewModel<MeshNodeUIState> viewModel = ServiceLocator.Get<IViewModelManager>().Create(
            meshNode.ToMeshNodeUIState(),
            (s, dispatcher) => new MeshNodeStore(s, dispatcher, meshNode.Id)
        );

        viewModel.Bind(new MeshNodeView(viewModel, meshObject));
    }

    public void DeleteMeshNodeView(int ID)
    {
        var viewModelManager = ServiceLocator.Get<IViewModelManager>();

        if (!viewModelManager.TryGet<MeshNodeUIState>(out var viewModels))
            return;

        foreach (var viewModel in viewModels)
        {
            if (viewModel.State.ID == ID)
            {
                viewModelManager.Destroy(viewModel);
                break;
            }
        }

        ServiceLocator.Get<INodeObjectManager>().Destroy(ID);
    }
}