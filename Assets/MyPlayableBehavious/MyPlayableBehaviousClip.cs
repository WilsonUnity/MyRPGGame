using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MyPlayableBehaviousClip : PlayableAsset, ITimelineClipAsset
{
    public MyPlayableBehaviousBehaviour template = new MyPlayableBehaviousBehaviour ();
    public ExposedReference<ActorManager> MyActorManager;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.All; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MyPlayableBehaviousBehaviour>.Create (graph, template);
        MyPlayableBehaviousBehaviour clone = playable.GetBehaviour ();
        MyActorManager.exposedName = GetInstanceID().ToString();
        clone.am = MyActorManager.Resolve (graph.GetResolver ());
        return playable;
    }
}
