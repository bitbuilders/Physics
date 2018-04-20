using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObjectSpring : PhysicsObject
{
    public List<PhysicsObject> physicsObjectLinks = new List<PhysicsObject>();
    public float springConstant { get; set; }
	public float restLength { get; set; }

    public void AddLink(PhysicsObject physicsObject)
    {
        physicsObjectLinks.Add(physicsObject);
    }

    public Vector2 GetSpringForce(float dt)
    {
        Vector2 force = Vector2.zero;

        foreach (PhysicsObject physicsObject in physicsObjectLinks)
        {
            force += GetForceFromLink(physicsObject);
        }

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
        foreach (PhysicsObject physicsObject in physicsObjectLinks)
        {
            if (physicsObject != null)
            {
                Debug.DrawLine(position, physicsObject.position, Color.yellow);
            }
        }
    }
}
