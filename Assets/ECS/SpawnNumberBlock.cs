using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
public class SpawnNumberBlock : MonoBehaviour
{
    
    public static Texture2D HeightMap;
    public static EntityArchetype BlockArchetype;

    [Header("chunkBase * chunkBase")]
    public int chunkBase = 1;

    public Mesh blockMesh;
    public Material no0;
    public Material no1;
    public Material no2;
    public Material no3;
    public Material no4;
    public Material no5;
    public Material no6;
    public Material no7;
    Material maTemp;
    public EntityManager manager;
    public Entity entities;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        EntityManager manager = World.Active.GetOrCreateManager<EntityManager>();
        BlockArchetype = manager.CreateArchetype(
            typeof(Position)
        );
    }
    
    // Use this for initialization
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
                    hightlevel = (int)(HeightMap.GetPixel(xBlock, zBlock).r * 100) - yBlock;
                    airChecker = false;
                    Vector3 posTemp = new Vector3(xBlock, yBlock, zBlock);
                    switch (hightlevel)
                    {
                        case 0:
                            maTemp = no0;
                            break;
                        case 1:
                            maTemp = no1;
                            break;
                        case 2:
                            maTemp = no2;
                            break;
                        case 3:
                            maTemp = no3;
                            break;
                        case 4:
                            maTemp = no4;
                            break;
                        case 5:
                            maTemp = no5;
                            break;
                        case 6:
                            maTemp = no6;
                            break;
                        case 7:
                            maTemp = no7;
                            break;
							default:
							maTemp = no0;
							airChecker = true;
							break;

                    }
					if(!airChecker){
						Entity entity = manager.CreateEntity(BlockArchetype);
						manager.SetComponentData(entities,new Position{Value = new int3(xBlock,yBlock,zBlock)});
						manager.AddComponentData(entities,new BlockTag{});
						manager.AddSharedComponentData(entities,new MeshInstanceRenderer{
							mesh = blockMesh,
							material = maTemp
						});
					}
                }
            }
        }
    }

	 

    // Update is called once per frame
    void Update()
    {

    }
}
