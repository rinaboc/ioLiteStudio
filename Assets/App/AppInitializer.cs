using Core.Domain;
using Core.Data;
using UnityEngine;
using Core.Services;
using Core.Features;

public class AppInitializer : MonoBehaviour
{
    private IDispatcher _dispatcher;
    private IAppContext _context;

    void Awake()
    {
        _context = new AppContext(
            new NodeRegistry(),
            new ParameterRegistry(),
            new ParameterKeyRegistry(),
            new AnimationClipRegistry(),
            new ParameterTimelineTrackRegistry(),
            new TimelineKeyframeRegistry(),
            new ProjectSettings(),
            new DefaultSession()
        );

        IHandlerManager handlerManager = new HandlerManager();

        _dispatcher = new LogDispatcher(new Dispatcher(_context, handlerManager));

        RegisterServices();
        RegisterHandlers(handlerManager);
        CreateViewModels();
    }

    private void CreateViewModels()
    {
        var manager = ServiceLocator.Get<IViewModelManager>();

        var paramState = new ParameterUIState();
        manager.Create(
            paramState,
            (s, dispatcher) => new ParameterStore(s, dispatcher),
            (s, dispatcher) => new ViewModel<ParameterUIState>(s, dispatcher)
        );
    }

    void RegisterServices()
    {
        ServiceLocator.Register<ILogService>(new LogService());
        ServiceLocator.Register<IThreadPoolService>(new ThreadPoolService());
        ServiceLocator.Register<IUndoService>(new UndoService(40));
        ServiceLocator.Register<IViewModelManager>(new ViewModelManager(_dispatcher));
        ServiceLocator.Register<IKeyManager>(new KeyManager());
        ServiceLocator.Register<INodeObjectManager>(gameObject.GetComponentInChildren<NodeObjectManager>());
        ServiceLocator.Register<IImageService>(new ImageService());
    }

    void RegisterHandlers(IHandlerManager handlerManager)
    {
        handlerManager.Register(new UndoCommandHandler());
        handlerManager.Register(new RedoCommandHandler());
        handlerManager.Register(new CreateMeshNodeCommandHandler());
    }

    void Update()
    {
        _dispatcher.CheckResultQueue();
    }
}
