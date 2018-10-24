using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;
public class EntityInit : MonoBehaviour {

	[Header("Cube Prefab:")]
	[SerializeField]public GameObject prefab;

	[Header("width:")]
	[SerializeField] public int width;

	[Header("length:")]
	[SerializeField] public int length;
	[Header("height")]
	[SerializeField] public int height;

	[Header("/n噪声:")]
	[Header("随机种子 seed:")]
	[SerializeField] public int seed;

	[Header("基础高度")]
	[SerializeField] public int baseHeight = 10;
    public float frequency = 0.025f;
    public float amplitude = 1;

	private Vector3 offset0;
	private Vector3 offset1;
	private Vector3 offset2;

	int[,,] mapHeight;
	void Awake(){
		initWorld();
	}

	void initWorld(){
		initWorldMapData();
		GameObject _cube = null;
		int h;
		for(int i = 0;i<width;i++){
			for(int j=0;j<length;j++){
				// for(int h = 0;h<height;h++){
				h = mapHeight[i,j,0];

				_cube = Instantiate(prefab,new Vector3(i,h,j),Quaternion.identity);
					// MeshRenderer mr =_cube.GetComponent<MeshRenderer>();
					// mr.material.color = Color.red;
				// }
			}
		}
	}

	void initWorldMapData(){
		Random.InitState(seed);
		offset0 = new Vector3(Random.value *1000,Random.value*1000,Random.value*1000);
		offset1 = new Vector3(Random.value *1000,Random.value*1000,Random.value*1000);
		offset2 = new Vector3(Random.value *1000,Random.value*1000,Random.value*1000);

		mapHeight = new int[width,length,1];
		for(int x = 0 ; x < width ; x++){
			for(int y = 0; y < length ; y++){
				// for (int z = 0; z < height; z++)
                // {
                    mapHeight[x, y, 0] = getLandItemHeight(new Vector3(x, y, 0) + transform.position);
					Debug.Log("x y z"+x+" "+ y +" "+0 +"   ------ "+mapHeight[x,y,0]);
                // }
			}
		}
	}

	int getLandItemHeight(Vector3 pos){
		float x0 = (pos.x + offset0.x)*frequency;
		float y0 = (pos.y + offset0.y)*frequency;
		float z0 = (pos.z + offset0.z)*frequency;

		float x1 = (pos.x + offset1.x)*frequency * 2;
		float y1 = (pos.y + offset1.y)*frequency * 2;
		float z1 = (pos.z + offset1.z)*frequency * 2;

		float x2 = (pos.x + offset2.x)*frequency / 4;
		float y2 = (pos.y + offset2.y)*frequency / 4;
		float z2 = (pos.z + offset2.z)*frequency / 4;

		float noise0 = Noise.Generate(x0, y0, z0) * amplitude;
        float noise1 = Noise.Generate(x1, y1, z1) * amplitude / 2;
        float noise2 = Noise.Generate(x2, y2, z2) * amplitude / 4;

		 //在采样结果上，叠加上baseHeight，限制随机生成的高度下限
        return Mathf.FloorToInt(noise0 + noise1 + noise2 + baseHeight);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
