﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
public class GameSettings : MonoBehaviour
{

    public static GameSettings GM;
    public static Texture2D HeightMap;
    public static EntityArchetype BlockArchetype;

    public int chunkBase = 1;
    public Mesh blockMesh;
    public Mesh surfaceMesh;
    public Mesh tallGrassMesh;

    public Material stoneMaterial;
    public Material woodMaterial;
    public Material leaveMaterial;
    public Material surfaceMaterial;
    public Material cobbleMaterial;
    public Material dirtMaterial;
    public Material tallGrassMaterial;
    public Material roseMaterial;
    public Material CloundMaterial;

    public Material glassMaterial;
    public Material brickMaterial;
    public Material plankMaterial;
    public Material tntMaterial;

    public Material pinkMaterial;

    public bool createCollider;
    int ranDice;
    Material maTemp;
    Mesh meshTemp;
    // Use this for initialization
    public EntityManager manager;
    public Entity entities;
    void Awake()
    {
        if (GM != null && GM != this)
        {
            Destroy(gameObject);
        }
        else
        {
            GM = this;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        EntityManager manager = World.Active.GetOrCreateManager<EntityManager>();
        BlockArchetype = manager.CreateArchetype(
            typeof(Position)
        );
    }


    void Start()
    {
        manager = World.Active.GetOrCreateManager<EntityManager>();
        ChunkGenerator(chunkBase);
    }


    void ChunkGenerator(int amount)
    {
        int totalmount = (amount * amount) * 1500;
        int hightlevel;
        bool airChecker;
        for (int yBlock = 0; yBlock < 15; yBlock++)
        {
            for (int xBlock = 0; xBlock < 10 * amount; xBlock++)
            {
                for (int zBlock = 0; zBlock < 10 * amount; zBlock++)
                {
                    hightlevel = (int)(Heightmap.GetPixel(xBlock, zBlock).r * 100) - yBlock;
                    airChecker = false;
                    Vector3 posTemp = new Vector3(xBlock, yBlock, zBlock);
                    switch (hightlevel)
                    {
                        case 0:
                            ranDice = UnityEngine.Random.Range(1, 201);
                            if (ranDice <= 20)
                            {
                                PlantGenerator(xBlock, yBlock, zBlock, 1);
                            }
                            else if (ranDice == 198)
                            {
                                CloundGenerator(xBlock, yBlock, zBlock);
                            }
                            else if (ranDice == 200)
                            {
                                PlantGenerator(xBlock, yBlock, zBlock, 2);
                            }
                            else
                            {
                                TreeGenerator(xBlock,yBlock, zBlock);
                            }
                            airChecker = true;
                            break;
                        case 1:
                            meshTemp = surfaceMesh;
                            maTemp = surfaceMaterial;
                            break;
                        case 2:
                        case 3:
                        case 4:
                            meshTemp = blockMesh;
                            maTemp = dirtMaterial;
                            break;
                        case 5:
                        case 6:
                            meshTemp = blockMesh;
                            maTemp = stoneMaterial;
                            break;
                        case 7:
                        case 8:
                            meshTemp = blockMesh;
                            maTemp = cobbleMaterial;
                            break;
                        default:
                            airChecker = true;
                            break;
                    }
                    if (!airChecker)
                    {
						if(!maTemp){
							maTemp = pinkMaterial;
						}			
						AddCollider(posTemp);
                        Entity entity = manager.CreateEntity(BlockArchetype);
                        manager.SetComponentData(entities, new Position { Value = new int3(xBlock, yBlock, zBlock) });
                        manager.AddComponentData(entities, BlockTag{ });
       					manager.AddComponentData(entities, new MeshInstanceRenderer()
						{
            				Mesh = blockMesh,
            				Material = maTemp
        				});
    				}
				}
            }
        }
    }
	void TreeGenerator(int x,int y,int z){
		for(int i = y;i<y+7;i++){
			if(i== y + 6){
				maTemp = leavesMaterial;
			}else{
				maTemp = woodMaterial;
			}

			if(!maTemp)
				maTemp = pinkMaterial;
			Vector3 posTemp = new Vector3(x,i,z);
			AddCollider(posTemp);
			Entity entity = manager.CreateEntity(BlockArchetype);
			manager.SetComponentData(entities, new Position { Value = new int3(xBlock, yBlock, zBlock) });
			manager.AddComponentData(entities, BlockTag{ });
			manager.AddComponentData(entities, new MeshInstanceRenderer
			{
				Mesh = blockMesh,
				Material = maTemp
			});
		}
	}

	void AddCollider(Vector3 posTemp){
		if(createCollider){
			GM.GetComponent<ColliderPool>().AddCollider(posTemp);
		}
	}

	void CloundGenerator(int x ,int y,int z){
		meshTemp = blockMesh;
		maTemp = CloundMaterial;
		if(!maTemp){
			maTemp = pinkMaterial;
		}
	}
	
	// Update is called once per frame
	void Update()
{

}
}