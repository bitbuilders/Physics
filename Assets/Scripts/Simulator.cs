using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    [SerializeField] Creator.CreatorType m_creatorType = Creator.CreatorType.POINT;
	[SerializeField] Collider.eType m_type = Collider.eType.POINT;
    [SerializeField] BroadPhase.eType m_broadPhaseType = BroadPhase.eType.BVH;
    [SerializeField] [Range(-50.0f, 50.0f)] float m_gravity = 0.0f;
    [SerializeField][Range(0.0f, 1.0f)] float m_restingVelocityMin = 0.015f;
    [SerializeField][Range(0.0f, 1.0f)] float m_damping = 1.0f;
    [SerializeField][Range(0.0f, 1.0f)] float m_restitutionCoef = 1.0f;
    [SerializeField] [Range(0.1f, 5.0f)] float m_size = 1.0f;
	[SerializeField] [Range(0.0f, 100.0f)] float m_mass = 1.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_springConstant = 2.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_restLength = 2.0f;
    [SerializeField] [Range(0.0f, 10.0f)] float m_querySize = 2.0f;
    [SerializeField] int m_count = 0;

    public List<PhysicsObject> m_physicsObjects = null;
    List<PhysicsObject> m_queriedObjects = null;
    List<Intersection.Result> m_intersections = null;
    Creator m_creator = null;
    BroadPhase m_broadPhase;
    List<Creator> m_creatorTypes;
    List<BroadPhase> m_phaseTypes;
    AABB m_queryRange;

    public delegate void IntegratorDelegate(float dt, PhysicsObject physicsObject);
    IntegratorDelegate m_integrator;

    void Awake()
    {
        m_creatorTypes = new List<Creator>() {
            new CreatorInputLine(), new CreatorInputPoint(), new CreatorInputRandom(),
            new CreatorInputSpring(), new CreatorInputVelocity() };
        m_phaseTypes = new List<BroadPhase>() { new BVH(), new QuadTree() };
        m_broadPhase = m_phaseTypes[(int)m_broadPhaseType];
        m_creator = m_creatorTypes[(int)m_creatorType];
        m_physicsObjects = new List<PhysicsObject>();
        m_intersections = new List<Intersection.Result>();
        m_queriedObjects = new List<PhysicsObject>();
        m_queryRange = new AABB(Input.mousePosition, new Vector2(m_querySize, m_querySize));

        m_integrator = Integrator.ExplicitEuler;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // set creator values
        m_creator = m_creatorTypes[(int)m_creatorType];
        m_creator.restitutionCoef = m_restitutionCoef;
        m_creator.damping = m_damping;
        m_creator.type = m_type;
		m_creator.size = m_size;
		m_creator.mass = m_mass;
        m_creator.springConstant = m_springConstant;
        m_creator.restLength = m_restLength;
        m_count = m_physicsObjects.Count;

        m_queryRange.size = new Vector2(m_querySize, m_querySize);
        Vector2 size = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height) * 1.495f);
        AABB boundary = new AABB(Vector2.zero, size);
        m_broadPhase.Build(boundary, ref m_physicsObjects);

        m_creator.physicsObjectLink = (m_physicsObjects.Count == 0) ? null : m_physicsObjects[m_physicsObjects.Count - 1];

        // create physics objects
        PhysicsObject newPhysicsObject = m_creator.Update(dt);
        if (newPhysicsObject != null)
        {
            m_physicsObjects.Add(newPhysicsObject);

            if (newPhysicsObject.GetType() == typeof(PhysicsObjectSpring))
            {
                if (m_physicsObjects.Count > 1 && m_physicsObjects[m_physicsObjects.Count - 2].GetType() == typeof(PhysicsObjectSpring))
                {
                    ((PhysicsObjectSpring) m_physicsObjects[m_physicsObjects.Count - 2]).AddLink(newPhysicsObject);
                }
            }
            else if (!Input.GetKey(KeyCode.LeftShift) && newPhysicsObject.GetType() == typeof(PhysicsObjectSpringVerlet))
            {
                if (m_physicsObjects.Count > 1 && m_physicsObjects[m_physicsObjects.Count - 2].GetType() == typeof(PhysicsObjectSpringVerlet))
                {
                    PhysicsObjectSpringVerlet prev = (PhysicsObjectSpringVerlet)m_physicsObjects[m_physicsObjects.Count - 2];
                    prev.AddLink(newPhysicsObject);
                    ((PhysicsObjectSpringVerlet)newPhysicsObject).restLength = (prev.position - newPhysicsObject.position).magnitude;
                }
            }
        }

        Vector2 force = Vector2.zero;
        force.x = Input.GetAxis("Horizontal") * 20.0f;
        force.y = Input.GetAxis("Vertical") * 20.0f;

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
            physicsObject.state = physicsObject.state & ~PhysicsObject.eState.COLLIDED;
            if ((physicsObject.state & PhysicsObject.eState.AWAKE) == PhysicsObject.eState.AWAKE)
            {
                physicsObject.StepSimulation(dt, m_integrator);
            }
        }

        // check if objects are asleep
        foreach (PhysicsObject physicsObject in m_physicsObjects)
        {
            float bias = Mathf.Pow(0.1f, dt);
            physicsObject.dynamicMotion = (bias * physicsObject.dynamicMotion) + ((1.0f - bias) * physicsObject.velocity.magnitude * dt);
            if (physicsObject.dynamicMotion < m_restingVelocityMin)
            {
                physicsObject.SetAwake(false);
            }
        }

        // check collision detection
        Intersection.Result result = new Intersection.Result();
        foreach (PhysicsObject physicsObject in m_physicsObjects)
        {
            AABB range = new AABB(physicsObject.position, new Vector2(4.0f, 4.0f));
            m_queriedObjects.Clear();
            m_broadPhase.Query(range, ref m_queriedObjects);
            foreach (PhysicsObject queriedPhysicsObject in m_queriedObjects)
            {
                if (physicsObject != queriedPhysicsObject)
                {
                    bool intersects = physicsObject.Intersects(queriedPhysicsObject, ref result);
                    bool bothAsleep = !physicsObject.Awake && !queriedPhysicsObject.Awake;
                    if (!bothAsleep && intersects)
                    {
                        bool oneIsStatic = (physicsObject.inverseMass == 0.0f || queriedPhysicsObject.inverseMass == 0.0f);
                        if (!physicsObject.Awake && !oneIsStatic) physicsObject.SetAwake(true);
                        else if (!queriedPhysicsObject.Awake && !oneIsStatic) queriedPhysicsObject.SetAwake(true);

                        physicsObject.state |= PhysicsObject.eState.COLLIDED;
                        queriedPhysicsObject.state |= PhysicsObject.eState.COLLIDED;
                        m_intersections.Add(result);
                    }
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
            if (relativeVelocity.magnitude < 0.1f)
            {
                restitution = 0.0f;
            }
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
            bool red = (physicsObject.state & PhysicsObject.eState.COLLIDED) == PhysicsObject.eState.COLLIDED;
            bool blue = !physicsObject.Awake;
            Color color;
            if (red) color = Color.red;
            else if (blue) color = Color.blue;
            else color = Color.white;
			physicsObject.Draw(color);
		}

        m_intersections.Clear();

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_queryRange.center = mousePos;
        m_queryRange.Draw(Color.green, 0.0f);
        m_queriedObjects.Clear();
        m_broadPhase.Build(boundary, ref m_physicsObjects);
        m_broadPhase.Query(m_queryRange, ref m_queriedObjects);
        foreach (PhysicsObject physicsObject in m_queriedObjects)
        {
            Debug.DrawLine(mousePos, physicsObject.position, Color.red);
        }

        m_broadPhase.Draw(Color.green, 0.0f);
    }
}
