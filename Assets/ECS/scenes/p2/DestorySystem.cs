// using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public class DestorySystem : ComponentSystem {
    struct BlockGroup {
        [ReadOnly]
        public readonly int Length;
        [ReadOnly]
        public EntityArray entityArray;
        [ReadOnly]
        public ComponentDataArray<Position> positions;
        [ReadOnly]
        public ComponentDataArray<BlockTag> tags;
    }

    struct DestoryBlockGroup {
        [ReadOnly]
        public readonly int Length;
        [ReadOnly]
        public EntityArray entityArray;
        [ReadOnly]
        public ComponentDataArray<Position> positions;
        [ReadOnly]
        public ComponentDataArray<DestoryTag> tags;
    }
    struct SurfacePlantGroup {
        [ReadOnly]
        public readonly int Length;
        [ReadOnly]
        public EntityArray entityArray;
        [ReadOnly]
        public ComponentDataArray<Position> positions;
        [ReadOnly]
        public ComponentDataArray<SurfacePlantTag> tags;
    }
    private Transform objTransForms;
    private Vector3 objPosion;
    [Inject] BlockGroup targetBlocks;
    [Inject] DestoryBlockGroup sourceBlocks;
    [Inject] SurfacePlantGroup surfacePlants;
    protected override void OnUpdate () {
        if (this.objTransForms == null) {
            // this.transForms = GameObject.FindWithTag("Player").Transforms
            GameObject obj = GameObject.FindWithTag("Player");
            this.objTransForms = obj.transform;
        }
        this.objPosion = this.objTransForms.position;
        for (int n = 0; n < targetBlocks.Length; n++) {
            Vector3 v3 = new Vector3(targetBlocks.positions[n].Value.x,targetBlocks.positions[n].Value.y,
            targetBlocks.positions[n].Value.z);
            Vector3 temp = this.objPosion - v3;
            float sqrLen =  temp.magnitude;
            //targetBlocks.positions[n].Value.x+targetBlocks.positions[n].Value.z;
            //temp.sqrMagnitude;
            if (sqrLen > 80) {
                PostUpdateCommands.DestroyEntity (targetBlocks.entityArray[n]);
                Debug.Log ("销毁 --------  " + n);
            }
        }

        // Debug.Log("  OnUpdate   OnUpdate  OnUpdate  sourceBlocks.Length  "+sourceBlocks.Length+"    targetBlocks.Length  "+targetBlocks.Length);
        for (int i = 0; i < sourceBlocks.Length; i++) {
            // Debug.Log("  OnUpdate   i  "+i+"   ----- > "+sourceBlocks.positions[i].Value);
            for (int j = 0; j < targetBlocks.Length; j++) {
                // Debug.Log("  OnUpdate   OnUpd)
                Vector3 offset = targetBlocks.positions[j].Value - sourceBlocks.positions[i].Value;
                float sqrLen = offset.sqrMagnitude;
                // Debug.Log("  sqrLen  "+sqrLen);
                if (sqrLen == 0) //选中方块
                {
                    for (int k = 0; k < surfacePlants.Length; k++) {
                        float3 tmp = new float3 (surfacePlants.positions[k].Value.x, surfacePlants.positions[k].Value.y, surfacePlants.positions[k].Value.z);
                        offset = targetBlocks.positions[j].Value - tmp;
                        sqrLen = offset.sqrMagnitude;
                        if (sqrLen == 0) {
                            PostUpdateCommands.DestroyEntity (surfacePlants.entityArray[k]);
                        }
                    }
                    Debug.Log ("销毁  i " + i + "   j  " + j);
                    PostUpdateCommands.DestroyEntity (sourceBlocks.entityArray[i]);
                    PostUpdateCommands.DestroyEntity (targetBlocks.entityArray[j]);
                }
            }
        }
    }

}