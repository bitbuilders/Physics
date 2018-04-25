using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
	[SerializeField] Collider.eType m_type = Collider.eType.POINT;
    [SerializeField] [Range(-50.0f, 50.0f)] float m_gravity = 0.0f;
    [SerializeField][Range(0.0f, 1.0f)] float m_damping = 1.0f;
    [SerializeField][Range(0.0f, 1.0f)] float m_restitutionCoef = 1.0f;
    [SerializeField] [Range(0.1f, 5.0f)] float m_size = 1.0f;
	[SerializeField] [Range(0.0f, 100.0f)] float m_mass = 1.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_springConstant = 2.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_restLength = 2.0f;

    public List<PhysicsObject> m_physicsObjects = null;
    List<Intersection.Result> m_intersections = null;
    Creator m_creator = null;

    public delegate void IntegratorDelegate(float dt, PhysicsObject physicsObject);
    IntegratorDelegate m_integrator;

    void Awake()
    {
        m_creator = new CreatorInputVelocity();
        m_physicsObjects = new List<PhysicsObject>();
        m_intersections = new List<Intersection.Result>();

        m_integrator = Integrator.ExplicitEuler;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // set creator values
        m_creator.restitutionCoef = m_restitutionCoef;
        m_creator.damping = m_damping;
        m_creator.type = m_type;
		m_creator.size = m_size;
		m_creator.mass = m_mass;
        m_creator.springConstant = m_springConstant;
        m_creator.restLength = m_restLength;

        m_creator.physicsObjectLink = (m_physicsObjects.Count == 0) ? null : m_physicsObjects[m_physicsObjects.Count - 1];

        // create physics objects
        PhysicsObject newPhysicsObject = m_creator.Update(dt);
        if (newPhysicsObject != null)
        {
            m_physicsObjects.Add(newPhysicsObject);

            if (newPhysicsObject.GetType().Equals(typeof(PhysicsObjectSpring)))
            {
                if (m_physicsObjects.Count > 1)
                {
                    ((PhysicsObjectSpring) m_physicsObjects[m_physicsObjects.Count - 2]).AddLink(newPhysicsObject);
                }
            }
        }

        Vector2 force = Vector2.zero;
        force.x = Input.GetAxis("Horizontal") * 5.0f;
        force.y = Input.GetAxis("Vertical") * 5.0f;

        foreach (PhysicsObject physicsObject in m_physicsObjects)
        {
            physicsObject.force = Vector2.down * m_gravity;
            physicsObject.force += force;

            if (physicsObject.GetType() == typeof(PhysicsObjectSpring))
            {
                physicsObject.force += ((PhysicsObjectSpring)physicsObject).GetSpringForce(dt);
            }
        }

        // reset physics object collision state
        foreach (PhysicsObject physicsObject in m_physicsObjects)
        {
            physicsObject.m_collided = false;
            physicsObject.StepSimulation(dt, m_integrator);
        }

        // check collision detection
        Intersection.Result result = new Intersection.Result();
		for (int i = 0; i < m_physicsObjects.Count; i++)
		{
			for (int j = i + 1; j < m_physicsObjects.Count; j++)
			{
				bool intersects = m_physicsObjects[i].Intersects(m_physicsObjects[j], ref result);
				if (intersects)
				{
					m_physicsObjects[i].m_collided = true;
					m_physicsObjects[j].m_collided = true;
                    m_intersections.Add(result);
				}
			}
		}

        //Solve intersections
        foreach (Intersection.Result intersection in m_intersections)
        {
            PhysicsObject object1 = intersection.collided1.physicsObject;
            PhysicsObject object2 = intersection.collided2.physicsObject;
            //object1.position -= intersection.contactNormal * intersection.distance * 0.5f;
            object2.position += intersection.contactNormal * intersection.distance * 1.0f;

            Vector2 relativeVelocity = object1.velocity - object2.velocity;
            float dot = Vector2.Dot(relativeVelocity, intersection.contactNormal);
            float restitution = (object1.restitutionCoef + object2.restitutionCoef) * 0.5f;
            Vector2 impulse1 = intersection.contactNormal * (relativeVelocity).magnitude * 1.5f * restitution;
            Vector2 impulse2 = intersection.contactNormal * (relativeVelocity).magnitude * 1.5f * restitution;

            if (dot > 0.0f)
            {
                // Doesn't get called
                object1.velocity -= impulse1 * object1.inverseMass;
                object2.velocity += impulse2 * object2.inverseMass;
            }
            else if(dot < 0.0f)
            {
                object1.velocity += impulse1 * object1.inverseMass;
                object2.velocity -= impulse2 * object2.inverseMass;
            }
        }

        // draw physics objects
        foreach (PhysicsObject physicsObject in m_physicsObjects)
        {
			Color color = (physicsObject.m_collided) ? Color.red : Color.white;
			physicsObject.Draw(color);
		}

        m_intersections.Clear();
    }
}
