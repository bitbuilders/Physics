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
            PhysicsObject object1 = intersection.collider1.physicsObject;
            PhysicsObject object2 = intersection.collider2.physicsObject;
            Vector2 separation = intersection.contactNormal * (intersection.distance / (object1.inverseMass + object2.inverseMass));
            object1.position += separation * object1.inverseMass;
            object2.position -= separation * object2.inverseMass;

            Vector2 relativeVelocity = object2.velocity - object1.velocity;
            float velNorm = Vector2.Dot(relativeVelocity, intersection.contactNormal);
            float restitution = Mathf.Min(object1.restitutionCoef, object2.restitutionCoef);
            Vector2 impulse = Vector2.zero;
            float j = -(1.0f + restitution) * velNorm / (object1.inverseMass + object2.inverseMass); 
            impulse = intersection.contactNormal * j;

            if (velNorm < 0.0f) // Opposite direction
            {
                object1.velocity -= impulse * object1.inverseMass;
                object2.velocity += impulse * object2.inverseMass;
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
