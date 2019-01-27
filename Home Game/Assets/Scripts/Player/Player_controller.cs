using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this makes sure that the Player has the Movement2D script attached so it doesn't start without it
[RequireComponent(typeof(Movement2D))] //THIS ALSO AUTOMATICALLY ADDS THE SCRIPTS NEED WOOOOOO STREAMLINING!!!
public class Player_controller : MonoBehaviour
{

    //these two variables give a more intuitive way to assing gravity and jump velocity rather than changing them directly
    public float accelerationTime = .1f;

    public float moveSpeed = 6;
    public float maxMoveSpeed = 12;

    public float rotateSpeed;
    public float aimAngle;
    Quaternion aimRotation;

    Vector3 velocity;
    float velocityXSmoothing;
    float velocityYSmoothing;
    

    Movement2D Movement;
   //public Animator animator;

    // Use this for initialization
    void Start()
    {
        Movement = GetComponent<Movement2D>();
        //just some physics to set the gravity and jump velocity
        Movement.collisions.faceDir = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetBool("isGrounded", Movement.collisions.below || Movement.passengerCollisions.standingOnPlatform);
        //animator.SetFloat("fallSpeed", velocity.y * Time.deltaTime);
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 aimDirection = new Vector2(Input.GetAxisRaw("RHorizontal"), Input.GetAxisRaw("RVertical"));

        Movement.CalculatePlayerVelocity(ref velocity, input, moveSpeed, ref velocityXSmoothing, ref velocityYSmoothing, accelerationTime);

        //animator.SetFloat("Speed", Mathf.Abs(velocity.x * Time.deltaTime));



        /*if(Input.GetButtonDown("Attack"))
        {


        }*/

        Movement.RotatePlayer(aimDirection, aimAngle, aimRotation, rotateSpeed);
        Movement.MovePlayer(velocity * Time.deltaTime, input);

        //MovePlayer is being called by both the player and the moving platforms, so we move this above and below collision to after the movement so that we have the correct values after we move the player with our own input
        //if you're hitting something above you or below you, velocity will change to zero.
        Movement.WallCollisions(ref velocity);
    }


}
