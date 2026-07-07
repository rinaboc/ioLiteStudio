using Core.Domain;
using Core.Features;
using Core.Services;
using NUnit.Framework;

public abstract class NodeBaseTest
{
    protected IAppContext _dummyContext;
    protected IDispatcher _dispatcher;

    [OneTimeSetUp]
    public void OnetimeSetup()
    {
        _dummyContext = new DummyContext();
        IHandlerManager handlerManager = new HandlerManager();
        _dispatcher = new Dispatcher(_dummyContext, handlerManager);

        ServiceLocator.Register<IUndoService>(new UndoService(20));
        ServiceLocator.Register<IThreadPoolService>(new ThreadPoolService());
        ServiceLocator.Register<ILogService>(new LogService());
        ServiceLocator.Register<IKeyManager>(new KeyManager());
        ServiceLocator.Register<IImageService>(new ImageService());

        handlerManager.Register(new CreateDeformerNodeCommandHandler());
        handlerManager.Register(new CreateMeshNodeCommandHandler());
        handlerManager.Register(new SelectNodeCommandHandler());
        handlerManager.Register(new DeleteNodeCommandHandler());
        handlerManager.Register(new UndoCommandHandler());
        handlerManager.Register(new RedoCommandHandler());
    }

    [SetUp]
    public void Setup()
    {
        ServiceLocator.Get<IUndoService>().Clear();
        _dummyContext.CurrentSession.UndoCount = 0;
        _dummyContext.CurrentSession.RedoCount = 0;

        _dummyContext.Nodes.Clear();
    }
}
