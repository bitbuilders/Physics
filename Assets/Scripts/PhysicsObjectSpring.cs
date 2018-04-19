using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObjectSpring : PhysicsObject
{
    public PhysicsObject physicsObjectLinkPrev { get; set; }
    public PhysicsObject physicsObjectLinkNext { get; set; }
    public float springConstant { get; set; }
	public float restLength { get; set; }

    public Vector2 GetSpringForce(float dt)
    {
        Vector2 forcePrev = GetForceFromLink(physicsObjectLinkPrev);
        Vector2 forceNext = GetForceFromLink(physicsObjectLinkNext) * 0.5f; // Had to reduce by half to get realistic results
        Vector2 force = forcePrev + forceNext;

        return force;
    }

    private Vector2 GetForceFromLink(PhysicsObject link)
    {
        if (link == null) return Vector2.zero;

        Vector2 springForce = position - link.position;
        float length = springForce.magnitude;
        length = Mathf.Abs(length - restLength);
        length *= springConstant;
        springForce = springForce.normalized * -length;

        return springForce;
    }

    public override void Draw(Color color, float duration = 0.0f)
    {
        base.Draw(color, duration);

        // Draw spring
        if (physicsObjectLinkNext != null)
        {
            Debug.DrawLine(position, physicsObjectLinkNext.position, Color.yellow);
        }
    }
}
