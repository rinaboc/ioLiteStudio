using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Core.Domain;
using Core.Features;
using NUnit.Framework;

public class CreateMeshNodeTests : NodeBaseTest
{
    [Test]
    public void When_CreateMeshNode_Then_NodeStored()
    {
        var nodeCount = _dummyContext.Nodes.Count();

        var nodeConfig = new NodeConfig("Mesh", DefaultTransform: Transform2D.Default);
        var meshConfig = new MeshNodeConfig(nodeConfig, "Assets/Textures/test.png", new MeshInfo());

        _dispatcher.Dispatch(new CreateMeshNodeIntent(meshConfig));

        Assert.AreEqual(nodeCount + 1, _dummyContext.Nodes.Count());

        var node = _dummyContext.Nodes.GetAll().ToArray()[0] as MeshNode;
        Assert.IsNotNull(node);
        Assert.AreEqual("Mesh", node.Name);
        Assert.IsNull(node.ParentId);
    }

    [Test]
    public void When_ManyMeshNodesCreated_Then_ManyStored()
    {
        var nodeCount = _dummyContext.Nodes.Count();

        for (int i = 0; i < 5; i++)
        {
            var nodeConfig = new NodeConfig("Mesh" + i, DefaultTransform: Transform2D.Default);
            var meshConfig = new MeshNodeConfig(nodeConfig, "Assets/Textures/test.png", new MeshInfo(), i);

            _dispatcher.Dispatch(new CreateMeshNodeIntent(meshConfig));
        }

        Assert.AreEqual(nodeCount + 5, _dummyContext.Nodes.Count());
    }

    [Test]
    public void When_CreateMeshNodeAndUndo_Then_NoNodeStored()
    {
        var nodeCount = _dummyContext.Nodes.Count();

        var nodeConfig = new NodeConfig("Mesh", DefaultTransform: Transform2D.Default);
        var meshConfig = new MeshNodeConfig(nodeConfig, "Assets/Textures/test.png", new MeshInfo());

        _dispatcher.Dispatch(new CreateMeshNodeIntent(meshConfig));
        _dispatcher.Dispatch(new UndoIntent());

        Assert.AreEqual(nodeCount, _dummyContext.Nodes.Count());
    }

    [Test]
    public void When_UndoCreationAndRedo_Then_NodeStored()
    {
        var nodeCount = _dummyContext.Nodes.Count();

        var nodeConfig = new NodeConfig("Mesh", DefaultTransform: Transform2D.Default);
        var meshConfig = new MeshNodeConfig(nodeConfig, "Assets/Textures/test.png", new MeshInfo());

        _dispatcher.Dispatch(new CreateMeshNodeIntent(meshConfig));
        _dispatcher.Dispatch(new UndoIntent());
        _dispatcher.Dispatch(new RedoIntent());

        Assert.AreEqual(nodeCount + 1, _dummyContext.Nodes.Count());
    }

    [Test]
    public void When_NodeIsSelectedAndCreateMeshNode_Then_CreatedNodeIsChildOfSelected()
    {
        var parentConfig = new NodeConfig("Parent", DefaultTransform: Transform2D.Default);
        var parentDeformerConfig = new DeformerNodeConfig(parentConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(parentDeformerConfig));

        var parentNode = _dummyContext.Nodes.GetAll().First();
        var session = _dummyContext.CurrentSession as DefaultSession;
        session.SelectedNodeId = parentNode.Id;

        var childConfig = new NodeConfig("MeshChild", DefaultTransform: Transform2D.Default);
        var childMeshConfig = new MeshNodeConfig(childConfig, "Assets/Textures/test.png", new MeshInfo());

        _dispatcher.Dispatch(new CreateMeshNodeIntent(childMeshConfig));

        var childNode = _dummyContext.Nodes.GetAll().First(n => n.Name == "MeshChild");
        Assert.AreEqual(parentNode.Id, childNode.ParentId);
    }
}

