using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SOURCE: my (Riley) code from CMPM 125 project
public class PlayerControllerNew : MonoBehaviour
{

    [SerializeField] private MovementSettings movementSettings = new MovementSettings();
    [SerializeField] private MouseSettings mouseSettings = new MouseSettings();
    private InternalVars internalVars = new InternalVars();
    private Rigidbody body;
    private Transform cameraTransform;
    public int trashCount = 0; // Current amount of trash the player is carrying
    public int maxTrash = 5; // Maximum pieces of trash the player can carry
    public Transform trashCan; // Reference to the trash can object for depositing
    public Transform trashCan2;


    private void cameraInput()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSettings.mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSettings.mouseSensitivity * Time.fixedDeltaTime;
        internalVars.xRotation -= mouseY;
        internalVars.xRotation = Mathf.Clamp(internalVars.xRotation, -90f, 90f);

        internalVars.yRotation += mouseX;
        cameraTransform.localRotation = Quaternion.Euler(internalVars.xRotation, internalVars.yRotation, 0f);
    }

    private void releasedJumpAndGravity()
    {
        
        body.AddForce(Vector3.down * movementSettings.gravity * 2);
    }

    private void xyMovementInput() {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

        Vector3 moveDirection = (cameraRight * moveHorizontal + cameraForward * moveVertical).normalized;

        Vector3 targetVelocity;
        if(Input.GetButton("Sprint")) targetVelocity = moveDirection * (movementSettings.maxSpeed * 1.5f);
        else targetVelocity = moveDirection * movementSettings.maxSpeed;
        Vector3 currentVelocity = body.velocity;
        Vector3 newVelocity = Vector3.Lerp(currentVelocity, targetVelocity, movementSettings.accelerationFactor);
        body.velocity = new Vector3(newVelocity.x, currentVelocity.y, newVelocity.z); // Preserve vertical velocity
        
        body.velocity = new Vector3(newVelocity.x * movementSettings.groundedTraction, currentVelocity.y, newVelocity.z * movementSettings.groundedTraction);

    }


    private void cancelAngularVelocity()
    {
        body.angularVelocity = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();//grabbing reference to the rigidbody
        Cursor.lockState = CursorLockMode.Locked;//cursor lock
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        cameraInput();
        // Trash depositing logic
        if (Input.GetKeyDown(KeyCode.E)) // Press 'E' to deposit trash
        {
            DepositTrash();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if colliding with trash objects
        if (other.CompareTag("Trash") && trashCount < maxTrash)
        {
            PickUpTrash(other.gameObject);
        }
    }

    private void FixedUpdate()
    {
        releasedJumpAndGravity();
        xyMovementInput();
        cancelAngularVelocity();
    }

    void PickUpTrash(GameObject trash)
    {
        trashManager.instance.pickTrash();
        trashCount++;
        
        Destroy(trash);
        Debug.Log("Picked up trash. Current count: " + trashCount);
    }

    void DepositTrash()
    {
        // Check if near the trash can
        if (Vector3.Distance(transform.position, trashCan.position) < 5f && trashCount > 0)
        {
            Debug.Log("Deposited " + trashCount + " pieces of trash!");
            trashCount = 0; // Reset the count after depositing
            trashManager.instance.depositTrash();
        }
        else if (trashCount == 0)
        {
            Debug.Log("No trash to deposit.");
        }
        else
        {
            Debug.Log("Move closer to the trash can to deposit.");
        }
    }
}

[System.Serializable]
public class MovementSettings
{
    public float accelerationFactor = 0.35f;
    public float groundedTraction = 0.5f;
    public float aerialTraction = 0.35f;
    public float maxSpeed = 40f;
    //public float walkPower = 20.0f;
    public float gravity = 200f;
}

[System.Serializable]
public class MouseSettings
{
    public float mouseSensitivity = 100f;
}
public class InternalVars
{
    public float xRotation = 0f;
    public float yRotation = 0f;
}


