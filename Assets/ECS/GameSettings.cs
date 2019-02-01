using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;


using Unity.Rendering;

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

    // private PerlinNoiseGenerator perlin;
    void Start()
    {
        manager = World.Active.GetOrCreateManager<EntityManager>();
        // perlin = new PerlinNoiseGenerator();
        // HeightMap = perlin.GenerateHeightMap();
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
                                TreeGenerator(xBlock, yBlock, zBlock);
                            }
                            airChecker = true;
                            break;
                        case 1:
                             //meshTemp = surfaceMesh;
                             //maTemp = surfaceMaterial;
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
                        if (!maTemp)
                        {
                            maTemp = pinkMaterial;
                        }
                        AddCollider(posTemp);
                        Entity entities = manager.CreateEntity(BlockArchetype);
                        manager.SetComponentData(entities, new Position { Value = new int3(xBlock, yBlock, zBlock) });
                        manager.AddComponentData(entities, new BlockTag { });
                        manager.AddSharedComponentData(entities, new MeshInstanceRenderer
                        {
                            mesh = blockMesh,
                            material = maTemp
                        });
                    }
                }
            }
        }
    }


    void PlantGenerator(int x, int y, int z, int n)
    {
        if (n == 1)
        {
            maTemp = tallGrassMaterial;
        }
        else
        {
            maTemp = roseMaterial;
        }

        if (!maTemp)
        {
            maTemp = pinkMaterial;
        }

        Quaternion rotation = Quaternion.Euler(0, 45, 0);
        Entity entities = manager.CreateEntity(BlockArchetype);
        manager.SetComponentData(entities, new Position { Value = new int3(x, y, z) });
        manager.AddComponentData(entities, new Rotation { Value = rotation });
        manager.AddComponentData(entities, new SurfacePlantTag { });
        manager.AddSharedComponentData(entities, new MeshInstanceRenderer
        {//Shared
            mesh = tallGrassMesh,
            material = maTemp
        });
    }
    void TreeGenerator(int x, int y, int z)
    {
        for (int i = y; i < y + 7; i++)
        {
            if (i == y + 6)
            {
                maTemp = leaveMaterial;//leavesMaterial;
            }
            else
            {
                maTemp = woodMaterial;
            }

            if (!maTemp)
                maTemp = pinkMaterial;
            Vector3 posTemp = new Vector3(x, i, z);
            AddCollider(posTemp);
            Entity entities = manager.CreateEntity(BlockArchetype);
            manager.SetComponentData(entities, new Position { Value = new int3(x, y, z) });
            manager.AddComponentData(entities, new BlockTag { });
            manager.AddSharedComponentData(entities, new MeshInstanceRenderer
            {
                mesh = blockMesh,
                material = maTemp
            });
            if(i >= y+3 && i<= y+6){
                for(int j = x -1;j<=x+1;j++){
                    for(int k = z-1;k<=z+1;k++){
                        if(k!=z||j!=x){
                            posTemp = new Vector3(j,i,k);
                            AddCollider(posTemp);
                            entities = manager.CreateEntity(BlockArchetype);
                            manager.AddComponentData(entities,new BlockTag{});
                            manager.AddSharedComponentData(entities,new MeshInstanceRenderer{
                                mesh = blockMesh,
                                material = leaveMaterial
                            });
                        }
                    }
                }
            }
        }
    }

    void AddCollider(Vector3 posTemp)
    {
        if (createCollider)
        {
            GM.GetComponent<ColliderPool>().AddCollider(posTemp);
        }
    }

    void CloundGenerator(int x, int y, int z)
    {
        meshTemp = blockMesh;
        maTemp = CloundMaterial;
        if (!maTemp)
        {
            maTemp = pinkMaterial;
        }
        ranDice = UnityEngine.Random.Range(4, 7);
        for (int i = 0; i < ranDice; i++)
        {
            for (int j = 0; j < ranDice; j++)
            {
                Entity entities = manager.CreateEntity(BlockArchetype);
                manager.SetComponentData(entities, new Position{ Value = new int3(x + 1, y + 15 , z + j) });
                manager.AddSharedComponentData(entities, new MeshInstanceRenderer
                {
                    mesh = meshTemp,
                    material = maTemp
                });
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
