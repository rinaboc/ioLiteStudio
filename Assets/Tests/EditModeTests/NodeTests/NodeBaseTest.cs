using Core.Domain;
using Core.Features;
using Core.Services;
using NUnit.Framework;
using UnityEngine;

public abstract class NodeBaseTest
{
    protected IAppContext _dummyContext;
    protected IDispatcher _dispatcher;

    protected GameObject _testRoot;
    protected INodeObjectManager _nodeObjectManager;

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
        ServiceLocator.Register<IViewModelManager>(new ViewModelManager(_dispatcher));

        handlerManager.Register(new CreateDeformerNodeCommandHandler());
        handlerManager.Register(new CreateMeshNodeCommandHandler());
        handlerManager.Register(new SelectNodeCommandHandler());
        handlerManager.Register(new DeleteNodeCommandHandler());
        handlerManager.Register(new ToggleVisibilityCommandHandler());
        handlerManager.Register(new ChangeNodeNameCommandHandler());
        handlerManager.Register(new ReparentNodeCommandHandler());
        handlerManager.Register(new UndoCommandHandler());
        handlerManager.Register(new RedoCommandHandler());
    }

    [TearDown]
    public virtual void TearDown()
    {
        ServiceLocator.UnRegister<INodeObjectManager>();
        Object.DestroyImmediate(_testRoot);
    }

    [SetUp]
    public void Setup()
    {
        ServiceLocator.Get<IUndoService>().Clear();
        _dummyContext.CurrentSession.UndoCount = 0;
        _dummyContext.CurrentSession.RedoCount = 0;

        _dummyContext.Nodes.Clear();

        _testRoot = new GameObject("TestRoot");
        _nodeObjectManager = _testRoot.AddComponent<NodeObjectManager>();
        _nodeObjectManager.Configure(new Material(Shader.Find("Custom/S_MeshNode")), _testRoot.transform);
        ServiceLocator.Register(_nodeObjectManager);
    }
}
