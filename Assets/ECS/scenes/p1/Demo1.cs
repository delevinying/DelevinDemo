using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
public class Demo1 : MonoBehaviour {

	public static EntityArchetype entityArchetype;
	public Entity entity;
	public EntityManager entityManager;
	public Mesh mesh;
	public Material material;
	public int numEntities = 5;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void initialization(){
		EntityManager entityManager = World.Active.GetOrCreateManager<EntityManager>();
		entityArchetype = entityManager.CreateArchetype(
			typeof(Position)
		);
	}

	// Use this for initialization
	void Start () {
		 entityManager = World.Active.GetExistingManager<EntityManager>();
		
		for(int i = 0;i<numEntities;i++){
			entity = entityManager.CreateEntity(entityArchetype);
			entityManager.SetComponentData(entity,new Position{Value = new int3(i*2,0,0)});
			entityManager.AddComponentData(entity,new BlockTag{});
			entityManager.AddSharedComponentData(entity,new MeshInstanceRenderer{
				mesh = mesh,
				material = material
			});
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
