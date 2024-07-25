using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{


    public Rigidbody2D rb;
    public float speed;
    public float jumpforce;
    public Collider2D col;

    public Animator anim;
    [SerializeField] private LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dir = Input.GetAxisRaw("Horizontal");

        if(dir < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1,1);
            anim.SetBool("running",true);
        }
       
        else if(dir > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
             transform.localScale = new Vector2(1,1);
             anim.SetBool("running",true);
        }

        else
        {
            anim.SetBool("running",false);
        }

        if(Input.GetButtonDown("Jump") && col.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        }

    }
}
