namespace Core.Domain
{
    public enum WorkspaceMode { Default, Edit, Animation }
    public enum WindowType { File, CreateNode }
    public abstract class Session
    {
        public int UndoCount;
        public int RedoCount;
        public bool IsWindowOpen;
        public WindowType WindowType;
        public abstract WorkspaceMode CurrentWorkspace
        { get; }
    }
}
