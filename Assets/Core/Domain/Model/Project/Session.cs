namespace Core.Domain
{
    public enum WorkspaceMode { Default, Edit, Animation }
    public abstract class Session
    {
        public int UndoCount;
        public int RedoCount;
        public abstract WorkspaceMode CurrentWorkspace { get; }
    }
}
