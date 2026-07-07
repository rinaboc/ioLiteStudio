using System.Linq;
using System.Numerics;
using Core.Domain;
using Core.Features;
using NUnit.Framework;

public class SelectNodeTests : NodeBaseTest
{
    [Test]
    public void When_SelectNode_Then_NodeSelected()
    {
        var nodeConfig = new NodeConfig("Deformer");
        var deformerConfig = new DeformerNodeConfig(nodeConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));

        var node = _dummyContext.Nodes.GetAll().First();
        _dispatcher.Dispatch(new SelectNodeIntent(node.Id));

        Assert.AreEqual(node.Id, ((DefaultSession)_dummyContext.CurrentSession).SelectedNodeId);
    }

    [Test]
    public void When_SelectNodeAndUndo_Then_PreviousSelectionRestored()
    {
        var firstConfig = new NodeConfig("First");
        var secondConfig = new NodeConfig("Second");

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(firstConfig)));
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(secondConfig)));

        var firstNode = _dummyContext.Nodes.GetAll().First(n => n.Name == "First");
        var secondNode = _dummyContext.Nodes.GetAll().First(n => n.Name == "Second");

        _dispatcher.Dispatch(new SelectNodeIntent(firstNode.Id));
        _dispatcher.Dispatch(new SelectNodeIntent(secondNode.Id));
        _dispatcher.Dispatch(new UndoIntent());

        Assert.AreEqual(firstNode.Id, ((DefaultSession)_dummyContext.CurrentSession).SelectedNodeId);
    }

    [Test]
    public void When_UndoSelectAndRedo_Then_NodeSelected()
    {
        var firstConfig = new NodeConfig("First");
        var secondConfig = new NodeConfig("Second");

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(firstConfig)));
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(secondConfig)));

        var firstNode = _dummyContext.Nodes.GetAll().First(n => n.Name == "First");
        var secondNode = _dummyContext.Nodes.GetAll().First(n => n.Name == "Second");

        _dispatcher.Dispatch(new SelectNodeIntent(firstNode.Id));
        _dispatcher.Dispatch(new SelectNodeIntent(secondNode.Id));
        _dispatcher.Dispatch(new UndoIntent());
        _dispatcher.Dispatch(new RedoIntent());

        Assert.AreEqual(secondNode.Id, ((DefaultSession)_dummyContext.CurrentSession).SelectedNodeId);
    }

    [Test]
    public void When_SelectNull_Then_NodeDeselected()
    {
        var nodeConfig = new NodeConfig("Deformer");
        var deformerConfig = new DeformerNodeConfig(nodeConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));
        var node = _dummyContext.Nodes.GetAll().First();

        _dispatcher.Dispatch(new SelectNodeIntent(node.Id));
        _dispatcher.Dispatch(new SelectNodeIntent(null));

        Assert.IsNull(((DefaultSession)_dummyContext.CurrentSession).SelectedNodeId);
    }
}
