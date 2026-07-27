using Core.Domain;
using Core.Features;
using Core.Services;
using UI.Viewport;

public class ViewFactory : IViewFactory
{
    public void ClearViews()
    {
        var viewModelManager = ServiceLocator.Get<IViewModelManager>();

        if (!viewModelManager.TryGet<MeshNodeUIState>(out var viewModels))
            return;

        var nodeObjectManager = ServiceLocator.Get<INodeObjectManager>();

        foreach (var viewModel in viewModels)
        {
            nodeObjectManager.Destroy(viewModel.State.ID);
            viewModelManager.Destroy(viewModel);
        }
    }

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