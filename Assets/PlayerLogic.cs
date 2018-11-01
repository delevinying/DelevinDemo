using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerLogic : MonoBehaviour
{

    public LayerMask blockLayer;
    GameObject player;
    public static Transform BlockToDestory;
    Material BlcokToPlace;
    public static int blockID = 1;

    public AudioClip grass_audio;
    public AudioClip stone_audio;
    public AudioClip dirt_audio;
    public AudioClip wood_audio;

    AudioSource AS;

    public ParticleSystem digEffect;

    EntityManager entityManager;
    // Use this for initialization
    void Start()
    {
        AS = transform.GetComponent<AudioSource>();
        player = this.transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        entityManager = World.Active.GetOrCreateManager<EntityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blockID > 7)
        {
            blockID = 1;
        }
        if (blockID < 1)
        {
            blockID = 7;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            blockID++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            blockID--;
        }

        switch (blockID)
        {
            case 1:
                BlcokToPlace = GameSettings.GM.stoneMaterial;
                break;
            case 2:
                BlcokToPlace = GameSettings.GM.roseMaterial;
                break;
			case 3:
                BlcokToPlace = GameSettings.GM.glassMaterial;
                break;
            case 4:
                BlcokToPlace = GameSettings.GM.woodMaterial;
                break;
			case 5:
                BlcokToPlace = GameSettings.GM.cobbleMaterial;
                break;
            case 6:
                BlcokToPlace = GameSettings.GM.tntMaterial;
                break;
			case 7:
                BlcokToPlace = GameSettings.GM.brickMaterial;
                break;
        }

		if(Input.GetMouseButtonDown(1)){
			placeBlock(BlcokToPlace);
		}

		if(Input.GetMouseButtonDown(0)){
			DestoryBlock();
		}
    }

	void placeBlock(Material material){
		RaycastHit hitInfo;
		Physics.Raycast(transform.position,transform.forward,out hitInfo,7,blockLayer);
		if(hitInfo.transform!=null){
			Entity entity = entityManager.CreateEntity(GameSettings.BlockArchetype);
			entityManager.SetComponentData(entity,new Position{Value = hitInfo.transform.position+hitInfo.normal});
			entityManager.AddComponentData(entity,new BlockTag{});
			entityManager.AddSharedComponentData(entity,new MeshInstanceRenderer{
				mesh = GameSettings.GM.blockMesh,
				material = material
			});
			Position pos = new Position{Value = (hitInfo.transform.position+hitInfo.normal)};
			ColliderPool.CP.AddCollider(pos.Value);
		}
	}

	void DestoryBlock(){
		RaycastHit hitInfo;
		Physics.Raycast(transform.position,transform.forward,out hitInfo,7,blockLayer);
		if(hitInfo.transform!=null){
			// entityManager.get
			Entity entity = entityManager.CreateEntity(GameSettings.BlockArchetype);
			entityManager.SetComponentData(entity,new Position{Value = hitInfo.transform.position+hitInfo.normal});
			entityManager.AddComponentData(entity,new DestoryTag{});
		}
	}
}
