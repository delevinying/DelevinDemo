using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Rendering;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
public class Demo3 : MonoBehaviour {
	public static EntityArchetype entityArchetype;
	public EntityManager entityManager;
	public Mesh mesh;
	public Material material;
	public Entity entities;

	public int starNum;
	public int plane;
	
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void initialization(){
		EntityManager _entityManager = World.Active.GetOrCreateManager<EntityManager>();
		entityArchetype = _entityManager.CreateArchetype(
			typeof(Position)
		);
	}
	// Use this for initialization
	// private Entity entities;
	void Start () {
		entityManager = World.Active.GetOrCreateManager<EntityManager>();
		for(int i=0;i<starNum;i++){
			entities = entityManager.CreateEntity(entityArchetype);
			entityManager.SetComponentData(entities,new Position{Value = new int3(i*3,0,0)});
			entityManager.AddComponentData(entities,new Rotator{speed = i});
			// entityManager.AddComponent(entities, r);
			entityManager.AddSharedComponentData(entities,new MeshInstanceRenderer{
				mesh = mesh,
				material = material
			});
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
