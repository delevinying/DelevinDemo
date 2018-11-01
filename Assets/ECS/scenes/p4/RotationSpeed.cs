using System;
using Unity.Entities;

public struct RotationSpeed : IComponentData {
	public float Value;
}

public class RotationSpeedComponent : ComponentDataWrapper<RotationSpeed>{}
