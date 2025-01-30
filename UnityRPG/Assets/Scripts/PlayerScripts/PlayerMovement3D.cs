using System.Collections;
using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    // Cutscene/Combat checker
    public bool canMove = true;

    // Movement variables
    public float walkSpeed;
    public float sprintBoost;
    private float actualSpeed;
    private Vector3 movementDirection;

    // Animation variables
    private string currentAnimation = "Idle";
    private float _FloatingTime = 0;

    // Jump variables
    public float jumpForce;
    public float jumpDuration;
    private float _jumpDurationSet;
    private bool isJumping = false;
    private bool isInAir = false;

    // Physics variables
    private Vector3 gravityDirection;
    public float gravityForce;
    private float _gravitySet;
    bool characterIsGrounded;

    // Loading other game objects
    public Transform cameraOrientation;

    // Loading components
    private CharacterController characterController;
    private new Animation animation;

    // Player Input variables
    private float horizontalInput;
    private float verticalInput;
    private bool sprintInput;


    void Start()
    {
        // Loads the necessary components
        animation = GetComponent<Animation>();
        characterController = GetComponent<CharacterController>();

        // Checks if the character is grounded at the beginning of the game
        characterIsGrounded = characterController.isGrounded;

        // _gravitySet is used to store the default gravity force
        _gravitySet = gravityForce;

        // _jumpDurationSet is used to store the default gravity force
        _jumpDurationSet = jumpDuration;
    }

    private void FixedUpdate()
    {
        // Stores the boolean used to check if the character is grounded
        characterIsGrounded = characterController.isGrounded;
    }

    void Update()
    {
        // Check if Player is allowed to move - if not, inputs are disabled
        if (!canMove)
        {
            horizontalInput = 0f;
            verticalInput = 0f;
        }
        else
        {
            // Assigns the Input variables their corresponding Inputs while player is allowed to move
            sprintInput = Input.GetButton("Sprint");
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }

        // Character functions
        Movement();
        GravityPhysics();
        Animations();
    }

    private void Sprint()
    {
        // Checks if the Sprint key is held down
        if (sprintInput)
        {
            // Sum of walk speed and sprint boost is used for the actual movement speed
            actualSpeed = walkSpeed + sprintBoost;
        }
        else
        {
            // The walk speed is used for the actual movement speed 
            actualSpeed = walkSpeed;
        }
    }

    protected void Movement()
    {
        // Checking if the Player holds down the Sprint (default: Left Shift) button
        Sprint();

        // Move the player based on horizontal and vertical inputs
        movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection = Quaternion.AngleAxis(cameraOrientation.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();
        Vector3 velocity = movementDirection * actualSpeed;
        characterController.Move(velocity * Time.deltaTime);

        // Checks if the movementDirection is not a zero Vector3
        if (movementDirection != Vector3.zero)
        {
            // Rotates the character forward, based on its motion
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 999 * Time.deltaTime);
        }

        // Checks for the Player Jump input while the character is grounded
        if (characterIsGrounded && Input.GetButton("Jump"))
        {
            isJumping = true;
        }

        // Runs the Jump function while the character is in the process of Jumping
        if (isJumping)
        {
            Jump();
        }
    }

    protected void Jump()
    {
        if (isJumping && jumpDuration > 0)
        {
            // Starts a short clock to determine how long has the player been falling
            _FloatingTime += Time.deltaTime;

            // Sets true to play the falling animation
            isInAir = true;

            // Decrement of the jump timer
            jumpDuration -= Time.deltaTime;

            // Adds the jumping force used to push the character upwards
            gravityForce += jumpForce;
            gravityForce /= 2;

            // Pushes the character up using the jumping force
            gravityDirection = new Vector3(0, gravityForce, 0);

            // Checks if the character is grounded
            characterIsGrounded = characterController.isGrounded;
        }
        else
        {
            // Disables this boolean to make the character start falling
            isJumping = false;
        }
    }

    protected void GravityPhysics()
    {
        // Checks if there is a ground under the character
        if (!characterIsGrounded && !isJumping)
        {
            // Starts a short clock to determine how long has the player been falling
            _FloatingTime += Time.deltaTime;

            // Sets true to play the falling animation
            isInAir = true;

            // Pulls the character down using the gravity force
            gravityForce = _gravitySet;
            gravityDirection = new Vector3(0, -gravityForce, 0);
        }
        else if (characterIsGrounded)
        {
            // Sets true to disable the falling animation
            isInAir = false;

            // Disables this boolean to make the character start falling
            isJumping = false;

            // Resets the falling time timer
            _FloatingTime = 0;

            // Disables the gravity force when character is grounded
            gravityForce = 0.0f;

            // Resets the jump timer
            jumpDuration = _jumpDurationSet;
        }

        // Moves the character up/down depending on the Y element of the vector
        characterController.Move(gravityDirection * Time.deltaTime);
    }

    private void Animations()
    {
        // Local variable used to check if the Character receives input from Player
        bool _isMoving;
        if (movementDirection != Vector3.zero)
        {
            _isMoving = true;
        } else
        {
            _isMoving = false;
        }

        // Checks if the character is moving while it is grounded
        if (!isInAir && !isJumping && _isMoving && !sprintInput && characterIsGrounded)
        {
            // Plays the Walking animation
            currentAnimation = "Walk";
        }
        else if (!isInAir && _isMoving && sprintInput && characterIsGrounded)
        {
            // Plays the Running animation
            currentAnimation = "Run";
        }
        else if (!characterIsGrounded && isJumping || isInAir)
        {
            if (_FloatingTime > 0.2)
            {
                // Plays the Jumping animation
                currentAnimation = "Jump";
            }
        }
        else if (!_isMoving && canMove && characterIsGrounded)
        {
            // Plays the Idle animation
            currentAnimation = "Idle";
        }
        else
        {
            // Plays the Idle animation
            currentAnimation = "Idle";

            // DEBUG - Prints an error message as this should never happen
            Debug.LogWarning("ERROR 01 - Problem with the animation code!");
        }

        // Plays the desired animation chosen above
        animation.Play(currentAnimation);
    }
}




/*
 * REFERENCES
 * 
 * Character movement system - https://www.youtube.com/watch?v=jiyOZbKRfaY&t=395s
 * 
 */