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
        _dispatcher = new Dispatcher(_dummyContext);

        ServiceLocator.Register<IUndoService>(new UndoService(20));
        ServiceLocator.Register<IThreadPoolService>(new ThreadPoolService());
        ServiceLocator.Register<ILogService>(new LogService());

        _dispatcher.RegisterHandler(new CreateParameterCommandHandler());
        _dispatcher.RegisterHandler(new DeleteParameterCommandHandler());
        _dispatcher.RegisterHandler(new SelectParameterCommandHandler());
        _dispatcher.RegisterHandler(new UndoCommandHandler());
        _dispatcher.RegisterHandler(new RedoCommandHandler());
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
