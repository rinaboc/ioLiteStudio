namespace Core.Domain
{
    public interface IAppContext
    {
        INodeRegistry Nodes { get; }
        IParameterRegistry Parameters { get; }
        IParameterKeyRegistry ParameterKeys { get; }
        IAnimationClipRegistry AnimationClips { get; }
        IParameterTimelineTrackRegistry TimelineTracks { get; }
        ITimelineKeyframeRegistry TimelineKeyframes { get; }
        ProjectSettings ProjectSettings { get; }
        Session CurrentSession { get; set; }
    }
}
