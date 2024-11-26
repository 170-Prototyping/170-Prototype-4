using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InstantiateAI : MonoBehaviour
{
    public GameObject aiCharacterPrefab;
    public float spawnInterval = 1.0f;
    public Vector3 spawnMinBounds = new Vector3(-80, 2, -80);
    public Vector3 spawnMaxBounds = new Vector3(80, 2, 80);
    public Vector2 exclusionBoundsX = new Vector2(-50, 50); // Excluded X range
    public Vector2 exclusionBoundsZ = new Vector2(-50, 50); // Excluded Z range

    private void Start()
    {
        InvokeRepeating(nameof(SpawnAICharacter), 0, spawnInterval);
    }

    private void SpawnAICharacter()
    {
        if (aiCharacterPrefab == null)
        {
            Debug.LogError("AICharacter prefab is not assigned.");
            return;
        }
        Vector3 randomPosition;
        do
        {
            randomPosition = new Vector3(
                Random.Range(spawnMinBounds.x, spawnMaxBounds.x),
                spawnMinBounds.y,
                Random.Range(spawnMinBounds.z, spawnMaxBounds.z)
            );
            
        } while (IsWithinExclusionZone(randomPosition));
        Instantiate(aiCharacterPrefab, randomPosition, Quaternion.identity);
    }

    private bool IsWithinExclusionZone(Vector3 position)
    {
        return position.x > exclusionBoundsX.x && position.x < exclusionBoundsX.y &&
               position.z > exclusionBoundsZ.x && position.z < exclusionBoundsZ.y;
    }
}
