using System.Linq;
using Core.Domain;

namespace Core.Features
{
    public static class ReparentValidatorHelper
    {
        public static bool TryValidateParent(int? parentID, IAppContext ctx, ILogService logger)
        {
            if (parentID is not int pid) return true;

            if (!ctx.Nodes.TryGet(pid, out var parent))
            {
                logger.LogError("[REPARENT_NODE] Couldn't find new parent node.");
                return false;
            }

            if (parent.Type != NodeType.Deformer)
            {
                logger.LogError("[REPARENT_NODE] Only deformer nodes can be used as parents.");
                return false;
            }

            return true;
        }

        public static bool IsCircularReparenting(int? parentID, int nodeID, IAppContext ctx)
        {
            if (parentID is not int pid) return false;

            return ctx.Nodes.GetChildGraph(nodeID).Contains(pid) || parentID == nodeID;
        }
    }
}