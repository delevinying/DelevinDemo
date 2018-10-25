using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public struct BlockTag : IComponentData {
    
	
}

public struct MeshInstanceRenderer:ISharedComponentData{
    public Mesh mesh;
    public Material material;
}

public struct SurfacePlantTag:IComponentData{

}
