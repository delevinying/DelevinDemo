using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public class RotatorSystem : ComponentSystem {
	struct Group{
		public Transform transform;
		public Rotator rotator;
	}

	protected override void OnUpdate(){
		float del = Time.deltaTime;
		foreach(var entity in GetEntities<Group>()){
			entity.transform.rotation *= Quaternion.AngleAxis(entity.rotator.speed*del,Vector3.up);
		}
	}
}
