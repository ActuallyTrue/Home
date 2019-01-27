using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This makes sure that whatever the movement script is moving has a collider of some type
[RequireComponent(typeof(Collider2D))] //using just collider2D (I did this to be more general) may cause problems later, I'm not sure, the video used BoxCollider2D but we will see
public class Movement2D : MonoBehaviour
{

    public LayerMask collisionMask;
    //for moving things on platforms
    public LayerMask passengerMask;

    const float skinWidth = .015f;
    const float dstBetweenRays = .25f;
    [HideInInspector]
    public int horizontalRayCount;
    [HideInInspector]
    public int verticalRayCount;

    public float maxSlopeAngle = 80;


    //spacing between each ray
    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    [HideInInspector]
    public Collider2D collider;
    public RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;
    public PassengerMovement passengerCollisions;

    [HideInInspector]
    public Vector2 playerInput;

    List<PassengerMovement> passengerMovementList;
    //Dictionary is like a Hashset but you can access specific values
    Dictionary<Transform, Movement2D> passengerDictionary = new Dictionary<Transform, Movement2D>();

    //this goes before start functions, so the camera will be able to latch onto the player's colliders
    public void Awake()
    {
        collider = GetComponent<Collider2D>();
        CalculateRaySpacing(); //the only time you need to use this function again is if you change the amount of rays mid game
    }

    public void Start()
    {
        Vector2 startTopLeft = raycastOrigins.topLeft;
        Vector2 startTopRight = raycastOrigins.topRight;
        Vector2 startBottomLeft = raycastOrigins.bottomLeft;
        Vector2 startBottomRight = raycastOrigins.bottomRight;
    }


    public void CalculatePlayerVelocity(ref Vector3 velocity, Vector2 input, float moveSpeed, ref float velocityXSmoothing, ref float velocityYSmoothing, float accelerationTime)
    {
        float targetVelocityX = input.x * moveSpeed;
        float targetVelocityY = input.y * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);
        velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTime);
    }

    public void CalculateEnemyVelocity(ref Vector3 velocity, float moveSpeed, float gravity, ref float velocityXSmoothing, float accelerationTimeGrounded)
    {
        float targetVelocityx = moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityx, ref velocityXSmoothing, accelerationTimeGrounded);
        velocity.y += gravity * Time.deltaTime;
    }


    //if we want the player to move slower in a time zone, we could just create another function called SlowMove where it's basically the same, but the variables are just changed so that the player does everything they normally do slower
    public void MovePlayer(Vector3 velocity, Vector2 input)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;
        playerInput = input;

        if (velocity.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(velocity.x);
        }


        HorizontalCollisions(ref velocity);

        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }


        transform.Translate(velocity);

    }

    public void RotatePlayer(Vector2 aimDirection,float aimAngle, Quaternion aimRotation, float rotateSpeed){

        aimAngle = Mathf.Atan2(aimDirection.x, aimDirection.y) * Mathf.Rad2Deg;

        Debug.Log(aimDirection + " " + aimAngle, gameObject);

        if (aimDirection.x != 0 && aimDirection.y != 0){

            aimRotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, aimRotation, rotateSpeed * Time.time);
        }
    }

    public void WallCollisions(ref Vector3 velocity)
    {
        if (collisions.above || collisions.below)
        {
            velocity.y = 0;
        }
        if(collisions.right || collisions.left){

            velocity.x = 0;
        }
    }

    public void MoveGroundEnemy(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;

        if (velocity.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(velocity.x);
        }


        HorizontalCollisions(ref velocity);

        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        /*if (standingOnPlatform == true)
        {
            collisions.below = true;
        }*/


        transform.Translate(velocity);

    }

  




    //the ref means that it will change the variable that gets passed through it rather than creating a copy and changing it
    public void HorizontalCollisions(ref Vector3 velocity) //if we want different climb angles for enemies or something, make more hori or vert collision functions for them specifically vid is episode 4
    {
        float directionX = collisions.faceDir;
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        //so that a little ray will go out from whichever direction you were just moving to detect walls
        if (Mathf.Abs(velocity.x) < skinWidth)
        {
            rayLength = skinWidth * 2;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            //if we're moving left we want raycasts left. ? means if it's true, set ray origins equal to raycastOrigins.bottomLeft : means if we're moving right then start at bottom right
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

            //changing the ray origin to where you will be after moving on the x axis
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            //starts from the bottom left, then moves over one ray space every iteration, and it sends each ray left or right
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                //so that the horizontal rays don't interfere with you falling through the platform
                if (hit.collider.tag == "MoveThrough")
                {
                    if (directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if (playerInput.y == -1 || hit.distance == 0)
                    {
                        continue;
                    }
                }


                    //changing x velocity to how much we need to move to get to whatever the raycast hit
                velocity.x = (hit.distance - skinWidth) * directionX;
                //it changes the raylength so the ray will only hit what you're standing on if you're on a ledge or something
                rayLength = hit.distance;

                    //setting the collision booleans based on the direction you're moving
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;

            }

        }
    }


    //the ref means that it will change the variable that gets passed through it rather than creating a copy and changing it
    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            //if we're moving up we want raycasts up. ? means if it's true, set ray origins equal to raycastOrigins.bottomLeft : means if we're moving up then start at top left
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;

            //changing the ray origin to where you will be after moving on the x axis
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            //starts from the bottom left, then moves over one ray space every iteration, and it sends each ray down
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {

                //changing y velocity to how much we need to move to get to whatever the raycast hit
                velocity.y = (hit.distance - skinWidth) * directionY;
                //it changes the raylength so the ray will only hit what you're standing on if you're on a ledge or something
                rayLength = hit.distance;


                //setting the collision booleans based on the direction you're moving
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }

        }

    }


    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        //this isn't actually expanding it, it's making it smaller by multiplying the skin width by -2
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        //this isn't actually expanding it, it's making it smaller by multiplying the skin width by -2
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        //making sure you're sending at least 2 horizontal and vertical rays
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        //these equation gives equal spacing for however many horizontal raycasts you send out
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);

    }

    //this helps us easily get the corners of any box collider to start the raycasts.
    //These values won't change, so structs are useful because they can just be copied for any game object.
    public struct RaycastOrigins
    {

        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;

    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle;
        public float slopeAngleOld;
        public Vector3 velocityOld;
        public int faceDir;
        public bool fallingThroughPlatform;

        public void Reset()
        {
            above = false;
            below = false;
            left = false;
            right = false;
            climbingSlope = false;
            descendingSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

    //This struct will store all the variables needed for moving passengers
    public struct PassengerMovement
    {
        public Transform transform;
        public Vector3 velocity;
        public bool standingOnPlatform;
        public bool moveBeforePlatform;
        public PassengerMovement(Transform _transform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
        {
            transform = _transform;
            velocity = _velocity;
            standingOnPlatform = _standingOnPlatform;
            moveBeforePlatform = _moveBeforePlatform;

        }


    }


}
