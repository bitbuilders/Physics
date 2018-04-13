using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creator
{
	public Collider.eType type { get; set; }
	public float size { get; set; }
	public float mass { get; set; }
    public float damping { get; set; }

    public abstract PhysicsObject Update(float dt);
}
