using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;
public class SpawnOneBlock : MonoBehaviour {

	public static EntityArchetype BlockArchetype;
	[Header("Mesh Info")]
	public Mesh blockMesh;
	[Header("Nature Block Type")]
	public Material blockMaterial;
	public EntityManager manager;
	public Entity entities;
	public GameObject prefab_ref;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void Initialize(){
		EntityManager manager = World.Active.GetOrCreateManager<EntityManager>();
		BlockArchetype = manager.CreateArchetype(
			typeof(Position)
		);
	}

	// Use this for initialization
	//[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	void Start () {
		 manager = World.Active.GetOrCreateManager<EntityManager>();
		 entities = manager.CreateEntity(BlockArchetype);
		 manager.SetComponentData(entities,new Position{Value = new int3(2,0,0)});
		 manager.AddComponentData(entities,new BlockTag{ });
		 manager.AddSharedComponentData(entities,new MeshInstanceRenderer{
		 	mesh = blockMesh,
            material = blockMaterial
		 });
		 if(prefab_ref){
		 	NativeArray<Entity> entityArray = new NativeArray<Entity>(1,Allocator.Temp);
            manager.Instantiate(prefab_ref,entityArray);
		 	manager.SetComponentData(entityArray[0],new Position{Value = new float3(4,0f,0f)});
		 	entityArray.Dispose();
		 }
		 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

