using Core.Domain;
using Core.Data;
using UnityEngine;
using Core.Services;
using Core.Features;
using App;

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
        gameObject.GetComponentInChildren<UIManager>().InitializeViews();
        gameObject.GetComponentInChildren<InputBridge>().InitializeInputs();
    }

    private void CreateViewModels()
    {
        var manager = ServiceLocator.Get<IViewModelManager>();

        var paramState = new ParameterUIState();
        manager.Create(
            paramState,
            (s, dispatcher) => new ParameterStore(s, dispatcher)
        );

        manager.Create(
            new TopBarUIState(),
            (s, dispatcher) => new TopBarStore(s, dispatcher)
        );

        manager.Create(
            new WindowUIState(false, WindowType.File),
            (s, dispatcher) => new WindowStore(s, dispatcher)
        );

        manager.Create(
            new NodeInspectorUIState(new NodeInspectorElement[] { }, null),
            (s, dispatcher) => new NodeInspectorStore(s, dispatcher)
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
        ServiceLocator.Register<IViewFactory>(new ViewFactory());
    }

    void RegisterHandlers(IHandlerManager handlerManager)
    {
        #region System Handlers
        handlerManager.Register(new UndoCommandHandler());
        handlerManager.Register(new RedoCommandHandler());
        handlerManager.Register(new OpenWindowCommandHandler());
        handlerManager.Register(new CloseWindowCommandHandler());
        handlerManager.Register(new WidgetToggleCommandHandler());
        #endregion

        #region Node Handlers
        handlerManager.Register(new CreateMeshNodeCommandHandler());
        handlerManager.Register(new CreateDeformerNodeCommandHandler());
        handlerManager.Register(new SelectNodeCommandHandler());
        handlerManager.Register(new ChangeNodeNameCommandHandler());
        handlerManager.Register(new DeleteNodeCommandHandler());
        handlerManager.Register(new ToggleVisibilityCommandHandler());
        #endregion
    }

    void Update()
    {
        _dispatcher.CheckResultQueue();
    }
}
