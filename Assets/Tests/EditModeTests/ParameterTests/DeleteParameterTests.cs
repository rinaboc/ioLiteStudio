using System;
using System.Collections;
using System.Linq;
using Core.Domain;
using Core.Features;
using NUnit.Framework;
using UnityEngine.TestTools;

public class DeleteParameterTests : ParameterBaseTest
{
    [Test]
    public void When_DeleteParameter_Then_ParameterNotStored()
    {
        var config = new ParameterConfig("Test", -2f, 2f, BlendMode.Absolute);

        _dispatcher.Dispatch(new CreateParameterIntent(config));

        var parameter = _dummyContext.Parameters.GetAll().ToArray()[0];
        var paramCount = _dummyContext.Parameters.Count();

        _dispatcher.Dispatch(new DeleteParameterIntent(parameter.Id));

        Assert.AreEqual(paramCount - 1, _dummyContext.Parameters.Count());
        Assert.IsFalse(_dummyContext.Parameters.TryGet(parameter.Id, out _));
    }

    [Test]
    public void When_DeleteParameterAndUndo_Then_ParameterStored()
    {
        var config = new ParameterConfig("Test", -2f, 2f, BlendMode.Absolute);

        _dispatcher.Dispatch(new CreateParameterIntent(config));

        var parameter = _dummyContext.Parameters.GetAll().ToArray()[0];
        var paramCount = _dummyContext.Parameters.Count();

        _dispatcher.Dispatch(new DeleteParameterIntent(parameter.Id));
        _dispatcher.Dispatch(new UndoIntent());

        Assert.AreEqual(paramCount, _dummyContext.Parameters.Count());
        Assert.IsTrue(_dummyContext.Parameters.TryGet(parameter.Id, out _));
    }

    [Test]
    public void When_UndoDeletionAndRedo_Then_ParameterNotStored()
    {
        var config = new ParameterConfig("Test", -2f, 2f, BlendMode.Absolute);

        _dispatcher.Dispatch(new CreateParameterIntent(config));

        var parameter = _dummyContext.Parameters.GetAll().ToArray()[0];
        var paramCount = _dummyContext.Parameters.Count();

        _dispatcher.Dispatch(new DeleteParameterIntent(parameter.Id));
        _dispatcher.Dispatch(new UndoIntent());
        _dispatcher.Dispatch(new RedoIntent());

        Assert.AreEqual(paramCount - 1, _dummyContext.Parameters.Count());
        Assert.IsFalse(_dummyContext.Parameters.TryGet(parameter.Id, out _));
    }
}
