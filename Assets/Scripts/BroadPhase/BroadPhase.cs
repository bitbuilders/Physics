using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BroadPhase
{
    public abstract void Build(AABB boundary, ref List<PhysicsObject> physicsObjects);
    public abstract void Query(AABB range, ref List<PhysicsObject> physicsObjects);
    public abstract void Draw(Color color, float duration = 0.0f);
}
