using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LineRenderer))]
public class BSIOTInput : MonoBehaviour {

    [Header("Object References:")]

    /// <summary>
    /// Player controller object.  Used for player movement
    /// </summary>
    public GameObject playerController;
    [Space(10)]
    /// <summary>
    /// Oculus headset's centerEyeAnchor.  Used for determining forward
    /// direction.
    /// TODO: Consider implimenting a foward vector object like the player
    /// controller, and adding a buffer for player rotation.
    /// </summary>
    public Transform centerEyeAnchor;
    [Space(10)]
    /// <summary>
    /// Object from which the teleport cast originates. (Typ: Right Touch controller)
    /// </summary>
    public Transform teleportPointerOrigin;
    /// <summary>
    /// Marker object which is placed at the end of the teleport arc.
    /// </summary>
    public GameObject teleportMarker;
    /// <summary>
    /// The line renderer that will display the teleport arc.
    /// </summary>
    public LineRenderer teleportLineRenderer;
    /// <summary>
    /// Forward Direction reference
    /// </summary>
    public Transform forwardDirection;


    [Header("Teleport Arc variables:")]
    /// <summary>
    /// the smoothness of the arc
    /// </summary>
    public int teleportArcSmoothness = 40;
    /// <summary>
    /// Color of the arc when on an teleportable surface
    /// </summary>
    public Color validTeleportColor = Color.blue;
    /// <summary>
    /// Color of the arc when not on an teleportable surface
    /// </summary>
    public Color invalidTeleportColor = Color.red;
    /// <summary>
    /// Is the player currently attempting to teleport.
    /// </summary>
    private bool isAttemptingTeleport;
    /// <summary>
    /// Track the last object the teleporter hit.
    /// </summary>
    //Used to prevent unncessary checks for BSITeleportableSurface
    private GameObject lastHitObj;
    /// <summary>
    /// Is the object we are hitting teleportable?
    /// </summary>
    private bool isHitObjTeleportable;

    [Header("Player Movement variables:")]
    /// <summary>
    /// Player movement speed factor
    /// </summary>
    public float movementSpeed = 1.0f;
    /// <summary>
    /// Amount by which the player's height has been modified.
    /// </summary>
    private float playerHeightOffset = 0.0f;

    [Header("Player Rotation variables:")]
    /// <summary>
    /// Is the player rotation smooth, or does it occure in 15deg intervals
    /// </summary>
    public bool isUsingSmoothRotation = true;
    /// <summary>
    /// Player rotation speed factor
    /// </summary>
    public float rotationSpeed = 1.0f;
    /// <summary>
    /// How long (sec) to delay rotation. (for non-smooth rotation)
    /// </summary>
    public float rotationDelay = 1.0f;
    /// <summary>
    /// How long until we're able to rotate again.  (tracks actual time in game)
    /// </summary>
    private float curRotationDelay;

    [Header("In-Game Modifier Keys:")]
    /// <summary>
    /// Key used to speed up player movement
    /// </summary>
    public KeyCode movementSpeedUpKey = KeyCode.PageUp;
    /// <summary>
    /// Key used to slow down player movement
    /// </summary>
    public KeyCode movementSpeedDownKey = KeyCode.PageDown;
    /// <summary>
    /// How much to adjust the movement speed by
    /// </summary>
    public float movementSpeedModifier = .1f;
    [Space(10)]
    /// <summary>
    /// Key used to speed up player rotation
    /// </summary>
    public KeyCode rotationSpeedUpKey = KeyCode.Home;
    /// <summary>
    /// Key used to slow down player rotation
    /// </summary>
    public KeyCode rotationSpeedDownKey = KeyCode.End;
    /// <summary>
    /// How much to adjust the rotation speed by
    /// </summary>
    public float rotationSpeedModifier = .1f;
    [Space(10)]
    /// <summary>
    /// Key used to move the player character up
    /// </summary>
    public KeyCode adjustPlayerHeightUpKey = KeyCode.UpArrow;
    /// <summary>
    /// Key used to move the player character down
    /// </summary>
    public KeyCode adjustPlayerHeightDownKey = KeyCode.DownArrow;
    /// <summary>
    /// Amount to adjust the player height by
    /// </summary>
    public float adjustPlayerHeightAmount = 0.1f;
    [Space(10)]
    /// <summary>
    /// Key used to return the player to the scene's starting position
    /// </summary>
    public KeyCode returnPlayerToStartPositionKey = KeyCode.M;

    [Header("Head Movement Threshold Variables:")]
    [Space(10)]
    /// <summary>
    /// Will the player's head be able to move some before changing forward direction?
    /// </summary>
    public bool useHeadMovementThreshold = false;
    /// <summary>
    /// Button to toggle head movement threshold.
    /// </summary>
    public OVRInput.RawButton headThresholdToggleButton = OVRInput.RawButton.LThumbstickUp;
    /// <summary>
    /// Button to reset forward direction.
    /// </summary>
    public OVRInput.RawButton resetForwardDirectionButton = OVRInput.RawButton.A;


    //Variables for returning the player to the scene's starting position
    /// <summary>
    /// Where the start object in the current scene is located.  Used for returning to it.
    /// </summary>
    private Vector3 sceneStartPosition;

    private void Awake()
    {
        //Subscribe UpdateSceneStartPosition to the OnSceneChanged event
        //so that the start position will update whenever we change scenes
        SceneManager.activeSceneChanged += UpdateSceneStartPosition;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= UpdateSceneStartPosition;
    }


    // Use this for initialization
    void Start () {
        //initial variable assignments
        isAttemptingTeleport = false;
        teleportLineRenderer.enabled = false;
        teleportMarker.SetActive(false);
        lastHitObj = null;
        isHitObjTeleportable = false;

        curRotationDelay = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        /********************************************************************
         * Player teleportation movement.
         * ******************************************************************/
        //Activate or deactivate the teleportation arc. 
        if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) >0.0f && !isAttemptingTeleport)
        {
            isAttemptingTeleport = true;
        }

        if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) <= 0.0f && isAttemptingTeleport)
        {
            isAttemptingTeleport = false;
            teleportLineRenderer.enabled = false;
            teleportMarker.SetActive(false);
        }

        if (isAttemptingTeleport)
        {
            RaycastHit curHit;
            List<Vector3> points;

            BSICurvedRaycast.CurveCast(teleportPointerOrigin.position, teleportPointerOrigin.forward, 
                                        Vector3.down*2.0f, teleportArcSmoothness, out curHit, 30f, out points);

            if(curHit.transform != null)
            {
                teleportLineRenderer.enabled = true;
                teleportLineRenderer.positionCount = points.Count;
                teleportLineRenderer.SetPositions(points.ToArray());

                //Move the teleport marker to the hit point
                teleportMarker.transform.position = curHit.point;

                if(curHit.transform.gameObject != lastHitObj)
                {
                    lastHitObj = curHit.transform.gameObject;
                    //Determine if the object being hit is one the player can teleport to.
                    isHitObjTeleportable = checkForTeleportableScript(lastHitObj);

                    if (!isHitObjTeleportable)
                    {
                        teleportLineRenderer.material.color = invalidTeleportColor;
                        teleportLineRenderer.material.SetColor("_EmissionColor", invalidTeleportColor);
                        //Disable the marker
                        teleportMarker.SetActive(false);
                    } else
                    {
                        teleportLineRenderer.material.color = validTeleportColor;
                        teleportLineRenderer.material.SetColor("_EmissionColor", validTeleportColor);
                        //Enable the teleport marker
                        teleportMarker.SetActive(true);
                    }
                }

                //If the object we're hitting is a teleportable surface,
                //and the player presses A, teleport to that point.
                if (OVRInput.GetUp(OVRInput.Button.One) && isHitObjTeleportable)
                {
                    playerController.transform.position = curHit.point + (Vector3.up*playerHeightOffset);
                }

            } else
            {
                teleportLineRenderer.enabled = false;
                teleportMarker.SetActive(false);
            }
        }


        /********************************************************************
         * Player Joystick movement.
         * 
         * -These movements modify the player controller parent object
         * ******************************************************************/

        //Get the joystick movement data from the joystick of the left Oculus Touch
        Vector2 movJoy = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        if (movJoy.x < -.1f || movJoy.x > 0.1f || movJoy.y < -.1f || movJoy.y > 0.1f)
        {
            Vector3 dir = Vector3.zero;

            if (useHeadMovementThreshold)
            {
                dir = forwardDirection.forward * movJoy.y * movementSpeed * Time.deltaTime;
                dir += forwardDirection.right * movJoy.x * movementSpeed * Time.deltaTime;
            } else
            {
                dir = centerEyeAnchor.forward * movJoy.y * movementSpeed * Time.deltaTime;
                dir += centerEyeAnchor.right * movJoy.x * movementSpeed * Time.deltaTime;
            }

            dir.y = 0.0f; //The player should not move vertically
            playerController.transform.position += dir;

        }

        /********************************************************************
         * Player Joystick rotation.
         * 
         * -These movements modify the player controller parent object
         * ******************************************************************/

        //Get the joystick data from the joystick on the right oculus controller.
        Vector2 rotJoy = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        //Center clicking the right joystick modified rotation type
        if (OVRInput.GetUp(OVRInput.RawButton.RThumbstick))
        {
            isUsingSmoothRotation = !isUsingSmoothRotation;
        }

        if (isUsingSmoothRotation)
        {
            if(rotJoy.x < -0.1f || rotJoy.x > 0.1f)
            {
                Vector3 rotEul = playerController.transform.rotation.eulerAngles;

                Vector3 newRot = new Vector3(rotEul.x,
                                             rotEul.y + (rotJoy.x * rotationSpeed * Time.deltaTime),
                                             rotEul.z);
                playerController.transform.rotation = Quaternion.Euler(newRot);
            }
        } else
        {
            if(rotJoy.x < -0.9f && curRotationDelay <= 0.0f)
            {
                //Rotate the camera by -15 degrees
                Vector3 rotEul = playerController.transform.rotation.eulerAngles;

                Vector3 newRot = new Vector3(rotEul.x, rotEul.y -15, rotEul.z);
                playerController.transform.rotation = Quaternion.Euler(newRot);

                //Set the rotation delay
                curRotationDelay = rotationDelay;
            }

            if(rotJoy.x > 0.9f && curRotationDelay <= 0.0f)
            {
                //Rotate the camera by -15 degrees
                Vector3 rotEul = playerController.transform.rotation.eulerAngles;

                Vector3 newRot = new Vector3(rotEul.x, rotEul.y + 15, rotEul.z);
                playerController.transform.rotation = Quaternion.Euler(newRot);

                //Set the rotation delay
                curRotationDelay = rotationDelay;
            }

            //If the rotation joystick returns to neutral, reset delay time:
            if(rotJoy.x > -0.1f && rotJoy.x < 0.1f)
            {
                curRotationDelay = 0.0f;
            }

            //Count down rotation delay
            if(curRotationDelay > 0.0f)
            {
                curRotationDelay -= Time.deltaTime;
                curRotationDelay = Mathf.Clamp01(curRotationDelay);
            }
        }

        /********************************************************************
         * Variable adjustment
         * ******************************************************************/

        if (Input.GetKeyDown(movementSpeedUpKey))
        {
            movementSpeed += movementSpeedModifier;
        }
        if (Input.GetKeyDown(movementSpeedDownKey))
        {
            movementSpeed -= movementSpeedModifier;
        }
        if (Input.GetKeyDown(rotationSpeedUpKey))
        {
            rotationSpeed += rotationSpeedModifier;
        }
        if (Input.GetKeyDown(rotationSpeedDownKey))
        {
            rotationSpeed -= rotationSpeedModifier;
        }
        if (Input.GetKeyDown(adjustPlayerHeightUpKey))
        {
            adjustPlayerHeight(adjustPlayerHeightAmount);
        }
        if (Input.GetKeyDown(adjustPlayerHeightDownKey))
        {
            adjustPlayerHeight(-adjustPlayerHeightAmount);
        }

        //toggle head movement threshold
        if (OVRInput.GetUp(headThresholdToggleButton))
        {
            useHeadMovementThreshold = !useHeadMovementThreshold;
        }

        //Reset forward direction 'forward'
        if (OVRInput.GetUp(resetForwardDirectionButton))
        {
            BSIUpdateForwardRotation fwdDir = forwardDirection.GetComponent<BSIUpdateForwardRotation>();

            if(fwdDir != null)
            {
                fwdDir.ResetForwardDirection();
            }
        }

        /********************************************************************
         * Reset Player Position
         * ******************************************************************/
        if (Input.GetKeyDown(returnPlayerToStartPositionKey))
        {
            returnPlayerToStartPosition();
        }

    }

    /// <summary>
    /// Is the hit object defined as a surface to which the player can teleport?
    /// </summary>
    /// <param name="hitObj">The object being checked.</param>
    /// <returns>True if the object has BSITeleportableSurface script attached</returns>
    private bool checkForTeleportableScript(GameObject hitObj)
    {
        if (hitObj.GetComponent<BSITeleportableSurface>() != null) return true;
        return false;
    }


    public void adjustPlayerHeight(float amnt)
    {
        playerHeightOffset += amnt;

        playerController.transform.position += Vector3.up * amnt;
        //Modify the player collider's position to acocunt for the height change
        playerController.GetComponent<SphereCollider>().center -= new Vector3(0.0f, amnt, 0.0f);
    }


    public void UpdateSceneStartPosition(Scene oldScene, Scene newScene)
    {
        if (newScene.name == "Player") return;

        //Look for an object in the scene called "BSIStartPos"  This is a prefab.
        GameObject start = GameObject.Find("BSIStartPos");

        sceneStartPosition = start.transform.position;
    }

    public void returnPlayerToStartPosition()
    {
        playerController.transform.position = sceneStartPosition + (Vector3.up*playerHeightOffset);
    }
}
