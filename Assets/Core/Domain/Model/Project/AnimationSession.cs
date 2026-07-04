using System;

namespace Core.Domain
{
    public class AnimationSession : Session
    {
        public override WorkspaceMode CurrentWorkspace => WorkspaceMode.Animation;
        public Guid ActiveClipId;
        public Guid SelectedKeyframeId;
    }
}
