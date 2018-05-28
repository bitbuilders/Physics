using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObjectSpringVerlet : PhysicsObjectSpring
{
    public override void Draw(Color color, float duration = 0.0f)
    {
        base.Draw(color, duration);

        // Draw spring
        foreach (PhysicsObject physicsObject in physicsObjectLinks)
        {
            if (physicsObject.GetType() == typeof(PhysicsObjectSpringVerlet))
            {
                if (physicsObject != null)
                {
                    Debug.DrawLine(position, physicsObject.position, Color.yellow);
                }
            }
        }
    }

    public void UpdateSpring()
    {
        foreach (PhysicsObject po in physicsObjectLinks)
        {
            if (po.GetType() == typeof(PhysicsObjectSpringVerlet))
            {
                PhysicsObjectSpringVerlet next = (PhysicsObjectSpringVerlet)po;
                Vector2 pos = next.position;
                Vector2 delta = position - pos;
                float deltaLength = Mathf.Sqrt(Vector2.Dot(delta, delta));
                float diff = (deltaLength - restLength) / deltaLength;
                position -= delta * 0.5f * diff;
                next.position += delta * 0.5f * diff;
            }
        }
    }

    public void UpdateSprings()
    {
        foreach (PhysicsObject po in physicsObjectLinks)
        {
            if (po.GetType() == typeof(PhysicsObjectSpringVerlet))
            {
                ((PhysicsObjectSpringVerlet)po).UpdateSpring();
            }
        }
    }
}
