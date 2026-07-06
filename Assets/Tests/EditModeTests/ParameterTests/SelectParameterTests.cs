using System;
using System.Collections;
using System.Linq;
using Core.Domain;
using Core.Features;
using NUnit.Framework;
using UnityEngine.TestTools;

public class SelectParameterTests : ParameterBaseTest
{
    [Test]
    public void When_SelectParameter_Then_ParameterSelected()
    {
        var config = new ParameterConfig("Test", 2f, -1f, BlendMode.Absolute);
        _dispatcher.Dispatch(new CreateParameterIntent(config));

        var parameter = _dummyContext.Parameters.GetAll().First();
        _dispatcher.Dispatch(new SelectParameterIntent(parameter.Id));

        Assert.AreEqual(parameter.Id, ((DefaultSession)_dummyContext.CurrentSession).SelectedParamId);
    }

    [Test]
    public void When_SelectParameterAndUndo_Then_PreviousParameterSelected()
    {
        var configA = new ParameterConfig("TestA", 2f, -1f, BlendMode.Absolute);
        var configB = new ParameterConfig("TestB", 2f, -1f, BlendMode.Absolute);
        _dispatcher.Dispatch(new CreateParameterIntent(configA));
        _dispatcher.Dispatch(new CreateParameterIntent(configB));

        var first = _dummyContext.Parameters.GetAll().First(p => p.Name == "TestA");
        var second = _dummyContext.Parameters.GetAll().First(p => p.Name == "TestB");

        _dispatcher.Dispatch(new SelectParameterIntent(first.Id));
        _dispatcher.Dispatch(new SelectParameterIntent(second.Id));
        _dispatcher.Dispatch(new UndoIntent());

        Assert.AreEqual(first.Id, ((DefaultSession)_dummyContext.CurrentSession).SelectedParamId);
    }

    [Test]
    public void When_UndoSelectAndRedo_Then_ParameterSelected()
    {
        var configA = new ParameterConfig("TestA", 2f, -1f, BlendMode.Absolute);
        var configB = new ParameterConfig("TestB", 2f, -1f, BlendMode.Absolute);
        _dispatcher.Dispatch(new CreateParameterIntent(configA));
        _dispatcher.Dispatch(new CreateParameterIntent(configB));

        var first = _dummyContext.Parameters.GetAll().First(p => p.Name == "TestA");
        var second = _dummyContext.Parameters.GetAll().First(p => p.Name == "TestB");

        _dispatcher.Dispatch(new SelectParameterIntent(first.Id));
        _dispatcher.Dispatch(new SelectParameterIntent(second.Id));
        _dispatcher.Dispatch(new UndoIntent());
        _dispatcher.Dispatch(new RedoIntent());

        Assert.AreEqual(second.Id, ((DefaultSession)_dummyContext.CurrentSession).SelectedParamId);
    }
}
