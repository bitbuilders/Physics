﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject
{
	public Collider m_collider = null;
	public bool m_collided = false;
    public float m_mass = 0.0f;

	public Vector2 position { get; set; }
	public Vector2 previousPosition { get; set; }
    public Vector2 velocity { get; set; }
	public Vector2 force { get; set; }
	public Vector2 acceleration { get; set; }
    public float restitutionCoef { get; set; }
	public float damping { get; set; }
	public float inverseMass { get; set; }
    public float mass
    {
        get { return m_mass; }
        set
        {
            m_mass = value;
            inverseMass = value > 0.0f ? 1.0f / value : 0.0f;
        }
    }

	public virtual void Initialize(Collider collider, Vector2 position, Vector2 velocity, float mass, float damping)
    {
		m_collider = collider;
		m_collider.physicsObject = this;
		
        this.position = position;
        previousPosition = position;
		inverseMass = (mass == 0.0f) ? 0.0f : (1.0f / mass);
        this.mass = mass;
		this.velocity = velocity * inverseMass;
        this.damping = damping;
    }

    public virtual void Draw(Color color, float duration = 0.0f)
    {
        m_collider.Draw(position, color, duration);
    }

	public bool Intersects(PhysicsObject other, ref Intersection.Result result)
	{
		bool intersects = m_collider.Intersects(other.m_collider, ref result);

		return intersects;
	}

    public void StepSimulation(float dt, Simulator.IntegratorDelegate integrator)
    {
        integrator(dt, this);
    }
}
