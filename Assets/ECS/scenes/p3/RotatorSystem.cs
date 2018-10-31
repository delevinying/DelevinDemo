using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
public class RotatorSystem : JobComponentSystem
{
    [ComputeJobOptimization]
    struct RotationSpeedRotation : IJobProcessComponentData<Rotation, Rotator>
    {
        /// <summary>
        /// deltaTime
        /// </summary>
        public float dt;
        /// <summary>
        /// 实现接口，在Excute中实现旋转
        /// </summary>
        public void Execute(ref Rotation rotation, ref Rotator speed)
        {
            //读取speed，进行运算后，赋值给rotation
            rotation.Value = math.mul(math.normalize(rotation.Value), Quaternion.AxisAngle(math.up(), speed.speed * dt));
        }
    }

    /// <summary>
    /// 我们在这里，只需要声明我们将要用到那些job
    /// JobComponentSystem 携带以前定义的所有job
    /// 最后别忘了返回jobs，因为别的job system 可能还要用
    /// 完全独立于主进程，没有等待时间！
    /// </summary>
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new RotationSpeedRotation() { dt = Time.deltaTime };
        return job.Schedule(this, 64, inputDeps);
    }
}
