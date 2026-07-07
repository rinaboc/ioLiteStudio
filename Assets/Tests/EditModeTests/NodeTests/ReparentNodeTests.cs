using System.Linq;
using Core.Domain;
using Core.Features;
using NUnit.Framework;

public class ReparentNodeTests : NodeBaseTest
{
    [Test]
    public void When_ReparentNode_Then_ParentIdUpdated()
    {
        // Create parent
        var parentConfig = new NodeConfig("Parent");
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(parentConfig)));
        var parent = _dummyContext.Nodes.GetAll().First(n => n.Name == "Parent");

        // Create child with no parent
        var childConfig = new NodeConfig("Child");
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(childConfig)));
        var child = _dummyContext.Nodes.GetAll().First(n => n.Name == "Child");

        _dispatcher.Dispatch(new ReparentNodeIntent(child.Id, parent.Id));

        Assert.AreEqual(parent.Id, child.ParentId);
    }

    [Test]
    public void When_ReparentNodeAndUndo_Then_ParentIdRestored()
    {
        // Create nodes
        var firstParentConfig = new NodeConfig("FirstParent");
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(firstParentConfig)));
        var firstParent = _dummyContext.Nodes.GetAll().First(n => n.Name == "FirstParent");

        var secondParentConfig = new NodeConfig("SecondParent");
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(secondParentConfig)));
        var secondParent = _dummyContext.Nodes.GetAll().First(n => n.Name == "SecondParent");

        var childConfig = new NodeConfig("Child");
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(childConfig)));
        var child = _dummyContext.Nodes.GetAll().First(n => n.Name == "Child");

        // Reparent child to firstParent
        _dispatcher.Dispatch(new ReparentNodeIntent(child.Id, firstParent.Id));
        var originalParentId = child.ParentId;

        // Reparent to secondParent
        _dispatcher.Dispatch(new ReparentNodeIntent(child.Id, secondParent.Id));
        _dispatcher.Dispatch(new UndoIntent());

        Assert.AreEqual(originalParentId, child.ParentId);
    }

    [Test]
    public void When_UndoReparentAndRedo_Then_NewParentApplied()
    {
        // Create parents
        var firstParentConfig = new NodeConfig("FirstParent");
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(firstParentConfig)));
        var firstParent = _dummyContext.Nodes.GetAll().First(n => n.Name == "FirstParent");

        var secondParentConfig = new NodeConfig("SecondParent");
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(secondParentConfig)));
        var secondParent = _dummyContext.Nodes.GetAll().First(n => n.Name == "SecondParent");

        var childConfig = new NodeConfig("Child");
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(childConfig)));
        var child = _dummyContext.Nodes.GetAll().First(n => n.Name == "Child");

        _dispatcher.Dispatch(new ReparentNodeIntent(child.Id, firstParent.Id));
        _dispatcher.Dispatch(new ReparentNodeIntent(child.Id, secondParent.Id));
        _dispatcher.Dispatch(new UndoIntent());
        _dispatcher.Dispatch(new RedoIntent());

        Assert.AreEqual(secondParent.Id, child.ParentId);
    }

    [Test]
    public void When_ReparentNodeToNull_Then_NodeBecomesRoot()
    {
        // Create parent
        var parentConfig = new NodeConfig("Parent");
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(parentConfig)));
        var parent = _dummyContext.Nodes.GetAll().First(n => n.Name == "Parent");

        // Create child under parent
        var session = _dummyContext.CurrentSession as DefaultSession;
        session.SelectedNodeId = parent.Id;
        var childConfig = new NodeConfig("Child");
        _dispatcher.Dispatch(new CreateDeformerNodeIntent(new DeformerNodeConfig(childConfig)));
        var child = _dummyContext.Nodes.GetAll().First(n => n.Name == "Child");

        Assert.IsNotNull(child.ParentId);

        // Reparent to null (make it root)
        _dispatcher.Dispatch(new ReparentNodeIntent(child.Id, null)); // 0 or null for root

        Assert.IsNull(child.ParentId);
    }
}