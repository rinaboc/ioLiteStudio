using Core.Domain;
using Core.Data;
using UnityEngine;

public class AppInitializer : MonoBehaviour
{
    private IDispatcher _dispatcher;
    private IAppContext _context;

    void Awake()
    {
        _context = new AppContext(
            new NodeRegistry(),
            new ParameterRegistry(),
            new ParameterKeyRegistry(),
            new AnimationClipRegistry(),
            new ParameterTimelineTrackRegistry(),
            new TimelineKeyframeRegistry(),
            new ProjectSettings(),
            new DefaultSession()
        );
        _dispatcher = new Dispatcher(_context);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _dispatcher.CheckResultQueue();
    }
}
