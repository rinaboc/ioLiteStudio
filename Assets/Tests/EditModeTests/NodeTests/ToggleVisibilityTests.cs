using System.Linq;
using System.Numerics;
using Core.Domain;
using Core.Features;
using NUnit.Framework;

public class ToggleVisibilityTests : NodeBaseTest
{
	[Test]
	public void When_ToggleVisibility_Then_NodeBecomesHidden()
	{
		var nodeConfig = new NodeConfig("Mesh");
		var meshConfig = new MeshNodeConfig(nodeConfig, "Assets/Textures/test.png", new MeshInfo());

		_dispatcher.Dispatch(new CreateMeshNodeIntent(meshConfig));

		var node = _dummyContext.Nodes.GetAll().First() as MeshNode;
		Assert.IsNotNull(node);
		Assert.IsTrue(node.Visibility);

		_dispatcher.Dispatch(new ToggleVisibilityIntent(node.Id));

		Assert.IsFalse(node.Visibility);
	}

	[Test]
	public void When_ToggleVisibilityTwice_Then_VisibilityRestored()
	{
		var nodeConfig = new NodeConfig("Mesh");
		var meshConfig = new MeshNodeConfig(nodeConfig, "Assets/Textures/test.png", new MeshInfo());

		_dispatcher.Dispatch(new CreateMeshNodeIntent(meshConfig));

		var node = _dummyContext.Nodes.GetAll().First() as MeshNode;
		Assert.IsNotNull(node);
		Assert.IsTrue(node.Visibility);

		_dispatcher.Dispatch(new ToggleVisibilityIntent(node.Id));
		_dispatcher.Dispatch(new ToggleVisibilityIntent(node.Id));

		Assert.IsTrue(node.Visibility);
	}

	[Test]
	public void When_ToggleChildVisibility_Then_OnlyChildAffected()
	{
		var parentConfig = new NodeConfig("Parent");
		var parentDeformerConfig = new DeformerNodeConfig(parentConfig);

		_dispatcher.Dispatch(new CreateDeformerNodeIntent(parentDeformerConfig));

		var parentNode = _dummyContext.Nodes.GetAll().First();
		var session = _dummyContext.CurrentSession as DefaultSession;
		session.SelectedNodeId = parentNode.Id;

		var childConfig = new NodeConfig("MeshChild");
		var childMeshConfig = new MeshNodeConfig(childConfig, "Assets/Textures/test.png", new MeshInfo());

		_dispatcher.Dispatch(new CreateMeshNodeIntent(childMeshConfig));

		var childNode = _dummyContext.Nodes.GetAll().First(n => n.Name == "MeshChild");

		Assert.IsTrue(parentNode.Visibility);
		Assert.IsTrue(childNode.Visibility);

		_dispatcher.Dispatch(new ToggleVisibilityIntent(childNode.Id));

		Assert.IsTrue(parentNode.Visibility);
		Assert.IsFalse(childNode.Visibility);
	}

	[Test]
	public void When_ToggleVisibility_Then_UndoRedoRestoresState()
	{
		var nodeConfig = new NodeConfig("Mesh");
		var meshConfig = new MeshNodeConfig(nodeConfig, "Assets/Textures/test.png", new MeshInfo());

		_dispatcher.Dispatch(new CreateMeshNodeIntent(meshConfig));

		var node = _dummyContext.Nodes.GetAll().First() as MeshNode;
		Assert.IsNotNull(node);
		Assert.IsTrue(node.Visibility);

		_dispatcher.Dispatch(new ToggleVisibilityIntent(node.Id));
		Assert.IsFalse(node.Visibility);

		_dispatcher.Dispatch(new UndoIntent());
		Assert.IsTrue(node.Visibility);

		_dispatcher.Dispatch(new RedoIntent());
		Assert.IsFalse(node.Visibility);
	}
}
