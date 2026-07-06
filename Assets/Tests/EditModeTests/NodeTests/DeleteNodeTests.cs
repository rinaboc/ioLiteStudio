using System.Linq;
using System.Numerics;
using Core.Domain;
using Core.Features;
using NUnit.Framework;

public class DeleteNodeTests : NodeBaseTest
{
    [Test]
    public void When_DeleteNode_Then_NodeRemoved()
    {
        var transform = new Transform2D { Position = Vector2.Zero, Rotation = Quaternion.Identity, Scale = Vector2.One };
        var nodeConfig = new NodeConfig("Deformer", transform);
        var deformerConfig = new DeformerNodeConfig(nodeConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));
        var node = _dummyContext.Nodes.GetAll().First();
        var nodeCount = _dummyContext.Nodes.Count();

        _dispatcher.Dispatch(new DeleteNodeIntent(node.Id));

        Assert.AreEqual(nodeCount - 1, _dummyContext.Nodes.Count());
        Assert.IsFalse(_dummyContext.Nodes.TryGet(node.Id, out _));
    }

    [Test]
    public void When_DeleteNodeAndUndo_Then_NodeRestored()
    {
        var transform = new Transform2D { Position = Vector2.Zero, Rotation = Quaternion.Identity, Scale = Vector2.One };
        var nodeConfig = new NodeConfig("Deformer", transform);
        var deformerConfig = new DeformerNodeConfig(nodeConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));
        var node = _dummyContext.Nodes.GetAll().First();
        var nodeCount = _dummyContext.Nodes.Count();

        _dispatcher.Dispatch(new DeleteNodeIntent(node.Id));
        _dispatcher.Dispatch(new UndoIntent());

        Assert.AreEqual(nodeCount, _dummyContext.Nodes.Count());
        Assert.IsTrue(_dummyContext.Nodes.TryGet(node.Id, out _));
    }

    [Test]
    public void When_UndoDeleteAndRedo_Then_NodeRemoved()
    {
        var transform = new Transform2D { Position = Vector2.Zero, Rotation = Quaternion.Identity, Scale = Vector2.One };
        var nodeConfig = new NodeConfig("Deformer", transform);
        var deformerConfig = new DeformerNodeConfig(nodeConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));
        var node = _dummyContext.Nodes.GetAll().First();
        var nodeCount = _dummyContext.Nodes.Count();

        _dispatcher.Dispatch(new DeleteNodeIntent(node.Id));
        _dispatcher.Dispatch(new UndoIntent());
        _dispatcher.Dispatch(new RedoIntent());

        Assert.AreEqual(nodeCount - 1, _dummyContext.Nodes.Count());
        Assert.IsFalse(_dummyContext.Nodes.TryGet(node.Id, out _));
    }

    [Test]
    public void When_DeleteSelectedNode_Then_SelectionCleared()
    {
        var transform = new Transform2D { Position = Vector2.Zero, Rotation = Quaternion.Identity, Scale = Vector2.One };
        var nodeConfig = new NodeConfig("Deformer", transform);
        var deformerConfig = new DeformerNodeConfig(nodeConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));
        var node = _dummyContext.Nodes.GetAll().First();
        _dispatcher.Dispatch(new SelectNodeIntent(node.Id));

        _dispatcher.Dispatch(new DeleteNodeIntent(node.Id));

        Assert.IsNull(((DefaultSession)_dummyContext.CurrentSession).SelectedNodeId);
        Assert.IsFalse(_dummyContext.Nodes.TryGet(node.Id, out _));
    }

    [Test]
    public void When_DeleteNodeWithChildren_Then_ChildrenReparentToParentParent()
    {
        var transform = new Transform2D { Position = Vector2.Zero, Rotation = Quaternion.Identity, Scale = Vector2.One };

        // create grandparent
        var grandConfig = new NodeConfig("Grandparent", transform);
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(grandConfig)));
        var grand = _dummyContext.Nodes.GetAll().First(n => n.Name == "Grandparent");

        // create parent under grandparent
        var session = _dummyContext.CurrentSession as DefaultSession;
        session.SelectedNodeId = grand.Id;
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(new NodeConfig("Parent", transform))));
        var parent = _dummyContext.Nodes.GetAll().First(n => n.Name == "Parent");

        // create children under parent
        session.SelectedNodeId = parent.Id;
        for (int i = 0; i < 3; i++)
        {
            _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(new NodeConfig("Child" + i, transform))));
        }

        var childrenBefore = _dummyContext.Nodes.GetAll().Where(n => n.Name.StartsWith("Child")).ToArray();
        Assert.AreEqual(3, childrenBefore.Length);
        foreach (var c in childrenBefore)
            Assert.AreEqual(parent.Id, c.ParentId);

        // delete parent and expect children to reparent to grandparent
        _dispatcher.Dispatch(new DeleteNodeIntent(parent.Id));

        var childrenAfter = _dummyContext.Nodes.GetAll().Where(n => n.Name.StartsWith("Child")).ToArray();
        Assert.AreEqual(3, childrenAfter.Length);
        foreach (var c in childrenAfter)
            Assert.AreEqual(grand.Id, c.ParentId);
    }
}

