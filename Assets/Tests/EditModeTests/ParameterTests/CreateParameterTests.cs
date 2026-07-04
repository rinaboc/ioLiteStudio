using System;
using System.Collections;
using System.Linq;
using Core.Features;
using NUnit.Framework;
using UnityEngine.TestTools;

public class CreateParameterTests : ParameterBaseTest
{
    [Test]
    public void When_CreateParameter_Then_ParameterStored()
    {
        var paramCount = _dummyContext.Parameters.Count();

        var config = new ParameterConfig("Test", 2f, -1f, Core.Domain.BlendMode.Absolute);
        _dispatcher.Dispatch(new CreateParameterIntent(config));

        Assert.AreEqual(paramCount + 1, _dummyContext.Parameters.Count());

        var parameter = _dummyContext.Parameters.GetAll().ToArray()[0];
        Assert.AreEqual(config.Name, parameter.Name);
        Assert.AreEqual(config.Min, parameter.MinValueFloat, 0.05f);
        Assert.AreEqual(config.Max, parameter.MaxValueFloat, 0.05f);
        Assert.AreEqual(config.BlendMode, parameter.BlendMode);
    }

    [Test]
    public void When_ManyParametersCreated_Then_ManyStored()
    {
        var paramCount = _dummyContext.Parameters.Count();

        for (int i = 0; i < 2000; i++)
        {
            var config = new ParameterConfig("Test" + i, 2f, -1f, Core.Domain.BlendMode.Absolute);
            _dispatcher.Dispatch(new CreateParameterIntent(config));
        }

        Assert.AreEqual(paramCount + 2000, _dummyContext.Parameters.Count());
    }

    [Test]
    public void When_CreateParameterAndUndo_Then_NoParameterStored()
    {
        var paramCount = _dummyContext.Parameters.Count();

        var config = new ParameterConfig("Test", 2f, -1f, Core.Domain.BlendMode.Absolute);
        _dispatcher.Dispatch(new CreateParameterIntent(config));
        _dispatcher.Dispatch(new UndoIntent());

        Assert.AreEqual(paramCount, _dummyContext.Parameters.Count());
    }

    [Test]
    public void When_UndoCreationAndRedo_Then_ParameterStored()
    {
        var paramCount = _dummyContext.Parameters.Count();

        var config = new ParameterConfig("Test", 2f, -1f, Core.Domain.BlendMode.Absolute);
        _dispatcher.Dispatch(new CreateParameterIntent(config));
        _dispatcher.Dispatch(new UndoIntent());
        _dispatcher.Dispatch(new RedoIntent());

        Assert.AreEqual(paramCount + 1, _dummyContext.Parameters.Count());
    }
}
