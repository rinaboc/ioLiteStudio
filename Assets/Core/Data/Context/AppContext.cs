using Core.Domain;

namespace Core.Data
{
    public class AppContext : IAppContext
    {
        private readonly INodeRegistry _nodes;
        private readonly IParameterRegistry _parameters;
        private readonly IParameterKeyRegistry _parameterKeys;
        private readonly IAnimationClipRegistry _animationClips;
        private readonly IParameterTimelineTrackRegistry _timelineTracks;
        private readonly ITimelineKeyframeRegistry _timelineKeyframes;
        private readonly ProjectSettings _projectSettings;
        private Session _currentSession;

        public AppContext(
            INodeRegistry nodeRegistry, IParameterRegistry parameterRegistry,
            IParameterKeyRegistry parameterKeyRegistry, IAnimationClipRegistry animationClipRegistry,
            IParameterTimelineTrackRegistry timelineTrackRegistry, ITimelineKeyframeRegistry timelineKeyframeRegistry,
            ProjectSettings projectSettings, Session session
        )
        {
            _nodes = nodeRegistry;
            _parameters = parameterRegistry;
            _parameterKeys = parameterKeyRegistry;
            _animationClips = animationClipRegistry;
            _timelineTracks = timelineTrackRegistry;
            _timelineKeyframes = timelineKeyframeRegistry;
            _projectSettings = projectSettings;
            _currentSession = session;
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
}
