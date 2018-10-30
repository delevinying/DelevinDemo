// using System.Collections.Generic;
// using System.Linq;
// // using Data;
// using Unity.Entities;
// using Unity.Mathematics;
// using UnityEngine;

// public class PlanetSpawner : MonoBehaviour
// {
//     /// <summary>
//     /// 星球预制体
//     /// </summary>
//     [SerializeField]
//     GameObject _planetPrefab;
//     /// <summary>
//     /// 初始化星球的个数
//     /// </summary>
//     [SerializeField]
//     int _initialCount = 20;
//     /// <summary>
//     /// 产生星球的随机半径
//     /// </summary>
//     [SerializeField] readonly float radius = 100.00f;

//     /// <summary>
//     /// 场景中所有的Entity的控制者、容器
//     /// </summary>
//     EntityManager _entityManager;
//     /// <summary>
//     /// 灰色 红色 绿色对应材质数组
//     /// </summary>
//     [SerializeField]
//     public Material[] _teamMaterials;
//     /// <summary>
//     /// 飞船Entity对应的GameObject 的字典
//     /// </summary>
//     static Dictionary<Entity, GameObject> entities = new Dictionary<Entity, GameObject>();

//     public static Material[] TeamMaterials;

//     void Awake()
//     {
//         TeamMaterials = _teamMaterials;
//     }

//     void OnEnable()
//     {
//         _entityManager = World.Active.GetOrCreateManager<EntityManager>();
//         //初始化
//         Instantiate(_initialCount);
//     }
//     /// <summary>
//     /// 初始化
//     /// </summary>
//     /// <param name="count">产生星球的数量</param>
//     void Instantiate(int count)
//     {
//         //产生飞船队伍列表 1绿色 2 红色
//         var planetOwnership = new List<int>
//         {
//             1, 1,
//             2, 2
//         };

//         for (var i = 0; i < count; ++i)
//         {

//             //获取星球对应的半径
//             var sphereRadius = UnityEngine.Random.Range(5.0f, 20.0f);

//             var safe = false;

//             float3 pos;

//             int attempts = 0;
//             do
//             {
//                 if (++attempts >= 500)
//                 {
//                     Debug.Log("新创建的行星找不到合适的位置");
//                     return;
//                 }
//                 //在半径为1的范围内返回一个随机点（只读）
//                 var randomValue = (Vector3)UnityEngine.Random.insideUnitSphere;
//                 randomValue.y = 0;
//                 //星球的实际位置
//                 pos = (randomValue * radius) + new Vector3(transform.position.x, transform.position.z);
//                 //检测星球是否有重合的物体
//                 var collisions = Physics.OverlapSphere(pos, sphereRadius);
//                 //如果没有重合的地方就是安全地
//                 if (!collisions.Any())
//                     safe = true;
//             } while (!safe);

//             //在半径为1的范围内返回一个随机点（只读）
//             var randomRotation = UnityEngine.Random.insideUnitSphere;
//             //实例化星球
//             var go = GameObject.Instantiate(_planetPrefab, pos, quaternion.identity);
//             go.name = "Sphere_" + i;
//             //获取星球上对应的 GameObjectEntity
//             var planetEntity = go.GetComponent<GameObjectEntity>().Entity;
//             //获取渲染星球的对应的子物体
//             var meshGo = go.GetComponentsInChildren<Transform>().First(c => c.gameObject != go).gameObject;
//             //获取碰撞体
//             var collider = go.GetComponent<SphereCollider>();
//             //获取渲染星球的对应的子物体的 GameObjectEntity
//             var meshEntity = meshGo.GetComponent<GameObjectEntity>().Entity;
//             //把碰撞体的半径设置和圆球一直
//             collider.radius = sphereRadius;
//             //半径*2等于实际扩大的倍数
//             meshGo.transform.localScale = new Vector3(sphereRadius * 2.0f, sphereRadius * 2.0f, sphereRadius * 2.0f);

//             var planetData = new PlanetData
//             {
//                 //星球所在的队伍
//                 TeamOwnership = 0,
//                 //星球的半径
//                 Radius = sphereRadius,
//                 //星球的位置
//                 Position = pos
//             };
//             var rotationData = new RotationData
//             {
//                 RotationSpeed = randomRotation
//             };
//             //队伍列表是否有任何元素 没有元素的划分为灰色星球
//             if (planetOwnership.Any())
//             {
//                 //给星球分队 【红队或者绿色队伍】
//                 planetData.TeamOwnership = planetOwnership.First();
//                 //移除对应的队伍
//                 planetOwnership.Remove(planetData.TeamOwnership);
//             }
//             else
//             {
//                 //设定飞船数量
//                 planetData.Occupants = UnityEngine.Random.Range(1, 100);
//             }
//             //设置字典对应的GameObject
//             entities[planetEntity] = go;
//             //设置对于队伍的颜色 1绿色 2红色
//             SetColor(planetEntity, planetData.TeamOwnership);
//             //动态添加对应的数据 减少了拖拖拽拽
//             _entityManager.AddComponentData(planetEntity, planetData);
//             _entityManager.AddComponentData(meshEntity, rotationData);
//         }
//     }
//     /// <summary>
//     /// 设置对应星球的颜色
//     /// </summary>
//     /// <param name="entity">对应字典</param>
//     /// <param name="team"></param>
//     public static void SetColor(Entity entity, int team)
//     {
//         var go = entities[entity];
//         go.GetComponentsInChildren<MeshRenderer>().First(c => c.gameObject != go).material = TeamMaterials[team];
//     }
// }

