using System;

namespace Core.Domain
{
    public class DefaultSession : Session
    {
        public override WorkspaceMode CurrentWorkspace => WorkspaceMode.Default;
        public Guid SelectedParamId;
        public int? SelectedNodeId;
    }
}
