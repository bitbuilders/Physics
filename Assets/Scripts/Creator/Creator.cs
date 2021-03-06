﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creator
{
    [System.Serializable]
    public enum CreatorType
    {
        LINE,
        POINT,
        RANDOM,
        SPRING,
        VERLET,
        VELOCITY
    }

    public Collider.eType type { get; set; }
	public float size { get; set; }
	public float mass { get; set; }
    public float restitutionCoef { get; set; }
    public float damping { get; set; }
    public float springConstant { get; set; }
    public float restLength { get; set; }
    public PhysicsObject physicsObjectLink { get; set; }

    public abstract PhysicsObject Update(float dt);
}
