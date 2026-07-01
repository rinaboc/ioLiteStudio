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

    [SetUp]
    public void Setup()
    {
        _dummyContext = new DummyContext();
        _dispatcher = new Dispatcher(_dummyContext);

        ServiceLocator.Register<IUndoService>(new UndoService(20));
        ServiceLocator.Register<IThreadPoolService>(new ThreadPoolService());

        _dispatcher.RegisterHandler(new UndoableIntentCommandHandler());
        _dispatcher.RegisterHandler(new NonUndoableIntentCommandHandler());

        _dispatcher.RegisterHandler(new AsyncUndoableIntentCommandHandler());
        _dispatcher.RegisterHandler(new AsyncNonUndoableIntentCommandHandler());
    }

    [Test]
    public void When_UndoableIntentDispatched_Then_SessionCountsChange()
    {
        ServiceLocator.Get<IUndoService>().Clear();
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
        ServiceLocator.Get<IUndoService>().Clear();
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
        ServiceLocator.Get<IUndoService>().Clear();
        var undoCount = _dummyContext.CurrentSession.UndoCount;
        var redoCount = _dummyContext.CurrentSession.RedoCount;

        _dispatcher.Dispatch(new NonUndoableIntent());

        Assert.AreEqual(undoCount, _dummyContext.CurrentSession.UndoCount);
        Assert.AreEqual(redoCount, _dummyContext.CurrentSession.RedoCount);
    }

    [Test]
    public async Task When_AsyncNonUndoableIntentDispatched_Then_SessionCountsAreUnchanged()
    {
        ServiceLocator.Get<IUndoService>().Clear();
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
        ServiceLocator.Get<IUndoService>().Clear();
        var undoCount = _dummyContext.CurrentSession.UndoCount;

        _dispatcher.Dispatch(new UndoableIntent());
        _dispatcher.Dispatch(new UndoIntent());

        Assert.AreEqual(undoCount, _dummyContext.CurrentSession.UndoCount);
        Assert.AreEqual(1, _dummyContext.CurrentSession.RedoCount);
    }
}
