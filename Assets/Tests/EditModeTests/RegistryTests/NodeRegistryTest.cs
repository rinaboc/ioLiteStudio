using Core.Data;
using Core.Domain;
using Core.Services;
using NUnit.Framework;

public class NodeRegistryTest
{
    private IAppContext _context;
    private IKeyManager _keyManager;

    [OneTimeSetUp]
    public void OneTimeSetup()
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

        ServiceLocator.Register<IKeyManager>(new KeyManager());
        _keyManager = ServiceLocator.Get<IKeyManager>();
    }

    [SetUp]
    public void Setup()
    {
        _context.Nodes.Clear();
        _keyManager.Reset();
    }

    [Test]
    public void When_NodeInserted_Then_StoredInRegistry()
    {
        var node = new MeshNode()
        {
            Id = _keyManager.Next()
        };

        _context.Nodes.Register(node.Id, node);
        _context.Nodes.TryGet(node.Id, out var storedNode);

        Assert.AreEqual(node, storedNode);
    }

    [Test]
    public void When_NodeRemoved_Then_NotInRegistry()
    {
        var node = new MeshNode()
        {
            Id = _keyManager.Next()
        };

        _context.Nodes.Register(node.Id, node);
        _context.Nodes.Remove(node.Id, out _);

        Assert.AreEqual(false, _context.Nodes.TryGet(node.Id, out _));
    }

    [Test]
    public void When_MoreNodesInserted_Then_ManyInRegistry()
    {
        int[] ids = new int[1000];
        for (int i = 0; i < 1000; i++)
        {
            var node = new MeshNode()
            {
                Id = _keyManager.Next()
            };

            ids[i] = node.Id;
            _context.Nodes.Register(node.Id, node);
        }

        Assert.AreEqual(1000, _context.Nodes.Count());
        Assert.AreEqual(1000, _context.Nodes.GetAll().Count);
        Assert.AreEqual(1000, _context.Nodes.GetValues(ids).Count);

        _context.Nodes.TryGet(ids[0], out var firstNode);
        _context.Nodes.TryGet(ids[1], out var secondNode);

        Assert.AreNotEqual(firstNode, secondNode);
    }
}
