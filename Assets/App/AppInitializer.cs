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
        RegisterServices();

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

        _dispatcher = new LogDispatcher(new Dispatcher(_context));
        RegisterHandlers();
    }

    void RegisterServices()
    {
        ServiceLocator.Register<ILogService>(new LogService());
        ServiceLocator.Register<IThreadPoolService>(new ThreadPoolService());
        ServiceLocator.Register<IUndoService>(new UndoService(40));
        ServiceLocator.Register<IViewModelManager>(new ViewModelManager(_dispatcher));
    }

    void RegisterHandlers()
    {
        _dispatcher.RegisterHandler(new UndoCommandHandler());
        _dispatcher.RegisterHandler(new RedoCommandHandler());
    }

    void Update()
    {
        _dispatcher.CheckResultQueue();
    }
}
