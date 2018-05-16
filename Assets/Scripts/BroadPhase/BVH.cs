using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVH : BroadPhase
{
    BVHNode m_node;

    public override void Build(AABB boundary, ref List<PhysicsObject> physicsObjects)
    {
        m_node = new BVHNode(physicsObjects);
    }

    public override void Draw(Color color, float duration = 0)
    {
        m_node.Draw(color, duration);
    }

    public override void Query(AABB range, ref List<PhysicsObject> physicsObjects)
    {
        m_node.ResetTouch();
        m_node.Query(range, ref physicsObjects);
    }
}
