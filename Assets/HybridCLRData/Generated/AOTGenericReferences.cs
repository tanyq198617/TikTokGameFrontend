using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"CatJson.dll",
		"DOTween.dll",
		"EventDispatcher.dll",
		"Main.dll",
		"Nino.Serialization.dll",
		"Nino.Shared.dll",
		"Sirenix.Utilities.dll",
		"System.Core.dll",
		"System.Memory.dll",
		"System.Runtime.CompilerServices.Unsafe.dll",
		"System.dll",
		"UniTask.dll",
		"UnityEngine.AndroidJNIModule.dll",
		"UnityEngine.CoreModule.dll",
		"UnityEngine.UI.dll",
		"YooAsset.dll",
		"mscorlib.dll",
		"spine-unity.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// CatJson.BaseJsonFormatter<object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<AFactory.<InitAsync>d__12>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<APanelBase.<PreLoadUI>d__36,byte>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<BattleSceneNode.<Initialize>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<CameraMgr.<TweenTo>d__26>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<CityStage.<LoadSceneAsync>d__7>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<FactoryRegister.<AutoRegister>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<GameObjectFactory.<PreLoadAsync>d__9<object>>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<GameObjectPool.<InitAsync>d__16<object>>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<GameObjectPool.<PreLoadAssets>d__18<object>>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<HotUpdateScripts.TNinoManager.<LoadBytesAsync>d__12<object,object>,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<HotUpdateScripts.TNinoParser.<ParseAsync>d__1<object>,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<LoginStage.<LoadSceneAsync>d__6>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<MapMgr.<LoadMap>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<PlayerHeadItem.<LoadHeadTexture>d__4>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<PlayerInfo.<CheckTextureAsync>d__34>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<PlayerInfo.<LoadHeadAsync>d__32>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<PlayerModel.<OnIntPlayer>d__19>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<PlayerModel.<S2C_OnGift>d__45>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<PlayerModel.<S2C_OnLike>d__47>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<ResultHandler.<ReqKillRecord>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<StartNetNode.<OnWait>d__3>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<TableRegister.<LoadTable>d__3>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<TextureMgr.<GetTexture>d__7,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<TextureMgr.<LoadSpriteAsync>d__5,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<TextureMgr.<LoadTextureAsync>d__4,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<TextureMgr.<SetAsync>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<TextureMgr.<SetAsync>d__9>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<UIInfiniteTable.<OnDelayCreateItemAsync>d__34>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<AFactory.<InitAsync>d__12>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<APanelBase.<PreLoadUI>d__36,byte>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<BattleSceneNode.<Initialize>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<CameraMgr.<TweenTo>d__26>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<CityStage.<LoadSceneAsync>d__7>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<FactoryRegister.<AutoRegister>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<GameObjectFactory.<PreLoadAsync>d__9<object>>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<GameObjectPool.<InitAsync>d__16<object>>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<GameObjectPool.<PreLoadAssets>d__18<object>>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<HotUpdateScripts.TNinoManager.<LoadBytesAsync>d__12<object,object>,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<HotUpdateScripts.TNinoParser.<ParseAsync>d__1<object>,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<LoginStage.<LoadSceneAsync>d__6>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<MapMgr.<LoadMap>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<PlayerHeadItem.<LoadHeadTexture>d__4>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<PlayerInfo.<CheckTextureAsync>d__34>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<PlayerInfo.<LoadHeadAsync>d__32>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<PlayerModel.<OnIntPlayer>d__19>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<PlayerModel.<S2C_OnGift>d__45>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<PlayerModel.<S2C_OnLike>d__47>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<ResultHandler.<ReqKillRecord>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<StartNetNode.<OnWait>d__3>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<TableRegister.<LoadTable>d__3>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<TextureMgr.<GetTexture>d__7,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<TextureMgr.<LoadSpriteAsync>d__5,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<TextureMgr.<LoadTextureAsync>d__4,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<TextureMgr.<SetAsync>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<TextureMgr.<SetAsync>d__9>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<UIInfiniteTable.<OnDelayCreateItemAsync>d__34>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<System.UIntPtr>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<byte>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<APanelBase.<ShowUI>d__35>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<ClassPool.<PreLoadAssets>d__18<object>>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<HotUpdateScripts.TNinoManager.<LoadAsync>d__14<object,object>>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<TableRegister.<PreLoadAll>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<APanelBase.<ShowUI>d__35>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<ClassPool.<PreLoadAssets>d__18<object>>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<HotUpdateScripts.TNinoManager.<LoadAsync>d__14<object,object>>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<TableRegister.<PreLoadAll>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.IStateMachineRunnerPromise<System.UIntPtr>
	// Cysharp.Threading.Tasks.CompilerServices.IStateMachineRunnerPromise<byte>
	// Cysharp.Threading.Tasks.CompilerServices.IStateMachineRunnerPromise<object>
	// Cysharp.Threading.Tasks.ITaskPoolNode<object>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.UIntPtr>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.UIntPtr>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.UIntPtr>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,byte>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,int>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,byte>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,int>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,byte>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,int>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.IUniTaskSource<byte>
	// Cysharp.Threading.Tasks.IUniTaskSource<int>
	// Cysharp.Threading.Tasks.IUniTaskSource<object>
	// Cysharp.Threading.Tasks.Internal.StatePool<Cysharp.Threading.Tasks.UniTask.Awaiter<byte>>
	// Cysharp.Threading.Tasks.Internal.StateTuple<Cysharp.Threading.Tasks.UniTask.Awaiter<byte>>
	// Cysharp.Threading.Tasks.TaskPool<object>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.UIntPtr>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.UIntPtr>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.UIntPtr>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,byte>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,int>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,byte>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,int>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,byte>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,int>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<byte>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<int>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<object>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.UIntPtr>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.UIntPtr>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.UIntPtr>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,byte>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,int>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,byte>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,int>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,byte>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,int>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<byte>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<int>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<object>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.UIntPtr>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.UIntPtr>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.UIntPtr>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,byte>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,int>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,byte>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,int>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,byte>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,int>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<byte>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<int>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<object>
	// Cysharp.Threading.Tasks.UniTask<System.UIntPtr>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.UIntPtr>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.UIntPtr>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.UIntPtr>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,byte>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,int>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,byte>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,int>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,byte>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,int>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,byte>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,int>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask<byte>
	// Cysharp.Threading.Tasks.UniTask<int>
	// Cysharp.Threading.Tasks.UniTask<object>
	// Cysharp.Threading.Tasks.UniTaskCompletionSourceCore<Cysharp.Threading.Tasks.AsyncUnit>
	// Cysharp.Threading.Tasks.UniTaskCompletionSourceCore<byte>
	// Cysharp.Threading.Tasks.UniTaskCompletionSourceCore<object>
	// Cysharp.Threading.Tasks.UniTaskExtensions.<>c__51<byte>
	// DG.Tweening.Core.DOGetter<UnityEngine.Vector3>
	// DG.Tweening.Core.DOSetter<UnityEngine.Vector3>
	// EventTriggerListener.EventHandle<object>
	// Nino.Serialization.NinoWrapperBase<double>
	// Nino.Serialization.NinoWrapperBase<int>
	// Nino.Serialization.NinoWrapperBase<object>
	// Nino.Shared.IO.ExtensibleBuffer<byte>
	// Nino.Shared.IO.ObjectPool<object>
	// Nino.Shared.UncheckedStack.Enumerator<object>
	// Nino.Shared.UncheckedStack<object>
	// Spine.Collections.OrderedDictionary.<GetEnumerator>d__34<object,object>
	// Spine.Collections.OrderedDictionary.KeyCollection<object,object>
	// Spine.Collections.OrderedDictionary.ValueCollection<object,object>
	// Spine.Collections.OrderedDictionary<object,object>
	// System.Action<Cysharp.Threading.Tasks.UniTask>
	// System.Action<DigitalRubyShared.GestureTouch>
	// System.Action<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Action<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Action<HotUpdateScripts.KingDataCfg>
	// System.Action<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Action<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Action<UnityEngine.Color>
	// System.Action<UnityEngine.EventSystems.RaycastResult>
	// System.Action<UnityEngine.Vector2>
	// System.Action<UnityEngine.Vector3>
	// System.Action<byte>
	// System.Action<double>
	// System.Action<float>
	// System.Action<int,int,int>
	// System.Action<int,int,object>
	// System.Action<int,int>
	// System.Action<int,long>
	// System.Action<int,object>
	// System.Action<int>
	// System.Action<long>
	// System.Action<object,UnityEngine.Vector2>
	// System.Action<object,int,object>
	// System.Action<object,object,long>
	// System.Action<object,object>
	// System.Action<object,ushort,object>
	// System.Action<object>
	// System.Action<ulong>
	// System.Collections.Concurrent.ConcurrentDictionary.<GetEnumerator>d__32<object,byte>
	// System.Collections.Concurrent.ConcurrentDictionary.DictionaryEnumerator<object,byte>
	// System.Collections.Concurrent.ConcurrentDictionary.Node<object,byte>
	// System.Collections.Concurrent.ConcurrentDictionary.Tables<object,byte>
	// System.Collections.Concurrent.ConcurrentDictionary<object,byte>
	// System.Collections.Concurrent.ConcurrentQueue.<Enumerate>d__27<object>
	// System.Collections.Concurrent.ConcurrentQueue.Segment<object>
	// System.Collections.Concurrent.ConcurrentQueue<object>
	// System.Collections.Concurrent.OrderablePartitioner.EnumerableDropIndices<object>
	// System.Collections.Concurrent.OrderablePartitioner<object>
	// System.Collections.Concurrent.Partitioner.DynamicPartitionerForIEnumerable<object>
	// System.Collections.Concurrent.Partitioner<object>
	// System.Collections.Generic.ArraySortHelper<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.Generic.ArraySortHelper<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.ArraySortHelper<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.Generic.ArraySortHelper<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.Generic.ArraySortHelper<HotUpdateScripts.KingDataCfg>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.Generic.ArraySortHelper<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Color>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector2>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector3>
	// System.Collections.Generic.ArraySortHelper<byte>
	// System.Collections.Generic.ArraySortHelper<double>
	// System.Collections.Generic.ArraySortHelper<float>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<long>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.ArraySortHelper<ulong>
	// System.Collections.Generic.Comparer<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.Generic.Comparer<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.Comparer<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.Generic.Comparer<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.Generic.Comparer<HotUpdateScripts.KingDataCfg>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.Generic.Comparer<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.Generic.Comparer<System.UIntPtr>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.UIntPtr>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,byte>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,int>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,byte>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,int>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.Comparer<UnityEngine.Color>
	// System.Collections.Generic.Comparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.Comparer<UnityEngine.Vector2>
	// System.Collections.Generic.Comparer<UnityEngine.Vector3>
	// System.Collections.Generic.Comparer<byte>
	// System.Collections.Generic.Comparer<double>
	// System.Collections.Generic.Comparer<float>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<long>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Comparer<ulong>
	// System.Collections.Generic.Dictionary.Enumerator<System.Nullable<long>,object>
	// System.Collections.Generic.Dictionary.Enumerator<float,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,DigitalRubyShared.FingersScript.ShownTouch>
	// System.Collections.Generic.Dictionary.Enumerator<int,DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.Dictionary.Enumerator<int,UnityEngine.Vector2>
	// System.Collections.Generic.Dictionary.Enumerator<int,float>
	// System.Collections.Generic.Dictionary.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<long,int>
	// System.Collections.Generic.Dictionary.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,DigitalRubyShared.FingersPanRotateScaleComponentScript.SavedState>
	// System.Collections.Generic.Dictionary.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.Enumerator<ulong,BestHTTP.SignalR.Messages.ClientMessage>
	// System.Collections.Generic.Dictionary.Enumerator<ulong,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<System.Nullable<long>,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<float,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,DigitalRubyShared.FingersScript.ShownTouch>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,UnityEngine.Vector2>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,float>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,DigitalRubyShared.FingersPanRotateScaleComponentScript.SavedState>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ulong,BestHTTP.SignalR.Messages.ClientMessage>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ulong,object>
	// System.Collections.Generic.Dictionary.KeyCollection<System.Nullable<long>,object>
	// System.Collections.Generic.Dictionary.KeyCollection<float,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,DigitalRubyShared.FingersScript.ShownTouch>
	// System.Collections.Generic.Dictionary.KeyCollection<int,DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.Dictionary.KeyCollection<int,UnityEngine.Vector2>
	// System.Collections.Generic.Dictionary.KeyCollection<int,float>
	// System.Collections.Generic.Dictionary.KeyCollection<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,long>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<long,int>
	// System.Collections.Generic.Dictionary.KeyCollection<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,DigitalRubyShared.FingersPanRotateScaleComponentScript.SavedState>
	// System.Collections.Generic.Dictionary.KeyCollection<object,float>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,long>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<ulong,BestHTTP.SignalR.Messages.ClientMessage>
	// System.Collections.Generic.Dictionary.KeyCollection<ulong,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<System.Nullable<long>,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<float,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,DigitalRubyShared.FingersScript.ShownTouch>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,UnityEngine.Vector2>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,float>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,DigitalRubyShared.FingersPanRotateScaleComponentScript.SavedState>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,long>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ulong,BestHTTP.SignalR.Messages.ClientMessage>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ulong,object>
	// System.Collections.Generic.Dictionary.ValueCollection<System.Nullable<long>,object>
	// System.Collections.Generic.Dictionary.ValueCollection<float,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,DigitalRubyShared.FingersScript.ShownTouch>
	// System.Collections.Generic.Dictionary.ValueCollection<int,DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.Dictionary.ValueCollection<int,UnityEngine.Vector2>
	// System.Collections.Generic.Dictionary.ValueCollection<int,float>
	// System.Collections.Generic.Dictionary.ValueCollection<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,long>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<long,int>
	// System.Collections.Generic.Dictionary.ValueCollection<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,DigitalRubyShared.FingersPanRotateScaleComponentScript.SavedState>
	// System.Collections.Generic.Dictionary.ValueCollection<object,float>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,long>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<ulong,BestHTTP.SignalR.Messages.ClientMessage>
	// System.Collections.Generic.Dictionary.ValueCollection<ulong,object>
	// System.Collections.Generic.Dictionary<System.Nullable<long>,object>
	// System.Collections.Generic.Dictionary<float,int>
	// System.Collections.Generic.Dictionary<int,DigitalRubyShared.FingersScript.ShownTouch>
	// System.Collections.Generic.Dictionary<int,DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.Dictionary<int,UnityEngine.Vector2>
	// System.Collections.Generic.Dictionary<int,float>
	// System.Collections.Generic.Dictionary<int,int>
	// System.Collections.Generic.Dictionary<int,long>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<long,int>
	// System.Collections.Generic.Dictionary<long,object>
	// System.Collections.Generic.Dictionary<object,DigitalRubyShared.FingersPanRotateScaleComponentScript.SavedState>
	// System.Collections.Generic.Dictionary<object,float>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,long>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<ulong,BestHTTP.SignalR.Messages.ClientMessage>
	// System.Collections.Generic.Dictionary<ulong,object>
	// System.Collections.Generic.EqualityComparer<BestHTTP.SignalR.Messages.ClientMessage>
	// System.Collections.Generic.EqualityComparer<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.Generic.EqualityComparer<DigitalRubyShared.FingersPanRotateScaleComponentScript.SavedState>
	// System.Collections.Generic.EqualityComparer<DigitalRubyShared.FingersScript.ShownTouch>
	// System.Collections.Generic.EqualityComparer<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.EqualityComparer<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.Generic.EqualityComparer<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.Generic.EqualityComparer<HotUpdateScripts.KingDataCfg>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.Generic.EqualityComparer<System.Nullable<long>>
	// System.Collections.Generic.EqualityComparer<System.UIntPtr>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.UIntPtr>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,byte>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,int>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,byte>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,int>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.EqualityComparer<UnityEngine.Color>
	// System.Collections.Generic.EqualityComparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.EqualityComparer<UnityEngine.Vector2>
	// System.Collections.Generic.EqualityComparer<UnityEngine.Vector3>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<double>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<long>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.EqualityComparer<ulong>
	// System.Collections.Generic.HashSet.Enumerator<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.HashSet.Enumerator<int>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.HashSet<int>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSetEqualityComparer<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.HashSetEqualityComparer<int>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.ICollection<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.Generic.ICollection<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.ICollection<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.Generic.ICollection<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.Generic.ICollection<HotUpdateScripts.KingDataCfg>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<System.Nullable<long>,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<float,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,DigitalRubyShared.FingersScript.ShownTouch>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,DigitalRubyShared.GestureTouch>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,UnityEngine.Vector2>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanRotateScaleComponentScript.SavedState>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ulong,BestHTTP.SignalR.Messages.ClientMessage>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.ICollection<UnityEngine.Color>
	// System.Collections.Generic.ICollection<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ICollection<UnityEngine.Vector2>
	// System.Collections.Generic.ICollection<UnityEngine.Vector3>
	// System.Collections.Generic.ICollection<byte>
	// System.Collections.Generic.ICollection<double>
	// System.Collections.Generic.ICollection<float>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<long>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.ICollection<ulong>
	// System.Collections.Generic.IComparer<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.Generic.IComparer<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.IComparer<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.Generic.IComparer<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.Generic.IComparer<HotUpdateScripts.KingDataCfg>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.Generic.IComparer<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.Generic.IComparer<UnityEngine.Color>
	// System.Collections.Generic.IComparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IComparer<UnityEngine.Vector2>
	// System.Collections.Generic.IComparer<UnityEngine.Vector3>
	// System.Collections.Generic.IComparer<byte>
	// System.Collections.Generic.IComparer<double>
	// System.Collections.Generic.IComparer<float>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<long>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IComparer<ulong>
	// System.Collections.Generic.IDictionary<object,byte>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IEnumerable<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.Generic.IEnumerable<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.IEnumerable<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.Generic.IEnumerable<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.Generic.IEnumerable<HotUpdateScripts.KingDataCfg>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.Nullable<long>,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<float,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,DigitalRubyShared.FingersScript.ShownTouch>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,DigitalRubyShared.GestureTouch>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,UnityEngine.Vector2>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanRotateScaleComponentScript.SavedState>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ulong,BestHTTP.SignalR.Messages.ClientMessage>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.IEnumerable<UnityEngine.Color>
	// System.Collections.Generic.IEnumerable<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector2>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerable<byte>
	// System.Collections.Generic.IEnumerable<double>
	// System.Collections.Generic.IEnumerable<float>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<long>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerable<ulong>
	// System.Collections.Generic.IEnumerator<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.Generic.IEnumerator<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.IEnumerator<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.Generic.IEnumerator<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.Generic.IEnumerator<HotUpdateScripts.KingDataCfg>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<System.Nullable<long>,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<float,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,DigitalRubyShared.FingersScript.ShownTouch>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,DigitalRubyShared.GestureTouch>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,UnityEngine.Vector2>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanRotateScaleComponentScript.SavedState>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,float>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,long>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ulong,BestHTTP.SignalR.Messages.ClientMessage>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ulong,object>>
	// System.Collections.Generic.IEnumerator<UnityEngine.Color>
	// System.Collections.Generic.IEnumerator<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector2>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerator<byte>
	// System.Collections.Generic.IEnumerator<double>
	// System.Collections.Generic.IEnumerator<float>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<long>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEnumerator<ulong>
	// System.Collections.Generic.IEqualityComparer<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.IEqualityComparer<System.Nullable<long>>
	// System.Collections.Generic.IEqualityComparer<float>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<long>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IEqualityComparer<ulong>
	// System.Collections.Generic.IList<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.Generic.IList<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.IList<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.Generic.IList<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.Generic.IList<HotUpdateScripts.KingDataCfg>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.Generic.IList<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.Generic.IList<UnityEngine.Color>
	// System.Collections.Generic.IList<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.IList<UnityEngine.Vector2>
	// System.Collections.Generic.IList<UnityEngine.Vector3>
	// System.Collections.Generic.IList<byte>
	// System.Collections.Generic.IList<double>
	// System.Collections.Generic.IList<float>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<long>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.IList<ulong>
	// System.Collections.Generic.KeyValuePair<System.Nullable<long>,object>
	// System.Collections.Generic.KeyValuePair<float,float>
	// System.Collections.Generic.KeyValuePair<float,int>
	// System.Collections.Generic.KeyValuePair<int,DigitalRubyShared.FingersScript.ShownTouch>
	// System.Collections.Generic.KeyValuePair<int,DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.KeyValuePair<int,UnityEngine.Vector2>
	// System.Collections.Generic.KeyValuePair<int,float>
	// System.Collections.Generic.KeyValuePair<int,int>
	// System.Collections.Generic.KeyValuePair<int,long>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<long,int>
	// System.Collections.Generic.KeyValuePair<long,object>
	// System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>
	// System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanRotateScaleComponentScript.SavedState>
	// System.Collections.Generic.KeyValuePair<object,byte>
	// System.Collections.Generic.KeyValuePair<object,float>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,long>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.KeyValuePair<uint,object>
	// System.Collections.Generic.KeyValuePair<ulong,BestHTTP.SignalR.Messages.ClientMessage>
	// System.Collections.Generic.KeyValuePair<ulong,object>
	// System.Collections.Generic.List.Enumerator<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.Generic.List.Enumerator<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.List.Enumerator<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.Generic.List.Enumerator<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.Generic.List.Enumerator<HotUpdateScripts.KingDataCfg>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.Generic.List.Enumerator<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Color>
	// System.Collections.Generic.List.Enumerator<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector2>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector3>
	// System.Collections.Generic.List.Enumerator<byte>
	// System.Collections.Generic.List.Enumerator<double>
	// System.Collections.Generic.List.Enumerator<float>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<long>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List.Enumerator<ulong>
	// System.Collections.Generic.List.SynchronizedList<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.Generic.List.SynchronizedList<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.List.SynchronizedList<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.Generic.List.SynchronizedList<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.Generic.List.SynchronizedList<HotUpdateScripts.KingDataCfg>
	// System.Collections.Generic.List.SynchronizedList<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.Generic.List.SynchronizedList<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.Generic.List.SynchronizedList<UnityEngine.Color>
	// System.Collections.Generic.List.SynchronizedList<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.List.SynchronizedList<UnityEngine.Vector2>
	// System.Collections.Generic.List.SynchronizedList<UnityEngine.Vector3>
	// System.Collections.Generic.List.SynchronizedList<byte>
	// System.Collections.Generic.List.SynchronizedList<double>
	// System.Collections.Generic.List.SynchronizedList<float>
	// System.Collections.Generic.List.SynchronizedList<int>
	// System.Collections.Generic.List.SynchronizedList<long>
	// System.Collections.Generic.List.SynchronizedList<object>
	// System.Collections.Generic.List.SynchronizedList<ulong>
	// System.Collections.Generic.List<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.Generic.List<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.List<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.Generic.List<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.Generic.List<HotUpdateScripts.KingDataCfg>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.Generic.List<UnityEngine.Color>
	// System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.List<UnityEngine.Vector2>
	// System.Collections.Generic.List<UnityEngine.Vector3>
	// System.Collections.Generic.List<byte>
	// System.Collections.Generic.List<double>
	// System.Collections.Generic.List<float>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<long>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.List<ulong>
	// System.Collections.Generic.ObjectComparer<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.Generic.ObjectComparer<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.ObjectComparer<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.Generic.ObjectComparer<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.Generic.ObjectComparer<HotUpdateScripts.KingDataCfg>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.Generic.ObjectComparer<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.Generic.ObjectComparer<System.UIntPtr>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,byte>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,int>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Color>
	// System.Collections.Generic.ObjectComparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector2>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectComparer<byte>
	// System.Collections.Generic.ObjectComparer<double>
	// System.Collections.Generic.ObjectComparer<float>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<long>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectComparer<ulong>
	// System.Collections.Generic.ObjectEqualityComparer<BestHTTP.SignalR.Messages.ClientMessage>
	// System.Collections.Generic.ObjectEqualityComparer<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.Generic.ObjectEqualityComparer<DigitalRubyShared.FingersPanRotateScaleComponentScript.SavedState>
	// System.Collections.Generic.ObjectEqualityComparer<DigitalRubyShared.FingersScript.ShownTouch>
	// System.Collections.Generic.ObjectEqualityComparer<DigitalRubyShared.GestureTouch>
	// System.Collections.Generic.ObjectEqualityComparer<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.Generic.ObjectEqualityComparer<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.Generic.ObjectEqualityComparer<HotUpdateScripts.KingDataCfg>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.Generic.ObjectEqualityComparer<System.Nullable<long>>
	// System.Collections.Generic.ObjectEqualityComparer<System.UIntPtr>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,byte>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,int>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.Color>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.Vector2>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<double>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<long>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<ulong>
	// System.Collections.Generic.Queue.Enumerator<BallInfo>
	// System.Collections.Generic.Queue.Enumerator<DigitalRubyShared.GestureVelocityTracker.VelocityHistory>
	// System.Collections.Generic.Queue.Enumerator<NoPlayDaGeRuChangStruct>
	// System.Collections.Generic.Queue.Enumerator<OverflowBall>
	// System.Collections.Generic.Queue.Enumerator<TimeLineStruct>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<BallInfo>
	// System.Collections.Generic.Queue<DigitalRubyShared.GestureVelocityTracker.VelocityHistory>
	// System.Collections.Generic.Queue<NoPlayDaGeRuChangStruct>
	// System.Collections.Generic.Queue<OverflowBall>
	// System.Collections.Generic.Queue<TimeLineStruct>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.SortedDictionary.Enumerator<uint,object>
	// System.Collections.Generic.SortedDictionary<uint,object>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<Cysharp.Threading.Tasks.UniTask>
	// System.Collections.ObjectModel.ReadOnlyCollection<DigitalRubyShared.GestureTouch>
	// System.Collections.ObjectModel.ReadOnlyCollection<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Collections.ObjectModel.ReadOnlyCollection<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Collections.ObjectModel.ReadOnlyCollection<HotUpdateScripts.KingDataCfg>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Collections.ObjectModel.ReadOnlyCollection<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Color>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.EventSystems.RaycastResult>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector2>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector3>
	// System.Collections.ObjectModel.ReadOnlyCollection<byte>
	// System.Collections.ObjectModel.ReadOnlyCollection<double>
	// System.Collections.ObjectModel.ReadOnlyCollection<float>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<long>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<ulong>
	// System.Comparison<Cysharp.Threading.Tasks.UniTask>
	// System.Comparison<DigitalRubyShared.GestureTouch>
	// System.Comparison<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Comparison<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Comparison<HotUpdateScripts.KingDataCfg>
	// System.Comparison<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Comparison<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Comparison<UnityEngine.Color>
	// System.Comparison<UnityEngine.EventSystems.RaycastResult>
	// System.Comparison<UnityEngine.Vector2>
	// System.Comparison<UnityEngine.Vector3>
	// System.Comparison<byte>
	// System.Comparison<double>
	// System.Comparison<float>
	// System.Comparison<int>
	// System.Comparison<long>
	// System.Comparison<object>
	// System.Comparison<ulong>
	// System.Func<System.UIntPtr>
	// System.Func<byte>
	// System.Func<int,DigitalRubyShared.GestureTouch>
	// System.Func<int,object,object,object>
	// System.Func<int>
	// System.Func<object,System.Nullable<byte>>
	// System.Func<object,System.UIntPtr>
	// System.Func<object,byte>
	// System.Func<object,int,object>
	// System.Func<object,object,Cysharp.Threading.Tasks.UniTask<object>>
	// System.Func<object,object,byte>
	// System.Func<object,object,long,object,object>
	// System.Func<object,object,object,byte>
	// System.Func<object,object,object,object>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.Func<object>
	// System.Func<ushort,byte>
	// System.IComparable<DigitalRubyShared.GestureTouch>
	// System.IComparable<object>
	// System.IEquatable<object>
	// System.IProgress<float>
	// System.Linq.Buffer<System.Collections.Generic.KeyValuePair<int,DigitalRubyShared.GestureTouch>>
	// System.Linq.Buffer<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Linq.Buffer<object>
	// System.Nullable<DigitalRubyShared.GestureTouch>
	// System.Nullable<System.DateTime>
	// System.Nullable<System.TimeSpan>
	// System.Nullable<UnityEngine.Vector3>
	// System.Nullable<byte>
	// System.Nullable<float>
	// System.Nullable<int>
	// System.Nullable<long>
	// System.Nullable<ushort>
	// System.Predicate<Cysharp.Threading.Tasks.UniTask>
	// System.Predicate<DigitalRubyShared.GestureTouch>
	// System.Predicate<DigitalRubyShared.ImageGestureRecognizer.Point>
	// System.Predicate<DigitalRubyShared.ImageGestureRecognizerComponentScriptImageEntry>
	// System.Predicate<HotUpdateScripts.KingDataCfg>
	// System.Predicate<System.Collections.Generic.KeyValuePair<float,float>>
	// System.Predicate<System.Collections.Generic.KeyValuePair<object,DigitalRubyShared.FingersPanARComponentScript.OrigState>>
	// System.Predicate<UnityEngine.Color>
	// System.Predicate<UnityEngine.EventSystems.RaycastResult>
	// System.Predicate<UnityEngine.Vector2>
	// System.Predicate<UnityEngine.Vector3>
	// System.Predicate<byte>
	// System.Predicate<double>
	// System.Predicate<float>
	// System.Predicate<int>
	// System.Predicate<long>
	// System.Predicate<object>
	// System.Predicate<ulong>
	// System.ReadOnlySpan<byte>
	// System.Runtime.CompilerServices.ConditionalWeakTable.CreateValueCallback<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable<object,object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<System.UIntPtr>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<System.UIntPtr>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<object>
	// System.Runtime.CompilerServices.TaskAwaiter<System.UIntPtr>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Span<byte>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<System.UIntPtr>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<object>
	// System.Threading.Tasks.Parallel.<>c__DisplayClass17_0<object>
	// System.Threading.Tasks.Parallel.<>c__DisplayClass30_0<object,object>
	// System.Threading.Tasks.Parallel.<>c__DisplayClass31_0<object,object>
	// System.Threading.Tasks.Parallel.<>c__DisplayClass42_0<object,object>
	// System.Threading.Tasks.Task.<>c<System.UIntPtr>
	// System.Threading.Tasks.Task.<>c<object>
	// System.Threading.Tasks.Task<System.UIntPtr>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.TaskCompletionSource<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<System.UIntPtr>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_1<System.UIntPtr>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_1<object>
	// System.Threading.Tasks.TaskFactory<System.UIntPtr>
	// System.Threading.Tasks.TaskFactory<object>
	// System.ValueTuple<byte,System.UIntPtr>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.UIntPtr>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.UIntPtr>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,byte>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,int>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,byte>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,int>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,byte>>
	// System.ValueTuple<byte,System.ValueTuple<byte,int>>
	// System.ValueTuple<byte,System.ValueTuple<byte,object>>
	// System.ValueTuple<byte,byte>
	// System.ValueTuple<byte,int>
	// System.ValueTuple<byte,object>
	// UIEventHandle<object>
	// UnityEngine.EventSystems.ExecuteEvents.EventFunction<object>
	// UnityEngine.Events.InvokableCall<UnityEngine.Vector2>
	// UnityEngine.Events.InvokableCall<byte>
	// UnityEngine.Events.InvokableCall<float>
	// UnityEngine.Events.InvokableCall<int>
	// UnityEngine.Events.InvokableCall<object>
	// UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene>
	// UnityEngine.Events.UnityAction<UnityEngine.Vector2>
	// UnityEngine.Events.UnityAction<byte,int>
	// UnityEngine.Events.UnityAction<byte,object>
	// UnityEngine.Events.UnityAction<byte>
	// UnityEngine.Events.UnityAction<float>
	// UnityEngine.Events.UnityAction<int>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityEvent<UnityEngine.Vector2>
	// UnityEngine.Events.UnityEvent<byte>
	// UnityEngine.Events.UnityEvent<float>
	// UnityEngine.Events.UnityEvent<int>
	// UnityEngine.Events.UnityEvent<object>
	// UnityEngine.UI.ListPool<object>
	// UnityEngine.UI.ObjectPool<object>
	// }}

	public void RefMethods()
	{
		// byte Boot.GetValue<byte>(string)
		// int Boot.GetValue<int>(string)
		// object CatJson.Extensions.ParseJson<object>(string,CatJson.JsonParser)
		// string CatJson.Extensions.ToJson<object>(object,CatJson.JsonParser)
		// object CatJson.JsonParser.ParseJson<object>(string)
		// string CatJson.JsonParser.ToJson<object>(object)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,AFactory.<InitAsync>d__12>(Cysharp.Threading.Tasks.UniTask.Awaiter&,AFactory.<InitAsync>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,BattleSceneNode.<Initialize>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter&,BattleSceneNode.<Initialize>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,CameraMgr.<TweenTo>d__26>(Cysharp.Threading.Tasks.UniTask.Awaiter&,CameraMgr.<TweenTo>d__26&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,CityStage.<LoadSceneAsync>d__7>(Cysharp.Threading.Tasks.UniTask.Awaiter&,CityStage.<LoadSceneAsync>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,FactoryRegister.<AutoRegister>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter&,FactoryRegister.<AutoRegister>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,GameObjectFactory.<PreLoadAsync>d__9<object>>(Cysharp.Threading.Tasks.UniTask.Awaiter&,GameObjectFactory.<PreLoadAsync>d__9<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,GameObjectPool.<InitAsync>d__16<object>>(Cysharp.Threading.Tasks.UniTask.Awaiter&,GameObjectPool.<InitAsync>d__16<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,GameObjectPool.<PreLoadAssets>d__18<object>>(Cysharp.Threading.Tasks.UniTask.Awaiter&,GameObjectPool.<PreLoadAssets>d__18<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,LoginStage.<LoadSceneAsync>d__6>(Cysharp.Threading.Tasks.UniTask.Awaiter&,LoginStage.<LoadSceneAsync>d__6&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,MapMgr.<LoadMap>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter&,MapMgr.<LoadMap>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,PlayerInfo.<CheckTextureAsync>d__34>(Cysharp.Threading.Tasks.UniTask.Awaiter&,PlayerInfo.<CheckTextureAsync>d__34&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,PlayerModel.<OnIntPlayer>d__19>(Cysharp.Threading.Tasks.UniTask.Awaiter&,PlayerModel.<OnIntPlayer>d__19&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,PlayerModel.<S2C_OnGift>d__45>(Cysharp.Threading.Tasks.UniTask.Awaiter&,PlayerModel.<S2C_OnGift>d__45&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,PlayerModel.<S2C_OnLike>d__47>(Cysharp.Threading.Tasks.UniTask.Awaiter&,PlayerModel.<S2C_OnLike>d__47&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,TableRegister.<LoadTable>d__3>(Cysharp.Threading.Tasks.UniTask.Awaiter&,TableRegister.<LoadTable>d__3&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,TextureMgr.<SetAsync>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter&,TextureMgr.<SetAsync>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,TextureMgr.<SetAsync>d__9>(Cysharp.Threading.Tasks.UniTask.Awaiter&,TextureMgr.<SetAsync>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<int>,ResultHandler.<ReqKillRecord>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter<int>&,ResultHandler.<ReqKillRecord>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<int>,StartNetNode.<OnWait>d__3>(Cysharp.Threading.Tasks.UniTask.Awaiter<int>&,StartNetNode.<OnWait>d__3&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,PlayerHeadItem.<LoadHeadTexture>d__4>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,PlayerHeadItem.<LoadHeadTexture>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,PlayerInfo.<LoadHeadAsync>d__32>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,PlayerInfo.<LoadHeadAsync>d__32&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,TextureMgr.<SetAsync>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,TextureMgr.<SetAsync>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,TextureMgr.<SetAsync>d__9>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,TextureMgr.<SetAsync>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,CameraMgr.<TweenTo>d__26>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,CameraMgr.<TweenTo>d__26&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,ResultHandler.<ReqKillRecord>d__2>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,ResultHandler.<ReqKillRecord>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,UIInfiniteTable.<OnDelayCreateItemAsync>d__34>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,UIInfiniteTable.<OnDelayCreateItemAsync>d__34&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<byte>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,APanelBase.<PreLoadUI>d__36>(Cysharp.Threading.Tasks.UniTask.Awaiter&,APanelBase.<PreLoadUI>d__36&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,HotUpdateScripts.TNinoManager.<LoadBytesAsync>d__12<object,object>>(Cysharp.Threading.Tasks.UniTask.Awaiter&,HotUpdateScripts.TNinoManager.<LoadBytesAsync>d__12<object,object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,TextureMgr.<GetTexture>d__7>(Cysharp.Threading.Tasks.UniTask.Awaiter&,TextureMgr.<GetTexture>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,HotUpdateScripts.TNinoParser.<ParseAsync>d__1<object>>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,HotUpdateScripts.TNinoParser.<ParseAsync>d__1<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,TextureMgr.<GetTexture>d__7>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,TextureMgr.<GetTexture>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,TextureMgr.<LoadSpriteAsync>d__5>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,TextureMgr.<LoadSpriteAsync>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,TextureMgr.<LoadTextureAsync>d__4>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,TextureMgr.<LoadTextureAsync>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,HotUpdateScripts.TNinoParser.<ParseAsync>d__1<object>>(System.Runtime.CompilerServices.TaskAwaiter<object>&,HotUpdateScripts.TNinoParser.<ParseAsync>d__1<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<AFactory.<InitAsync>d__12>(AFactory.<InitAsync>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<BattleSceneNode.<Initialize>d__2>(BattleSceneNode.<Initialize>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<CameraMgr.<TweenTo>d__26>(CameraMgr.<TweenTo>d__26&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<CityStage.<LoadSceneAsync>d__7>(CityStage.<LoadSceneAsync>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<FactoryRegister.<AutoRegister>d__2>(FactoryRegister.<AutoRegister>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<GameObjectFactory.<PreLoadAsync>d__9<object>>(GameObjectFactory.<PreLoadAsync>d__9<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<GameObjectPool.<InitAsync>d__16<object>>(GameObjectPool.<InitAsync>d__16<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<GameObjectPool.<PreLoadAssets>d__18<object>>(GameObjectPool.<PreLoadAssets>d__18<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<LoginStage.<LoadSceneAsync>d__6>(LoginStage.<LoadSceneAsync>d__6&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<MapMgr.<LoadMap>d__2>(MapMgr.<LoadMap>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<PlayerHeadItem.<LoadHeadTexture>d__4>(PlayerHeadItem.<LoadHeadTexture>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<PlayerInfo.<CheckTextureAsync>d__34>(PlayerInfo.<CheckTextureAsync>d__34&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<PlayerInfo.<LoadHeadAsync>d__32>(PlayerInfo.<LoadHeadAsync>d__32&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<PlayerModel.<OnIntPlayer>d__19>(PlayerModel.<OnIntPlayer>d__19&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<PlayerModel.<S2C_OnGift>d__45>(PlayerModel.<S2C_OnGift>d__45&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<PlayerModel.<S2C_OnLike>d__47>(PlayerModel.<S2C_OnLike>d__47&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<ResultHandler.<ReqKillRecord>d__2>(ResultHandler.<ReqKillRecord>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<StartNetNode.<OnWait>d__3>(StartNetNode.<OnWait>d__3&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<TableRegister.<LoadTable>d__3>(TableRegister.<LoadTable>d__3&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<TextureMgr.<SetAsync>d__10>(TextureMgr.<SetAsync>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<TextureMgr.<SetAsync>d__9>(TextureMgr.<SetAsync>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<UIInfiniteTable.<OnDelayCreateItemAsync>d__34>(UIInfiniteTable.<OnDelayCreateItemAsync>d__34&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<System.UIntPtr>.Start<HotUpdateScripts.TNinoParser.<ParseAsync>d__1<object>>(HotUpdateScripts.TNinoParser.<ParseAsync>d__1<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<byte>.Start<APanelBase.<PreLoadUI>d__36>(APanelBase.<PreLoadUI>d__36&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<HotUpdateScripts.TNinoManager.<LoadBytesAsync>d__12<object,object>>(HotUpdateScripts.TNinoManager.<LoadBytesAsync>d__12<object,object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<HotUpdateScripts.TNinoParser.<ParseAsync>d__1<object>>(HotUpdateScripts.TNinoParser.<ParseAsync>d__1<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<TextureMgr.<GetTexture>d__7>(TextureMgr.<GetTexture>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<TextureMgr.<LoadSpriteAsync>d__5>(TextureMgr.<LoadSpriteAsync>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<TextureMgr.<LoadTextureAsync>d__4>(TextureMgr.<LoadTextureAsync>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,APanelBase.<ShowUI>d__35>(Cysharp.Threading.Tasks.UniTask.Awaiter&,APanelBase.<ShowUI>d__35&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,TableRegister.<PreLoadAll>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter&,TableRegister.<PreLoadAll>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<byte>,APanelBase.<ShowUI>d__35>(Cysharp.Threading.Tasks.UniTask.Awaiter<byte>&,APanelBase.<ShowUI>d__35&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,HotUpdateScripts.TNinoManager.<LoadAsync>d__14<object,object>>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,HotUpdateScripts.TNinoManager.<LoadAsync>d__14<object,object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,ClassPool.<PreLoadAssets>d__18<object>>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,ClassPool.<PreLoadAssets>d__18<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<APanelBase.<ShowUI>d__35>(APanelBase.<ShowUI>d__35&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<ClassPool.<PreLoadAssets>d__18<object>>(ClassPool.<PreLoadAssets>d__18<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<HotUpdateScripts.TNinoManager.<LoadAsync>d__14<object,object>>(HotUpdateScripts.TNinoManager.<LoadAsync>d__14<object,object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<TableRegister.<PreLoadAll>d__2>(TableRegister.<PreLoadAll>d__2&)
		// Cysharp.Threading.Tasks.Internal.StateTuple<Cysharp.Threading.Tasks.UniTask.Awaiter<byte>> Cysharp.Threading.Tasks.Internal.StateTuple.Create<Cysharp.Threading.Tasks.UniTask.Awaiter<byte>>(Cysharp.Threading.Tasks.UniTask.Awaiter<byte>)
		// System.Void Cysharp.Threading.Tasks.UniTaskExtensions.Forget<byte>(Cysharp.Threading.Tasks.UniTask<byte>)
		// object DG.Tweening.TweenExtensions.Play<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.OnComplete<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.SetAutoKill<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.SetEase<object>(object,DG.Tweening.Ease)
		// object DG.Tweening.TweenSettingsExtensions.SetLoops<object>(object,int,DG.Tweening.LoopType)
		// object DG.Tweening.TweenSettingsExtensions.SetUpdate<object>(object,bool)
		// System.Void EventDispatcher.EventDelegateData.CallBack<byte>(byte)
		// System.Void EventDispatcher.EventDelegateData.CallBack<int,int,int>(int,int,int)
		// System.Void EventDispatcher.EventDelegateData.CallBack<int,int,object>(int,int,object)
		// System.Void EventDispatcher.EventDelegateData.CallBack<int,int>(int,int)
		// System.Void EventDispatcher.EventDelegateData.CallBack<int,long>(int,long)
		// System.Void EventDispatcher.EventDelegateData.CallBack<int,object>(int,object)
		// System.Void EventDispatcher.EventDelegateData.CallBack<int>(int)
		// System.Void EventDispatcher.EventDelegateData.CallBack<long>(long)
		// System.Void EventDispatcher.EventDelegateData.CallBack<object,object,long>(object,object,long)
		// System.Void EventDispatcher.EventDelegateData.CallBack<object,object>(object,object)
		// System.Void EventDispatcher.EventDelegateData.CallBack<object>(object)
		// System.Void EventDispatcher.EventDispatcher.Send<byte>(int,byte)
		// System.Void EventDispatcher.EventDispatcher.Send<int,int,int>(int,int,int,int)
		// System.Void EventDispatcher.EventDispatcher.Send<int,int,object>(int,int,int,object)
		// System.Void EventDispatcher.EventDispatcher.Send<int,int>(int,int,int)
		// System.Void EventDispatcher.EventDispatcher.Send<int,long>(int,int,long)
		// System.Void EventDispatcher.EventDispatcher.Send<int,object>(int,int,object)
		// System.Void EventDispatcher.EventDispatcher.Send<int>(int,int)
		// System.Void EventDispatcher.EventDispatcher.Send<long>(int,long)
		// System.Void EventDispatcher.EventDispatcher.Send<object,object,long>(int,object,object,long)
		// System.Void EventDispatcher.EventDispatcher.Send<object,object>(int,object,object)
		// System.Void EventDispatcher.EventDispatcher.Send<object>(int,object)
		// bool EventDispatcher.EventExtension.AddEventListener<int,int,int>(EventDispatcher.Event,int,System.Action<int,int,int>)
		// bool EventDispatcher.EventExtension.AddEventListener<int,int,object>(EventDispatcher.Event,int,System.Action<int,int,object>)
		// bool EventDispatcher.EventExtension.AddEventListener<int,int>(EventDispatcher.Event,int,System.Action<int,int>)
		// bool EventDispatcher.EventExtension.AddEventListener<int,long>(EventDispatcher.Event,int,System.Action<int,long>)
		// bool EventDispatcher.EventExtension.AddEventListener<int,object>(EventDispatcher.Event,int,System.Action<int,object>)
		// bool EventDispatcher.EventExtension.AddEventListener<object,object,long>(EventDispatcher.Event,int,System.Action<object,object,long>)
		// bool EventDispatcher.EventExtension.AddEventListener<object,object>(EventDispatcher.Event,int,System.Action<object,object>)
		// System.Void EventDispatcher.EventExtension.Dispatch<byte>(EventDispatcher.Event,int,byte)
		// System.Void EventDispatcher.EventExtension.Dispatch<int,int,int>(EventDispatcher.Event,int,int,int,int)
		// System.Void EventDispatcher.EventExtension.Dispatch<int,int,object>(EventDispatcher.Event,int,int,int,object)
		// System.Void EventDispatcher.EventExtension.Dispatch<int,int>(EventDispatcher.Event,int,int,int)
		// System.Void EventDispatcher.EventExtension.Dispatch<int,long>(EventDispatcher.Event,int,int,long)
		// System.Void EventDispatcher.EventExtension.Dispatch<int,object>(EventDispatcher.Event,int,int,object)
		// System.Void EventDispatcher.EventExtension.Dispatch<int>(EventDispatcher.Event,int,int)
		// System.Void EventDispatcher.EventExtension.Dispatch<long>(EventDispatcher.Event,int,long)
		// System.Void EventDispatcher.EventExtension.Dispatch<object,object,long>(EventDispatcher.Event,int,object,object,long)
		// System.Void EventDispatcher.EventExtension.Dispatch<object,object>(EventDispatcher.Event,int,object,object)
		// System.Void EventDispatcher.EventExtension.Dispatch<object>(EventDispatcher.Event,int,object)
		// System.Void EventDispatcher.EventExtension.RemoveEventListener<byte>(EventDispatcher.Event,int,System.Action<byte>)
		// System.Void EventDispatcher.EventExtension.RemoveEventListener<int,int,int>(EventDispatcher.Event,int,System.Action<int,int,int>)
		// System.Void EventDispatcher.EventExtension.RemoveEventListener<int,int,object>(EventDispatcher.Event,int,System.Action<int,int,object>)
		// System.Void EventDispatcher.EventExtension.RemoveEventListener<int,int>(EventDispatcher.Event,int,System.Action<int,int>)
		// System.Void EventDispatcher.EventExtension.RemoveEventListener<int,long>(EventDispatcher.Event,int,System.Action<int,long>)
		// System.Void EventDispatcher.EventExtension.RemoveEventListener<int,object>(EventDispatcher.Event,int,System.Action<int,object>)
		// System.Void EventDispatcher.EventExtension.RemoveEventListener<int>(EventDispatcher.Event,int,System.Action<int>)
		// System.Void EventDispatcher.EventExtension.RemoveEventListener<long>(EventDispatcher.Event,int,System.Action<long>)
		// System.Void EventDispatcher.EventExtension.RemoveEventListener<object,object,long>(EventDispatcher.Event,int,System.Action<object,object,long>)
		// System.Void EventDispatcher.EventExtension.RemoveEventListener<object,object>(EventDispatcher.Event,int,System.Action<object,object>)
		// System.Void EventDispatcher.EventExtension.RemoveEventListener<object>(EventDispatcher.Event,int,System.Action<object>)
		// bool EventMgr.AddEventListener<byte>(string,System.Action<byte>)
		// bool EventMgr.AddEventListener<int,int,int>(string,System.Action<int,int,int>)
		// bool EventMgr.AddEventListener<int,int,object>(string,System.Action<int,int,object>)
		// bool EventMgr.AddEventListener<int,int>(string,System.Action<int,int>)
		// bool EventMgr.AddEventListener<int,long>(string,System.Action<int,long>)
		// bool EventMgr.AddEventListener<int,object>(string,System.Action<int,object>)
		// bool EventMgr.AddEventListener<int>(string,System.Action<int>)
		// bool EventMgr.AddEventListener<long>(string,System.Action<long>)
		// bool EventMgr.AddEventListener<object,object,long>(string,System.Action<object,object,long>)
		// bool EventMgr.AddEventListener<object,object>(string,System.Action<object,object>)
		// bool EventMgr.AddEventListener<object>(string,System.Action<object>)
		// System.Void EventMgr.Dispatch<byte>(string,byte)
		// System.Void EventMgr.Dispatch<int,int,int>(string,int,int,int)
		// System.Void EventMgr.Dispatch<int,int,object>(string,int,int,object)
		// System.Void EventMgr.Dispatch<int,int>(string,int,int)
		// System.Void EventMgr.Dispatch<int,long>(string,int,long)
		// System.Void EventMgr.Dispatch<int,object>(string,int,object)
		// System.Void EventMgr.Dispatch<int>(string,int)
		// System.Void EventMgr.Dispatch<long>(string,long)
		// System.Void EventMgr.Dispatch<object,object,long>(string,object,object,long)
		// System.Void EventMgr.Dispatch<object,object>(string,object,object)
		// System.Void EventMgr.Dispatch<object>(string,object)
		// System.Void EventMgr.RemoveEventListener<byte>(string,System.Action<byte>)
		// System.Void EventMgr.RemoveEventListener<int,int,int>(string,System.Action<int,int,int>)
		// System.Void EventMgr.RemoveEventListener<int,int,object>(string,System.Action<int,int,object>)
		// System.Void EventMgr.RemoveEventListener<int,int>(string,System.Action<int,int>)
		// System.Void EventMgr.RemoveEventListener<int,long>(string,System.Action<int,long>)
		// System.Void EventMgr.RemoveEventListener<int,object>(string,System.Action<int,object>)
		// System.Void EventMgr.RemoveEventListener<int>(string,System.Action<int>)
		// System.Void EventMgr.RemoveEventListener<long>(string,System.Action<long>)
		// System.Void EventMgr.RemoveEventListener<object,object,long>(string,System.Action<object,object,long>)
		// System.Void EventMgr.RemoveEventListener<object,object>(string,System.Action<object,object>)
		// System.Void EventMgr.RemoveEventListener<object>(string,System.Action<object>)
		// bool GameConfig.GetValue<byte>(string,byte&,byte)
		// bool GameConfig.GetValue<int>(string,int&,int)
		// byte GameVersionHelper.GetValue<byte>(string,byte)
		// int GameVersionHelper.GetValue<int>(string,int)
		// object Nino.Serialization.Deserializer.Deserialize<object>(System.Span<byte>,Nino.Serialization.Reader,bool)
		// object Nino.Serialization.Deserializer.Deserialize<object>(byte[])
		// bool Nino.Serialization.Deserializer.TryDeserializeCodeGenType<object>(System.Type,Nino.Serialization.Reader,bool,bool,object&)
		// bool Nino.Serialization.Deserializer.TryDeserializeWrapperType<object>(System.Type,Nino.Serialization.Reader,bool,bool,object&)
		// System.Void Nino.Serialization.Reader.Read<double>(double&,int)
		// System.Void Nino.Serialization.Reader.Read<int>(int&,int)
		// object[] Nino.Serialization.Reader.ReadArray<object>()
		// object Nino.Serialization.Reader.ReadCommonVal<object>()
		// int Nino.Serialization.Serializer.GetSize<double>(double&,System.Collections.Generic.Dictionary<System.Reflection.MemberInfo,object>)
		// int Nino.Serialization.Serializer.GetSize<int>(int&,System.Collections.Generic.Dictionary<System.Reflection.MemberInfo,object>)
		// int Nino.Serialization.Serializer.GetSize<object>(object&,System.Collections.Generic.Dictionary<System.Reflection.MemberInfo,object>)
		// int Nino.Serialization.Serializer.Serialize<object>(System.Type,object,System.Collections.Generic.Dictionary<System.Reflection.MemberInfo,object>,System.Span<byte>,int)
		// bool Nino.Serialization.Serializer.TrySerializeArray<object>(object&,Nino.Serialization.Writer&)
		// bool Nino.Serialization.Serializer.TrySerializeCodeGenType<object>(System.Type,object&,Nino.Serialization.Writer&)
		// bool Nino.Serialization.Serializer.TrySerializeDict<object>(object&,Nino.Serialization.Writer&)
		// bool Nino.Serialization.Serializer.TrySerializeEnumType<object>(System.Type,object&,Nino.Serialization.Writer&)
		// bool Nino.Serialization.Serializer.TrySerializeList<object>(object&,Nino.Serialization.Writer&)
		// bool Nino.Serialization.Serializer.TrySerializeWrapperType<object>(System.Type,object&,Nino.Serialization.Writer&)
		// System.Void Nino.Serialization.Writer.Write<double>(double&,int)
		// System.Void Nino.Serialization.Writer.Write<int>(int&,int)
		// System.Void Nino.Serialization.Writer.Write<object>(object[])
		// System.Void Nino.Serialization.Writer.WriteCommonVal<object>(System.Type,object)
		// System.Void Nino.Serialization.Writer.WriteEnum<object>(object)
		// byte ServerVersion.GetValue<byte>(string,byte)
		// int ServerVersion.GetValue<int>(string,int)
		// object Sirenix.Utilities.MemberInfoExtensions.GetAttribute<object>(System.Reflection.ICustomAttributeProvider)
		// object Sirenix.Utilities.MemberInfoExtensions.GetAttribute<object>(System.Reflection.ICustomAttributeProvider,bool)
		// System.Collections.Generic.IEnumerable<object> Sirenix.Utilities.MemberInfoExtensions.GetAttributes<object>(System.Reflection.ICustomAttributeProvider,bool)
		// object System.Activator.CreateInstance<object>()
		// object[] System.Array.Empty<object>()
		// int System.Array.IndexOf<object>(object[],object,int,int)
		// int System.Array.IndexOfImpl<object>(object[],object,int,int)
		// System.Void System.Array.Resize<byte>(byte[]&,int)
		// System.Void System.Array.Resize<object>(object[]&,int)
		// System.Collections.Concurrent.OrderablePartitioner<object> System.Collections.Concurrent.Partitioner.Create<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Concurrent.OrderablePartitioner<object> System.Collections.Concurrent.Partitioner.Create<object>(System.Collections.Generic.IEnumerable<object>,System.Collections.Concurrent.EnumerablePartitionerOptions)
		// bool System.Linq.Enumerable.Any<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Cast<object>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.CastIterator<object>(System.Collections.IEnumerable)
		// int System.Linq.Enumerable.Count<HotUpdateScripts.KingDataCfg>(System.Collections.Generic.IEnumerable<HotUpdateScripts.KingDataCfg>)
		// System.Collections.Generic.KeyValuePair<int,DigitalRubyShared.GestureTouch>[] System.Linq.Enumerable.ToArray<System.Collections.Generic.KeyValuePair<int,DigitalRubyShared.GestureTouch>>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,DigitalRubyShared.GestureTouch>>)
		// System.Collections.Generic.KeyValuePair<object,object>[] System.Linq.Enumerable.ToArray<System.Collections.Generic.KeyValuePair<object,object>>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>)
		// object[] System.Linq.Enumerable.ToArray<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.List<object> System.Linq.Enumerable.ToList<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,LoadSceneNode.<LoadScene>d__4>(Cysharp.Threading.Tasks.UniTask.Awaiter&,LoadSceneNode.<LoadScene>d__4&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<LoadSceneNode.<LoadScene>d__4>(LoadSceneNode.<LoadScene>d__4&)
		// byte& System.Runtime.CompilerServices.Unsafe.AddByteOffset<byte>(byte&,System.IntPtr)
		// byte& System.Runtime.CompilerServices.Unsafe.AsRef<byte>(System.Void*)
		// double System.Runtime.CompilerServices.Unsafe.ReadUnaligned<double>(byte&)
		// int System.Runtime.CompilerServices.Unsafe.ReadUnaligned<int>(byte&)
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<double>()
		// int System.Runtime.CompilerServices.Unsafe.SizeOf<int>()
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<double>(byte&,double)
		// System.Void System.Runtime.CompilerServices.Unsafe.WriteUnaligned<int>(byte&,int)
		// byte& System.Runtime.InteropServices.MemoryMarshal.GetReference<byte>(System.ReadOnlySpan<byte>)
		// double System.Runtime.InteropServices.MemoryMarshal.Read<double>(System.ReadOnlySpan<byte>)
		// int System.Runtime.InteropServices.MemoryMarshal.Read<int>(System.ReadOnlySpan<byte>)
		// bool System.SpanHelpers.IsReferenceOrContainsReferences<double>()
		// bool System.SpanHelpers.IsReferenceOrContainsReferences<int>()
		// object System.Threading.Interlocked.CompareExchange<object>(object&,object,object)
		// System.Threading.Tasks.ParallelLoopResult System.Threading.Tasks.Parallel.ForEach<object>(System.Collections.Generic.IEnumerable<object>,System.Action<object>)
		// System.Threading.Tasks.ParallelLoopResult System.Threading.Tasks.Parallel.ForEachWorker<object,object>(System.Collections.Generic.IEnumerable<object>,System.Threading.Tasks.ParallelOptions,System.Action<object>,System.Action<object,System.Threading.Tasks.ParallelLoopState>,System.Action<object,System.Threading.Tasks.ParallelLoopState,long>,System.Func<object,System.Threading.Tasks.ParallelLoopState,object,object>,System.Func<object,System.Threading.Tasks.ParallelLoopState,long,object,object>,System.Func<object>,System.Action<object>)
		// System.Threading.Tasks.ParallelLoopResult System.Threading.Tasks.Parallel.ForEachWorker<object,object>(System.Collections.Generic.IList<object>,System.Threading.Tasks.ParallelOptions,System.Action<object>,System.Action<object,System.Threading.Tasks.ParallelLoopState>,System.Action<object,System.Threading.Tasks.ParallelLoopState,long>,System.Func<object,System.Threading.Tasks.ParallelLoopState,object,object>,System.Func<object,System.Threading.Tasks.ParallelLoopState,long,object,object>,System.Func<object>,System.Action<object>)
		// System.Threading.Tasks.ParallelLoopResult System.Threading.Tasks.Parallel.ForEachWorker<object,object>(object[],System.Threading.Tasks.ParallelOptions,System.Action<object>,System.Action<object,System.Threading.Tasks.ParallelLoopState>,System.Action<object,System.Threading.Tasks.ParallelLoopState,long>,System.Func<object,System.Threading.Tasks.ParallelLoopState,object,object>,System.Func<object,System.Threading.Tasks.ParallelLoopState,long,object,object>,System.Func<object>,System.Action<object>)
		// System.Threading.Tasks.ParallelLoopResult System.Threading.Tasks.Parallel.ForWorker<object>(int,int,System.Threading.Tasks.ParallelOptions,System.Action<int>,System.Action<int,System.Threading.Tasks.ParallelLoopState>,System.Func<int,System.Threading.Tasks.ParallelLoopState,object,object>,System.Func<object>,System.Action<object>)
		// System.Threading.Tasks.ParallelLoopResult System.Threading.Tasks.Parallel.PartitionerForEachWorker<object,object>(System.Collections.Concurrent.Partitioner<object>,System.Threading.Tasks.ParallelOptions,System.Action<object>,System.Action<object,System.Threading.Tasks.ParallelLoopState>,System.Action<object,System.Threading.Tasks.ParallelLoopState,long>,System.Func<object,System.Threading.Tasks.ParallelLoopState,object,object>,System.Func<object,System.Threading.Tasks.ParallelLoopState,long,object,object>,System.Func<object>,System.Action<object>)
		// System.Threading.Tasks.Task<object> System.Threading.Tasks.Task.Run<object>(System.Func<object>,System.Threading.CancellationToken)
		// byte UnityEngine.AndroidJNIHelper.ConvertFromJNIArray<byte>(System.IntPtr)
		// System.IntPtr UnityEngine.AndroidJNIHelper.GetMethodID<byte>(System.IntPtr,string,object[],bool)
		// byte UnityEngine.AndroidJavaObject.Call<byte>(string,object[])
		// byte UnityEngine.AndroidJavaObject.FromJavaArrayDeleteLocalRef<byte>(System.IntPtr)
		// byte UnityEngine.AndroidJavaObject._Call<byte>(string,object[])
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInParent<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// bool UnityEngine.EventSystems.ExecuteEvents.Execute<object>(UnityEngine.GameObject,UnityEngine.EventSystems.BaseEventData,UnityEngine.EventSystems.ExecuteEvents.EventFunction<object>)
		// System.Void UnityEngine.EventSystems.ExecuteEvents.GetEventList<object>(UnityEngine.GameObject,System.Collections.Generic.IList<UnityEngine.EventSystems.IEventSystemHandler>)
		// bool UnityEngine.EventSystems.ExecuteEvents.ShouldSendToComponent<object>(UnityEngine.Component)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// System.Void UnityEngine.GameObject.GetComponents<object>(System.Collections.Generic.List<object>)
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// object UnityEngine.Object.FindObjectOfType<object>()
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Transform)
		// byte UnityEngine._AndroidJNIHelper.ConvertFromJNIArray<byte>(System.IntPtr)
		// System.IntPtr UnityEngine._AndroidJNIHelper.GetMethodID<byte>(System.IntPtr,string,object[],bool)
		// string UnityEngine._AndroidJNIHelper.GetSignature<byte>(object[])
		// object YooAsset.AssetOperationHandle.GetAssetObject<object>()
		// YooAsset.AssetOperationHandle YooAsset.ResourcePackage.LoadAssetAsync<object>(string)
		// YooAsset.AssetOperationHandle YooAsset.ResourcePackage.LoadAssetSync<object>(string)
		// YooAsset.AssetOperationHandle YooAsset.YooAssets.LoadAssetAsync<object>(string)
		// YooAsset.AssetOperationHandle YooAsset.YooAssets.LoadAssetSync<object>(string)
	}
}