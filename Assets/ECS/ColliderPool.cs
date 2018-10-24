using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPool : MonoBehaviour {

	public static ColliderPool CP;
	public static GameObject boxCollider;

	void Awake(){
		if(CP !=null && CP != this){
			Destroy(gameObject);
		}else{
			CP = this;
		}
	}

	public void AddCollider(Vector3 entitypos){
		GameObject obj = (GameObject)Instantiate(boxCollider);
		obj.transform.position = entitypos;
		obj.transform.parent = transform;
		obj.layer = 9;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
