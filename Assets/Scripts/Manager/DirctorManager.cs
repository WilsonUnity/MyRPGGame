using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class DirctorManager : IActionInterface
{

	[Header("===== Play Setting =====")]
	public PlayableDirector pd;
	public PlayableAsset frontstab;

//	[Header("===== Assets Setting =====")] 
//	public ActorManager attacker;
//    public ActorManager victimer;
	
	private void Start()
	{
		pd = GetComponent<PlayableDirector>();
		pd.playOnAwake = false;
	}

	public void PlayTimeLine(string timelineName, ActorManager attacker, ActorManager victimer)
	{
		if (pd.playableAsset != null)
		{
			return;
		}
	    if (timelineName == "FrontStab")
		{
			pd.playableAsset = Instantiate(frontstab);

			TimelineAsset timeline = (TimelineAsset) pd.playableAsset;

			foreach (var track in timeline.GetOutputTracks())
			{
				if (track.name == "AttackAnimation")
				{
					pd.SetGenericBinding(track, attacker.ac._animator);
				}
				else if (track.name == "VicTimAnimation")
				{
					pd.SetGenericBinding(track, victimer.ac._animator);
				}
				else if (track.name == "AttackScript")
				{
					pd.SetGenericBinding(track, attacker);
					foreach (var clip in track.GetClips())
					{
						MyPlayableBehaviousClip myclip = (MyPlayableBehaviousClip) clip.asset;
						MyPlayableBehaviousBehaviour mybehav = myclip.template;
						mybehav.MyFloat = 777;
						pd.SetReferenceValue(myclip.MyActorManager.exposedName, attacker);
					}
				}
				else if (track.name == "VicTimScript")
				{
					pd.SetGenericBinding(track, victimer);
					foreach (var clip in track.GetClips())
					{
						MyPlayableBehaviousClip myclip = (MyPlayableBehaviousClip) clip.asset;
						MyPlayableBehaviousBehaviour mybehav = myclip.template;
						mybehav.MyFloat = 666;
						pd.SetReferenceValue(myclip.MyActorManager.exposedName, victimer);
					}
				}
			}

			pd.Play();
		}
	}
}
