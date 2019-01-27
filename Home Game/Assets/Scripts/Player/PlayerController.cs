using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb2d;
    public float moveSpeed;

    public float rotateSpeed;
    private float aimAngle;
    Quaternion aimRotation;
    public Animator anim;
    private bool alive = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Die(){

        alive = false;
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal * moveSpeed, moveVertical * moveSpeed);
        float absHori = Mathf.Abs(moveHorizontal);
        float absVert = Mathf.Abs(moveVertical);
        anim.SetFloat("Move Horizontal", absHori);
        anim.SetFloat("Move Vertical", absVert);
        anim.SetBool("Alive", alive);


        rb2d.velocity = movement;

        Vector2 aimDirection = new Vector2(Input.GetAxisRaw("RHorizontal"), Input.GetAxisRaw("RVertical"));

        aimAngle = Mathf.Atan2(aimDirection.x, aimDirection.y) * Mathf.Rad2Deg;


        if (aimDirection.x != 0 && aimDirection.y != 0)
        {

            aimRotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, aimRotation, rotateSpeed * Time.time);
        }
    }
}
