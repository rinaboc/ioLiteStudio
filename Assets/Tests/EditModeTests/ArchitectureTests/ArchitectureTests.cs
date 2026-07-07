using System.Threading.Tasks;
using Core.Domain;
using Core.Features;
using Core.Services;
using NUnit.Framework;
using UnityEngine.TestTools;

public class ArchitectureTests
{
    private IAppContext _dummyContext;
    private IDispatcher _dispatcher;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _dummyContext = new DummyContext();
        IHandlerManager handlerManager = new HandlerManager();
        _dispatcher = new Dispatcher(_dummyContext, handlerManager);

        ServiceLocator.Register<IUndoService>(new UndoService(20));
        ServiceLocator.Register<IThreadPoolService>(new ThreadPoolService());

        handlerManager.Register(new UndoableIntentCommandHandler());
        handlerManager.Register(new NonUndoableIntentCommandHandler());

        handlerManager.Register(new AsyncUndoableIntentCommandHandler());
        handlerManager.Register(new AsyncNonUndoableIntentCommandHandler());

        handlerManager.Register(new UndoCommandHandler());
        handlerManager.Register(new RedoCommandHandler());
    }

    [SetUp]
    public void Setup()
    {
        ServiceLocator.Get<IUndoService>().Clear();
        _dummyContext.CurrentSession.UndoCount = 0;
        _dummyContext.CurrentSession.RedoCount = 0;
    }

    [Test]
    public void When_UndoableIntentDispatched_Then_SessionCountsChange()
    {
        var undoCount = _dummyContext.CurrentSession.UndoCount;

        _dispatcher.Dispatch(new UndoableIntent());

        Assert.AreEqual(undoCount + 1, _dummyContext.CurrentSession.UndoCount);
        Assert.AreEqual(0, _dummyContext.CurrentSession.RedoCount);

        _dispatcher.Dispatch(new UndoableIntent());

        Assert.AreEqual(undoCount + 2, _dummyContext.CurrentSession.UndoCount);
        Assert.AreEqual(0, _dummyContext.CurrentSession.RedoCount);
    }

    [Test]
    public async Task When_AsyncUndoableIntentDispatched_Then_SessionCountsChange()
    {
        var undoCount = _dummyContext.CurrentSession.UndoCount;

        _dispatcher.Dispatch(new AsyncUndoableIntent());

        // simulate execution time
        await Task.Delay(200);
        _dispatcher.CheckResultQueue();

        Assert.AreEqual(undoCount + 1, _dummyContext.CurrentSession.UndoCount);
        Assert.AreEqual(0, _dummyContext.CurrentSession.RedoCount);

        _dispatcher.Dispatch(new AsyncUndoableIntent());

        // simulate execution time
        await Task.Delay(200);
        _dispatcher.CheckResultQueue();

        Assert.AreEqual(undoCount + 2, _dummyContext.CurrentSession.UndoCount);
        Assert.AreEqual(0, _dummyContext.CurrentSession.RedoCount);
    }

    [Test]
    public void When_NonUndoableIntentDispatched_Then_SessionCountsAreUnchanged()
    {
        var undoCount = _dummyContext.CurrentSession.UndoCount;
        var redoCount = _dummyContext.CurrentSession.RedoCount;

        _dispatcher.Dispatch(new NonUndoableIntent());

        Assert.AreEqual(undoCount, _dummyContext.CurrentSession.UndoCount);
        Assert.AreEqual(redoCount, _dummyContext.CurrentSession.RedoCount);
    }

    [Test]
    public async Task When_AsyncNonUndoableIntentDispatched_Then_SessionCountsAreUnchanged()
    {
        var undoCount = _dummyContext.CurrentSession.UndoCount;
        var redoCount = _dummyContext.CurrentSession.RedoCount;

        _dispatcher.Dispatch(new AsyncNonUndoableIntent());

        // simulate execution time
        await Task.Delay(200);
        _dispatcher.CheckResultQueue();

        Assert.AreEqual(undoCount, _dummyContext.CurrentSession.UndoCount);
        Assert.AreEqual(redoCount, _dummyContext.CurrentSession.RedoCount);
    }

    [Test]
    public void When_UndoableIntentDispatched_Then_DoUndo_Then_SessionCountsChanged()
    {
        var undoCount = _dummyContext.CurrentSession.UndoCount;

        _dispatcher.Dispatch(new UndoableIntent());
        _dispatcher.Dispatch(new UndoIntent());

        Assert.AreEqual(undoCount, _dummyContext.CurrentSession.UndoCount);
        Assert.AreEqual(1, _dummyContext.CurrentSession.RedoCount);
    }

    [Test]
    public void When_UndoableIntentDispatched_Then_DoUndoRedo_Then_SessionCountsUnchanged()
    {
        var undoCount = _dummyContext.CurrentSession.UndoCount;

        _dispatcher.Dispatch(new UndoableIntent());
        _dispatcher.Dispatch(new UndoIntent());
        _dispatcher.Dispatch(new RedoIntent());

        Assert.AreEqual(undoCount + 1, _dummyContext.CurrentSession.UndoCount);
        Assert.AreEqual(0, _dummyContext.CurrentSession.RedoCount);
    }
}
