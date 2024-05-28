using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.U2D;

public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ constraint implement type
	// }} 

	// {{ AOT generic type
	// }}

	public void RefMethods()
	{
		// System.Object UnityEngine.AssetBundle::LoadAsset<System.Object>(System.String)
		// System.Object UnityEngine.GameObject::AddComponent<System.Object>()

		SpriteAtlasManager.atlasRequested += null;
		UnityEngine.ParticleSystem.MainModule model = new ParticleSystem.MainModule();
		
		PlayableDirector director = GetComponent<PlayableDirector>();
		PlayableAsset asset = Resources.Load<PlayableAsset>("Path");
		ControlPlayableAsset controlPlayableAsset = Resources.Load<ControlPlayableAsset>("Path");
		ControlTrack controlTrack =  Resources.Load<ControlTrack>("Path");
		ParticleSystem particle = Resources.Load<ParticleSystem>("Path");
		var duration = director.duration;
		var time = director.time;
		
		var aot = Boot.GetValue<int>("aot");
	}
}