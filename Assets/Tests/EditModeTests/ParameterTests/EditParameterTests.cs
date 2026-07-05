using System;
using System.Collections;
using System.Linq;
using Core.Features;
using NUnit.Framework;
using UnityEngine.TestTools;

public class EditParameterTests : ParameterBaseTest
{
    [Test]
    public void When_EditParameter_Then_ParameterOverwritten()
    {
        var config = new ParameterConfig("Test", 2f, -1f, Core.Domain.BlendMode.Absolute);
        _dispatcher.Dispatch(new CreateParameterIntent(config));

        var parameter = _dummyContext.Parameters.GetAll().First();

        var editConfig = new ParameterConfig("TestEdited", 3f, -2f, Core.Domain.BlendMode.Additive);
        _dispatcher.Dispatch(new EditParameterIntent(parameter.Id, editConfig));

        var edited = _dummyContext.Parameters.GetAll().First(p => p.Id == parameter.Id);
        Assert.AreEqual(editConfig.Name, edited.Name);
        Assert.AreEqual(editConfig.Min, edited.MinValueFloat, 0.05f);
        Assert.AreEqual(editConfig.Max, edited.MaxValueFloat, 0.05f);
        Assert.AreEqual(editConfig.BlendMode, edited.BlendMode);
    }

    [Test]
    public void When_EditParameterAndUndo_Then_NoChanges()
    {
        var config = new ParameterConfig("Test", 2f, -1f, Core.Domain.BlendMode.Absolute);
        _dispatcher.Dispatch(new CreateParameterIntent(config));

        var parameter = _dummyContext.Parameters.GetAll().First();
        var originalName = parameter.Name;
        var originalMin = parameter.MinValueFloat;
        var originalMax = parameter.MaxValueFloat;
        var originalBlend = parameter.BlendMode;

        var editConfig = new ParameterConfig("TestEdited", 3f, -2f, Core.Domain.BlendMode.Additive);
        _dispatcher.Dispatch(new EditParameterIntent(parameter.Id, editConfig));
        _dispatcher.Dispatch(new UndoIntent());

        var afterUndo = _dummyContext.Parameters.GetAll().First(p => p.Id == parameter.Id);
        Assert.AreEqual(originalName, afterUndo.Name);
        Assert.AreEqual(originalMin, afterUndo.MinValueFloat, 0.05f);
        Assert.AreEqual(originalMax, afterUndo.MaxValueFloat, 0.05f);
        Assert.AreEqual(originalBlend, afterUndo.BlendMode);
    }

    [Test]
    public void When_UndoEditAndRedo_Then_ParameterOverwritten()
    {
        var config = new ParameterConfig("Test", 2f, -1f, Core.Domain.BlendMode.Absolute);
        _dispatcher.Dispatch(new CreateParameterIntent(config));

        var parameter = _dummyContext.Parameters.GetAll().First();

        var editConfig = new ParameterConfig("TestEdited", 3f, -2f, Core.Domain.BlendMode.Additive);
        _dispatcher.Dispatch(new EditParameterIntent(parameter.Id, editConfig));
        _dispatcher.Dispatch(new UndoIntent());
        _dispatcher.Dispatch(new RedoIntent());

        var afterRedo = _dummyContext.Parameters.GetAll().First(p => p.Id == parameter.Id);
        Assert.AreEqual(editConfig.Name, afterRedo.Name);
        Assert.AreEqual(editConfig.Min, afterRedo.MinValueFloat, 0.05f);
        Assert.AreEqual(editConfig.Max, afterRedo.MaxValueFloat, 0.05f);
        Assert.AreEqual(editConfig.BlendMode, afterRedo.BlendMode);
    }
}
