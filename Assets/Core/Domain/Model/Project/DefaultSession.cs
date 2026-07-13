using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public enum Widget { Node, Parameter, History }

    public class DefaultSession : Session
    {
        public override WorkspaceMode CurrentWorkspace => WorkspaceMode.Default;
        public Guid SelectedParamId;
        public int? SelectedNodeId;
        public HashSet<Widget> OpenedWidgets = new HashSet<Widget> { Widget.Node, Widget.Parameter };
    }
}
