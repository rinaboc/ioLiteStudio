using Core.Domain;

namespace Core.Features
{
    public class ToggleDeformerFolderCommandHandler : ICommandHandler<ToggleDeformerFolderIntent>
    {
        public CommandResult Execute(ToggleDeformerFolderIntent intent, IAppContext context)
        {
            if (!context.Nodes.TryGet(intent.ID, out var node) || node.Type != NodeType.Deformer)
            {
                ServiceLocator.Get<ILogService>().LogError("[TOGGLE_FOLDER] Couldn't find node or node is not a Deformer");
                return CommandResult.None;
            }

            var previousState = (node as DeformerNode).IsOpened;

            void mutation()
            {
                (node as DeformerNode).IsOpened = !previousState;
            }

            return CommandResult.MutationOnly(
                mutation,
                new ContextMutationResult(ContextMutationType.Session)
            );
        }
    }
}
