using System.Linq;
using System.Numerics;
using Core.Domain;
using Core.Features;
using NUnit.Framework;

public class CreateDeformerNodeTests : NodeBaseTest
{
    [Test]
    public void When_CreateDeformerNode_Then_NodeStored()
    {
        var nodeCount = _dummyContext.Nodes.Count();

        var nodeConfig = new NodeConfig("Deformer");
        var deformerConfig = new DeformerNodeConfig(nodeConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));

        Assert.AreEqual(nodeCount + 1, _dummyContext.Nodes.Count());

        var node = _dummyContext.Nodes.GetAll().ToArray()[0] as DeformerNode;
        Assert.IsNotNull(node);
        Assert.AreEqual("Deformer", node.Name);
        Assert.IsNull(node.ParentId);
    }

    [Test]
    public void When_ManyDeformerNodesCreated_Then_ManyStored()
    {
        var nodeCount = _dummyContext.Nodes.Count();

        for (int i = 0; i < 100; i++)
        {
            var nodeConfig = new NodeConfig("Deformer" + i);
            var deformerConfig = new DeformerNodeConfig(nodeConfig);

            _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));
        }

        Assert.AreEqual(nodeCount + 100, _dummyContext.Nodes.Count());
    }

    [Test]
    public void When_CreateDeformerNodeAndUndo_Then_NoNodeStored()
    {
        var nodeCount = _dummyContext.Nodes.Count();

        var nodeConfig = new NodeConfig("Deformer");
        var deformerConfig = new DeformerNodeConfig(nodeConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));
        _dispatcher.Dispatch(new UndoIntent());

        Assert.AreEqual(nodeCount, _dummyContext.Nodes.Count());
    }

    [Test]
    public void When_UndoCreationAndRedo_Then_NodeStored()
    {
        var nodeCount = _dummyContext.Nodes.Count();

        var nodeConfig = new NodeConfig("Deformer");
        var deformerConfig = new DeformerNodeConfig(nodeConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));
        _dispatcher.Dispatch(new UndoIntent());
        _dispatcher.Dispatch(new RedoIntent());

        Assert.AreEqual(nodeCount + 1, _dummyContext.Nodes.Count());
    }

    [Test]
    public void When_NodeIsSelectedAndCreateNode_Then_CreatedNodeIsChildOfSelected()
    {
        var parentConfig = new NodeConfig("Parent");
        var parentDeformerConfig = new DeformerNodeConfig(parentConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(parentDeformerConfig));

        var parentNode = _dummyContext.Nodes.GetAll().ToArray()[0];
        var session = _dummyContext.CurrentSession as DefaultSession;
        session.SelectedNodeId = parentNode.Id;

        var childConfig = new NodeConfig("Child");
        var childDeformerConfig = new DeformerNodeConfig(childConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(childDeformerConfig));

        var childNode = _dummyContext.Nodes.GetAll().ToArray()[1];
        Assert.AreEqual(parentNode.Id, childNode.ParentId);
    }
}
