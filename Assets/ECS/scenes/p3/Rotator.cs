using System;
using Unity.Entities;
public struct Rotator : IComponentData {
	public float speed;
}

public class RotationSpeedComponent : ComponentDataWrapper<Rotator> { }