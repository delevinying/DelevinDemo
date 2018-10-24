using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public class Managers : ComponentSystem {
	struct Sp{
		public SpData spData;
		public Transform transform;
	}

	protected override void OnUpdate(){
		foreach (var sp in GetEntities<Sp>())
       	{
           
           sp.transform.Rotate(Vector3.up * Time.deltaTime * sp.spData.speed, Space.Self);
           
        //    sp.transform.position = sp.planetComponent.orbit.Evaluate(Time.time / sp.planetComponent.orbitDuration);
       	}
	}
	
}



