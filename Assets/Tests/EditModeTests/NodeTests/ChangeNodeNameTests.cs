using System.Linq;
using Core.Domain;
using Core.Features;
using NUnit.Framework;

public class ChangeNodeNameTests : NodeBaseTest
{
    [Test]
    public void When_ChangeNodeName_Then_NodeNameUpdated()
    {
        var nodeConfig = new NodeConfig("OldName");
        var deformerConfig = new DeformerNodeConfig(nodeConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));
        var node = _dummyContext.Nodes.GetAll().First();
        var oldName = node.Name;

        _dispatcher.Dispatch(new ChangeNodeNameIntent(node.Id, "NewName"));

        Assert.AreEqual("NewName", node.Name);
        Assert.AreNotEqual(oldName, node.Name);
    }

    [Test]
    public void When_ChangeNodeNameAndUndo_Then_OriginalNameRestored()
    {
        var nodeConfig = new NodeConfig("OriginalName");
        var deformerConfig = new DeformerNodeConfig(nodeConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));
        var node = _dummyContext.Nodes.GetAll().First();

        _dispatcher.Dispatch(new ChangeNodeNameIntent(node.Id, "UpdatedName"));
        _dispatcher.Dispatch(new UndoIntent());

        Assert.AreEqual("OriginalName", node.Name);
    }

    [Test]
    public void When_UndoChangeNameAndRedo_Then_NameChanged()
    {
        var nodeConfig = new NodeConfig("InitialName");
        var deformerConfig = new DeformerNodeConfig(nodeConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));
        var node = _dummyContext.Nodes.GetAll().First();

        _dispatcher.Dispatch(new ChangeNodeNameIntent(node.Id, "ChangedName"));
        _dispatcher.Dispatch(new UndoIntent());
        _dispatcher.Dispatch(new RedoIntent());

        Assert.AreEqual("ChangedName", node.Name);
    }

    [Test]
    public void When_ChangeNodeNameToSameName_Then_NameUnchanged()
    {
        var nodeName = "SameName";
        var nodeConfig = new NodeConfig(nodeName);
        var deformerConfig = new DeformerNodeConfig(nodeConfig);

        _dispatcher.Dispatch(new CreateDeformerNodeIntent(deformerConfig));
        var node = _dummyContext.Nodes.GetAll().First();

        _dispatcher.Dispatch(new ChangeNodeNameIntent(node.Id, "SameName"));

        Assert.AreEqual(nodeName, node.Name);
    }
}