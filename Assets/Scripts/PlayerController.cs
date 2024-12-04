using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    public float playerSpeed = 6.5f;
    public float rotationSpeed = 720f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;

    public int trashCount = 0; // Current amount of trash the player is carrying
    public int maxTrash = 5; // Maximum pieces of trash the player can carry
    public Transform trashCan; // Reference to the trash can object for depositing

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Movement and gravity (existing logic)
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (move.magnitude > 0)
        {
            move.Normalize();
            transform.forward = Vector3.Slerp(transform.forward, move.normalized, Time.deltaTime * rotationSpeed);
        }
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime * 4.0f;
        controller.Move(playerVelocity * Time.deltaTime * 4.0f);

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
        if (Vector3.Distance(transform.position, trashCan.position) < 2f && trashCount > 0)
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
