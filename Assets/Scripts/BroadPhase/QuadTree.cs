using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadTree : BroadPhase
{
    QuadTreeNode m_node;

    public override void Build(AABB boundary, ref List<PhysicsObject> physicsObjects)
    {
        m_node = new QuadTreeNode();
    }

    public override void Draw(Color color, float duration = 0)
    {
        m_node.Draw(color, duration);
    }

    public override void Query(AABB range, ref List<PhysicsObject> physicsObjects)
    {
        m_node.Query(range, ref physicsObjects);
    }
}
