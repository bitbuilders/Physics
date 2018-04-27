using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator
{
    [System.Serializable]
    public enum CreatorType
    {
        POINT,
        RANDOM,
        SPRING,
        VELOCITY
    }

    CreatorInputPoint m_creatorPoint = null;
    CreatorInputRandom m_creatorRandom = null;
    CreatorInputSpring m_creatorSpring = null;
    CreatorInputVelocity m_creatorVelocity = null;

    public CreatorType creatorType { get; set; }
    public Collider.eType type { get; set; }
	public float size { get; set; }
	public float mass { get; set; }
    public float restitutionCoef { get; set; }
    public float damping { get; set; }
    public float springConstant { get; set; }
    public float restLength { get; set; }
    public PhysicsObject physicsObjectLink { get; set; }

    public void Initialize()
    {
        m_creatorPoint = new CreatorInputPoint();
        m_creatorRandom = new CreatorInputRandom();
        m_creatorSpring = new CreatorInputSpring();
        m_creatorVelocity = new CreatorInputVelocity();
    }

    public PhysicsObject Update(float dt)
    {
        PhysicsObject physicsObject = null;

        switch (creatorType)
        {
            case CreatorType.POINT:
                physicsObject = m_creatorPoint.UpdateCreator(dt);
                break;
            case CreatorType.RANDOM:
                physicsObject = m_creatorRandom.UpdateCreator(dt);
                break;
            case CreatorType.SPRING:
                physicsObject = m_creatorSpring.UpdateCreator(dt);
                break;
            case CreatorType.VELOCITY:
                physicsObject = m_creatorVelocity.UpdateCreator(dt);
                break;
        }

        return physicsObject;
    }
}
