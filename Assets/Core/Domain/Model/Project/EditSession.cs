namespace Core.Domain
{
    public enum EditTool { Select, Create, Delete }

    public class EditSession : Session
    {
        public override WorkspaceMode CurrentWorkspace => WorkspaceMode.Edit;
        public int EditedNodeId;
        public EditTool CurrentTool;
        // public MeshInfo CurrentMesh;
        // public Vertex? SelectedVertex;
    }
}
