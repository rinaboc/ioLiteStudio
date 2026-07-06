using Core.Domain;
using Core.Features;
using Core.Services;
using NUnit.Framework;
using UnityEngine.TestTools;

public abstract class ParameterBaseTest
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

        handlerManager.Register(new CreateParameterCommandHandler());
        handlerManager.Register(new DeleteParameterCommandHandler());
        handlerManager.Register(new SelectParameterCommandHandler());
        handlerManager.Register(new EditParameterCommandHandler());
        handlerManager.Register(new UndoCommandHandler());
        handlerManager.Register(new RedoCommandHandler());
    }

    [SetUp]
    public void Setup()
    {
        ServiceLocator.Get<IUndoService>().Clear();
        _dummyContext.CurrentSession.UndoCount = 0;
        _dummyContext.CurrentSession.RedoCount = 0;

        _dummyContext.Parameters.Clear();
    }
}
