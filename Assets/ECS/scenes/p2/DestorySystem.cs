using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
public class DestorySystem : ComponentSystem
{
    struct BlockGroup
    {	
		[ReadOnly]
        public readonly int length;
		[ReadOnly]
        public EntityArray entityArray;
		[ReadOnly]
        public ComponentDataArray<Position> positions;
		[ReadOnly]
        public ComponentDataArray<BlockTag> tags;
    }

    struct DestoryBlockGroup
    {
		[ReadOnly]
        public readonly int length;
		[ReadOnly]
        public EntityArray entityArray;
		[ReadOnly]
        public ComponentDataArray<Position> positions;
		[ReadOnly]
        public ComponentDataArray<DestoryTag> tags;
    }
    struct SurfacePlantGroup
    {
		[ReadOnly]
        public readonly int length;
		[ReadOnly]
        public EntityArray entityArray;
		[ReadOnly]
        public ComponentDataArray<Position> positions; 
		[ReadOnly]
		public ComponentDataArray<SurfacePlantTag> tags;
    }

    [Inject] BlockGroup targetBlocks;
    [Inject] DestoryBlockGroup sourceBlocks;
    [Inject] SurfacePlantGroup surfacePlants;
    protected override void OnUpdate()
    {
        for (int i = 0; i < sourceBlocks.length; i++)
        {
            for (int j = 0; j < targetBlocks.length; j++)
            {
                Vector3 offset = targetBlocks.positions[j].Value - sourceBlocks.positions[i].Value;
                float sqrLen = offset.sqrMagnitude;
                if (sqrLen == 0)//选中方块
                {
                    for (int k = 0; k < surfacePlants.length; k++)
                    {
                        float3 tmp = new float3(surfacePlants.positions[k].Value.x, surfacePlants.positions[k].Value.y, surfacePlants.positions[k].Value.z);
                        offset = targetBlocks.positions[j].Value - tmp;
                        sqrLen = offset.sqrMagnitude;
                        if (sqrLen == 0)
                        {
                            PostUpdateCommands.DestroyEntity(surfacePlants.entityArray[k]);
                        }
                    }
                    PostUpdateCommands.DestroyEntity(sourceBlocks.entityArray[i]);
                    PostUpdateCommands.DestroyEntity(targetBlocks.entityArray[j]);
                }
            }
        }
    }

}
