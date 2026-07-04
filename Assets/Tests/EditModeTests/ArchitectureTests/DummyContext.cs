using Core.Data;
using Core.Domain;

public class DummyContext : IAppContext
{
    private readonly INodeRegistry _nodes;
    private readonly IParameterRegistry _parameters;
    private readonly IParameterKeyRegistry _parameterKeys;
    private readonly IAnimationClipRegistry _animationClips;
    private readonly IParameterTimelineTrackRegistry _timelineTracks;
    private readonly ITimelineKeyframeRegistry _timelineKeyframes;
    private readonly ProjectSettings _projectSettings;
    private Session _currentSession;

    public DummyContext()
    {
        _nodes = new NodeRegistry();
        _parameters = new ParameterRegistry();
        _parameterKeys = new ParameterKeyRegistry();
        _animationClips = new AnimationClipRegistry();
        _timelineTracks = new ParameterTimelineTrackRegistry();
        _timelineKeyframes = new TimelineKeyframeRegistry();
        _projectSettings = new ProjectSettings();
        _currentSession = new DefaultSession();
    }

    public INodeRegistry Nodes => _nodes;
    public IParameterRegistry Parameters => _parameters;
    public IParameterKeyRegistry ParameterKeys => _parameterKeys;
    public IAnimationClipRegistry AnimationClips => _animationClips;
    public IParameterTimelineTrackRegistry TimelineTracks => _timelineTracks;
    public ITimelineKeyframeRegistry TimelineKeyframes => _timelineKeyframes;
    public ProjectSettings ProjectSettings => _projectSettings;
    public Session CurrentSession { get => _currentSession; set => _currentSession = value; }
}
