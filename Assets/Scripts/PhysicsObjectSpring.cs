using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObjectSpring : PhysicsObject
{
    public PhysicsObject physicsObjectLink { get; set; }
	public float springConstant { get; set; }
	public float restLength { get; set; }

    public Vector2 GetSpringForce(float dt)
    {
        if (physicsObjectLink == null) return Vector2.zero;

        Vector2 springForce = position - physicsObjectLink.position;
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
        if (physicsObjectLink != null)
        {
            Debug.DrawLine(position, physicsObjectLink.position, Color.yellow);
        }
    }
}
