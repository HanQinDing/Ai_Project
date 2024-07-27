


#region Includes
using UnityEngine;
using System.Collections;
#endregion

/// <summary>
/// Component for car movement and collision detection.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    #region Members
    /// <summary>
    /// Event for when the car hit a wall.
    /// </summary>
    public event System.Action HitWall;
    public Rigidbody2D rb;
    public double speed, jumpforce;
    public Collider2D col;
    //Movement constants
    private const float MAX_VEL = 20f;
    private const float ACCELERATION = 8f;
    private const float VEL_FRICT = 2f;
    private const float TURN_SPEED = 100;

    private PlayerController controller;

    /// <summary>
    /// The current velocity of the car.
    /// </summary>
    public float Velocity
    {
        get;
        private set;
    }

    /// <summary>
    /// The current rotation of the car.
    /// </summary>
    public Quaternion Rotation
    {
        get;
        private set;
    }

    private double horizontalInput, verticalInput; //Horizontal = engine force, Vertical = turning force
    private bool isCollidingWithTargetLayer;

    /// <summary>
    /// The current inputs for turning and engine force in this order.
    /// </summary>
    public double[] CurrentInputs
    {
        get { return new double[] { horizontalInput, verticalInput, speed, jumpforce }; }
    }
    #endregion

    #region Constructors
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }
    #endregion

    #region Methods
    // Unity method for physics updates
	void FixedUpdate ()
    {
        //Get user input if controller tells us to
        if (controller != null && controller.UseUserInput)
            CheckInput();

        ApplyInput();

        //ApplyVelocity();

        //ApplyFriction();
	}

    // Checks for user input
    private void CheckInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    // Applies the currently set input
    private void ApplyInput()
    {
        if (horizontalInput > 0)
        { // move right 
            rb.velocity = new Vector2((float)speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        else if (horizontalInput < 0) { // move left
            rb.velocity = new Vector2(-(float)speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }

        if (verticalInput > 0 && isCollidingWithTargetLayer) {
            rb.velocity = new Vector2(rb.velocity.x, (float)jumpforce);

        }
    }

    /// <summary>
    /// Sets the engine and turning input according to the given values.
    /// </summary>
    /// <param name="input">The inputs for turning and engine force in this order.</param>
    public void SetInputs(double[] input)
    {
        horizontalInput = input[0];
        verticalInput = input[1];
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the layer of the colliding object matches the target layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            isCollidingWithTargetLayer = true;
            //Debug.Log("Continuing to collide with object on target layer: " + targetLayerName);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the layer of the colliding object matches the target layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            isCollidingWithTargetLayer = false;
            //Debug.Log("Stopped colliding with object on target layer: " + targetLayerName);
        }
    }
    // Applies the current velocity to the position of the car.
    private void ApplyVelocity()
    {
        Vector3 direction = new Vector3(0, 1, 0);
        transform.rotation = Rotation;
        direction = Rotation * direction;

        this.transform.position += direction * Velocity * Time.deltaTime;
    }

    // Applies some friction to velocity
    private void ApplyFriction()
    {
        if (verticalInput == 0)
        {
            if (Velocity > 0)
            {
                Velocity -= VEL_FRICT * Time.deltaTime;
                if (Velocity < 0)
                    Velocity = 0;
            }
            else if (Velocity < 0)
            {
                Velocity += VEL_FRICT * Time.deltaTime;
                if (Velocity > 0)
                    Velocity = 0;            
            }
        }
    }

    // Unity method, triggered when collision was detected.
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Replace 'yourLayerName' with the name of the layer you want to check
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (HitWall != null)
                HitWall();
        }
    }


    /// <summary>
    /// Stops all current movement of the car.
    /// </summary>
    public void Stop()
    {
        Velocity = 0;
        Rotation = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
    }
    #endregion
}
