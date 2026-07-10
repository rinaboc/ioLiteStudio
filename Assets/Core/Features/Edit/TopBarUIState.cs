using Core.Domain;

namespace Core.Features
{
    public record TopBarUIState(
        bool IsOpened = true,
        WorkspaceMode Workspace = WorkspaceMode.Default,
        bool CanUndo = false,
        bool CanRedo = false
    );
}
