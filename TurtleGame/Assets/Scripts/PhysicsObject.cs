using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhysicsObject : MonoBehaviour
{

    [Header("Physics Object Attributes")]
    public float minGroundNormalY = 5f;
    public float gravityModifier = 1f;

    [SerializeField]protected Vector2 targetVelocity;
    [SerializeField]protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    [SerializeField]protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    public Vector2 currentNormal;

    protected const float minMoveDistance = 0.01f;
    [SerializeField]protected float shellRadius = 0.075f;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    void FixedUpdate()
    {
       //Debug.Log("velocity in fixed update1 " + velocity);
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move;
        move = moveAlongGround * deltaPosition.x;

        //Debug.Log("velocity in fixed update2 " + velocity + " move in fu2 " + move);
        Movement(move, false);

        move = Vector2.up * deltaPosition.y;
        //Debug.Log("velocity in fixed update3 " + velocity + " move in fu 3" + move);
        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        
        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                PlatformEffector2D platform = hitBuffer[i].collider.GetComponent<PlatformEffector2D>();
                if (!platform || (hitBuffer[i].normal == Vector2.up && velocity.y < 0 && yMovement))
                {
                    hitBufferList.Add(hitBuffer[i]);
                }
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                currentNormal = hitBufferList[i].normal;
                
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

        }
        //Debug.Log("YMovement:" + yMovement + " move:" + move.normalized * distance);
        //Debug.Log("PosB4:" + rb2d.position);
        rb2d.position = rb2d.position + move.normalized * distance;
        //Debug.Log("YMovementAfter:" + rb2d.position);
    }

    private void OnDrawGizmos()
    {
        Vector3 normalgizmoX = this.transform.position;
        normalgizmoX.x += 25;
        normalgizmoX.y += currentNormal.x * 25;
        Vector3 normalgizmoY = this.transform.position;
        normalgizmoY.y += 25;
        normalgizmoY.x += (1-currentNormal.y) * 25;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, normalgizmoX);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, normalgizmoY);
    }
}
