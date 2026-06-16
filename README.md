Unity project - Group work for our University assignment.
3rd person adventure game, with turn-based combat. 

Team members:
Lukas Kubinec (myself) - Character movement, character animations system, 3D model rigs+animated using Mixamo, Save/Load game system.
Samuel Begley - Turn-Based Combat scripting.
Elise Holcroft - Game UI & UX, music & music system. 
Matthew McKay - Level design, environment art, puzzles game system. 

Introduction
Introduction outlining game concept and techniques used.
The Report (Word or PDF document) describing the game in detail, including its
overall design, implementation, screenshots, and integration testing. Include brief
instructions on how to run the game and interact with it (game GUI). Provide
background information based on academic sources
Concept, Design & Development
Background and context for your game (highlight any sources of inspiration).
Design and development of a minimum of two main development tasks per
student. Include screenshots of every feature.

Pre-Production
Mood boards

Mind Maps

Story board

Gameplay Loop

Matthew Mckay
I was responsible for creating the levels, and the puzzles that could be found in each of them. 
We chose to only use low-poly models for the game as they will take up less storage and are easier to change/find on the Unity Asset Store. I created all the physical environments for levels, which are the Forest of Echoes, Mountain of Despair, Cave of Illusions, Castle of the Final Battle, and all the miniature versions of the levels that show up in battles. I also added particle effects to some areas. To create the effects I made (not including the fireflies), I had to teach myself about particle effects, such as ‘Size over time’, ‘Trails’, etc. I created all these environments on terrain, which I first painted, then terraformed using unique brushes to create a more natural look, then added trees to. For these trees, I chose to make them billboards as they will use up less memory when the player isn’t close to them, lowering the lag of the game (the fog also helps cover up the look of billboarded items). After creating these terrains, I also added some additional models, such as the village market in the forest, which adds to the immersion. I created ambient spaces for each level with lighting and fog to create a more immersive gameplay experience. There are also some hidden areas scattered across the map that players can find. They can be found off paths that look like they shouldn’t be accessible but still have hints to show they are accessible. This includes the hidden giant red crystal in the forest, and the upside-down room in the cave. 
I also created the puzzle found in the Cave of Illusions, which required coding to teleport the player to that scene, and I coded the actual completion of the puzzle. It is a basic drag and drop puzzle, that is necessary for the player to move onto the boss.
I also performed testing on my maps, and the puzzle. Overall, the maps work well as the player can easily traverse through them and still allows for exploration. All the lighting, fog and particle systems all work as expected.

Elise Holcroft
I was responsible for creating the user interfaces in the project, as well as sound design. I also, alongside the rest of the group, contributed to pre-production elements, such as the mind maps and mood boards we created, as well as making a simple storyboard. 


Samuel Begley
I was responsible for creating the turn-based combat system.


Lukas Kubinec
My responsibilities were to create a basic character movement system, with the ability to sprint and jump. The movement system uses the Unity built-in Character Controller, which already comes with many essential features, that I could use without having to create them from scratch. The drawback of this system however is that it does not apply physics to the character, therefore that had to be programmed by me. This allowed me to make a robust movement system, which is also switching the animation of the characters based on their movements and states.
I was also tasked with allowing the character to pick up collectibles and also the saving and loading system, which would allow the player to move between levels without loosing their progress. 
Source code
Source code for all scripts developed, with a discussion of how and why they
were created (in addition to the information in the comments).
Comments in the code which clearly define the role of the code.

Character movement script - “PlayerMovement3D.cs” WIP
This C# code was written to allow the player to take control of the character. This code handles the player input, allowing the character to move around, jump and sprint, while also switching the character animations based on the current character action. 
X
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
        else if (characterIsGrounded && !canMove)
        {
            // DEBUG TEST - Plays the Attack animation when movement is disabled
            currentAnimation = "Attack";
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
 
    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
 
/*
* REFERENCES
* Character movement system - https://www.youtube.com/watch?v=jiyOZbKRfaY&t=395s
*/


Player collectibles script – “PlayerCollectiblesController.cs” WIP
To allow the player to collect items in the world, this C# code was written. The code must be attached to the player prefab, and the code functions by waiting until the player enters a trigger, and compares the tag of the triggered object, if it matches the “Collectible” tag...!!!
X
using TMPro;
using UnityEngine;
 
public class PlayerCollectiblesController : MonoBehaviour
{
 
    // Player's Collectible Counter
    private int score = 0;
 
    // UI Elements
    public TextMeshProUGUI CollectibleCounter;
 
 
    private void Start()
    {
        // Updates the UI Score at the beginning of the game
        CollectibleCounter.text = "Score: " + score.ToString();
    }
 
 
    // Function that checks if the Player entered a trigger
    private void OnTriggerEnter(Collider other)
    {
        // Compares the Trigger Tag
        if (other.CompareTag("Collectible"))
        {
            // Adds one to Player's score
            score++;
 
            // Updates the UI Score text
            CollectibleCounter.text = "Score: " + score.ToString();
 
            // Destroys the collectible object
            GameObject.Destroy(other.gameObject);
        }
    }
}

Puzzle Script – ‘Puzzle3’ – Matthew Mckay - WIP
This is the script for the puzzle found in the Cave of Illusions scene. The puzzle is a drag and drop, where the user needs to place the correctly coloured crystal in the right spot, shown by a faded version of the crystal. This uses user mouse input.

using UnityEngine;

public class Puzzle3 : MonoBehaviour
{
    //These 2 define the 2 objects in the game that will be selected.
    public GameObject ObjectPlace;
    public GameObject ObjectAnswer;
    //This is the distance the object has from the answer that will then lock it into place if correct.
    public float DropDistance;
    //This will lock the object in place after the correct answer.
    private bool islocked;
    Vector3 ObjectStart;

    void Start()
    {
        //This will save the ObjectStart as the in-game ObjectPlaces starting position.
        ObjectStart = ObjectPlace.transform.position;
    }

    public void DragObject()
    {
        //If the object isn't answered/not locked.
        if(islocked == false)
        {
            //The object will move with the mouse input.
            ObjectPlace.transform.position = Input.mousePosition;
        }
    }

    public void DropObject()
    {
        //This is the distance from the answer object.
        float Distance = Vector3.Distance(ObjectPlace.transform.position, ObjectAnswer.transform.position);
        //If the Distance is further away from the answer than the DropDistance
        if(Distance < DropDistance)
        {
            //Then the object becomes locked, and the ObjectPlace becomes the ObjectAnswers position.
            islocked = true;
            ObjectPlace.transform.position = ObjectAnswer.transform.position;
        }
        else
        {
            //Else, the ObjectPlace returns to its original position.
            ObjectPlace.transform.position = ObjectStart;
        }
    }
    
    void Update () {
        //The game will constantly be checking to see if the player has completed the puzzle, which will load a congrats text.
    }
}


Testing
Fully documented testing strategy and a discussion of the outcomes
obtained.
Test Type	How it will be tested	Expected Outcome	Actual Outcome	Alterations	Tested by:
Environment	Using the player model, it will be controlled through the environments to ensure they are working properly.	All scenes should work fine, with no clipping/visual errors occurring.	Due to the trees being billboarded, their collision boxes are also removed, which means in the ‘Forest of Echoes’ and ‘Mountain of Despair’, the trees placed on the terrain will allow the player to go through them, which is an issue as they are also supposed to be walls that won’t allow the player to fall off the map.	To fix this, invisible walls will be placed all over the map which will not allow players to pass through. 	Matthew Mckay
Lighting	Using the player model, I will travel through the worlds and ensure the lighting is okay in each level.	All lighting should work appropriately.	The lighting works fine in all levels and creates nice ambience particularly in the ‘Cave of Illusions’. The fog also works as planned. However, the lighting on the ‘Mountain of Despair’ is very dark, and the fog makes it hard to traverse the level.	Will provide more lighting to the ‘Mountain of Despair’.	Matthew Mckay
Particle Effects	In-game, I will check all the particle systems.	All particles should work as expected.	The particles all work fine, which is in the forest (falling leaves), mountain (snowfall, campfire), cave (fake crystal glow) and castle (red floor particles).	N/A	Matthew Mckay


References
Ketra Games (2023). Creating a Third Person Camera (Unity Tutorial). [online] Available at: https://www.youtube.com/watch?v=jiyOZbKRfaY 
[Accessed 01 Dec. 2024].
Clip Collection Vault (2022). Unity Puzzle Game Drag & Drop (Tutorial 2022). [online] Available at: https://www.youtube.com/watch?v=c_OXvAGodlo [Accessed 6th Jan. 2025].
Unity Assets
VanillaArt (2023). Low-Poly Medieval Market. [online] Available at: https://assetstore.unity.com/packages/3d/environments/low-poly-medieval-market-262473 [Accessed 20th Dec. 2024].
Render Knight (2024). Fantasy Skybox FREE. [online] Available at: https://assetstore.unity.com/packages/2d/textures-materials/sky/fantasy-skybox-free-18353 [Accessed 20th Dec. 2024].
Polytope Studio (2025). Lowpoly Environment – Nature Free – MEDIEVAL FANTASY SERIES. [online] Available at: https://assetstore.unity.com/packages/3d/environments/lowpoly-environment-nature-free-medieval-fantasy-series-187052 [Accessed 23rd Dec. 2024].
Nebula (2024). Low poly trees - free nature pack. [online] Available at: https://assetstore.unity.com/packages/3d/vegetation/trees/low-poly-trees-free-nature-pack-300824 [Accessed 27th Dec. 2024].
Chromisu (2023). Handpainted Grass & Ground Textures. [online] Available at: https://assetstore.unity.com/packages/2d/textures-materials/nature/handpainted-grass-ground-textures-187634 [Accessed 27th Dec. 2024].
Lumo-Art 3D (2023). FREE Stylized PBR Textures Pack. [online] Available at: https://assetstore.unity.com/packages/2d/textures-materials/free-stylized-pbr-textures-pack-111778 [Accessed 28th Dec. 2024].
Papersy (2021). Low Poly Mushrooms Pack. [online] Available at: https://assetstore.unity.com/packages/3d/vegetation/low-poly-mushrooms-pack-205460 [Accessed 3rd Jan. 2025].
ClayManStudio (2018). Toon Crystals Pack. [online] Available at: https://assetstore.unity.com/packages/3d/props/toon-crystals-pack-66182 [Accessed 3rd Jan. 2025].
SineVFX (2024). Translucent Crystals. [online] Available at: https://assetstore.unity.com/packages/3d/props/toon-crystals-pack-66182 [Accessed 3rd Jan. 2025].
ALP (2019). Terrain Textures Pack Free. [online] Available at: https://assetstore.unity.com/packages/2d/textures-materials/nature/terrain-textures-pack-free-139542 [Accessed 3rd Jan. 2025].
Bauervision (2017). PBR Texture Lib. [online] Available at: https://assetstore.unity.com/packages/2d/textures-materials/pbr-texture-lib-82799 [Accessed 3rd Jan. 2025].
Stylized Labs (2022). Stylized Fantasy: Props Sample. [online] Available at: https://assetstore.unity.com/packages/3d/props/stylized-fantasy-props-sample-234139 [Accessed 4th Jan. 2025].
GAPH (2021). 48 Particle Effect Pack. [online] Available at: https://assetstore.unity.com/packages/vfx/particles/spells/48-particle-effect-pack-13998 [Accessed 4th Jan. 2025].
Rad-Coders (2017). Low Poly: Foliage. [online] Available at: https://assetstore.unity.com/packages/3d/vegetation/low-poly-foliage-66638 [Accessed 4th Jan. 2025].
IgniteCoders (2021). Simple Water Shader URP. [online] Available at: https://assetstore.unity.com/packages/2d/textures-materials/water/simple-water-shader-urp-191449 [Accessed 4th Jan. 2025].
Mana Station (2021). Alchemy Lab Props. [online] Available at: https://assetstore.unity.com/packages/2d/textures-materials/water/simple-water-shader-urp-191449 [Accessed 5th Jan. 2025].
Runemark Studio (2020). Dark Fantasy Kit [Lite]. [online] Available at: https://assetstore.unity.com/packages/3d/environments/fantasy/dark-fantasy-kit-lite-127925 [Accessed 6th Jan. 2025].
HQP Studios (2024). Rocks and Terrains Pack – Low Poly. [online] Available at: https://assetstore.unity.com/packages/3d/environments/rocks-and-terrains-pack-low-poly-281733 [Accessed 6th Jan. 2025].
Broken Vector (2022). Ultimate Low Poly Dungeon. [online] Available at: https://assetstore.unity.com/packages/3d/environments/dungeons/ultimate-low-poly-dungeon-143535 [Accessed 6th Jan. 2025].
‌
