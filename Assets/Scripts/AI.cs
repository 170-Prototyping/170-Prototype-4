using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    // Start is called before the first frame update
    public string dir = "";
    public float speed = 8.0f;
    [SerializeField] private GameObject[] trashList;
    private float spawnTimer = 5.0f;
    void Start()
    {
        float pos_x = gameObject.transform.position.x;
        float pos_z = gameObject.transform.position.z;
        if (pos_x < -50) dir = "right";
        else if (pos_x > 50) dir = "left";
        else if (pos_z < -50) dir = "up";
        else if (pos_z > 50) dir = "down";


                if (dir == "right")
                {
                    transform.Rotate(0, 90, 0);
                }
                else if (dir == "left")
                {
                    transform.Rotate(0, -90, 0);
                }
                else if (dir == "up")
                {
                    transform.Rotate(0, 0, 0);
                }
                else if (dir == "down")
                {
                    transform.Rotate(0, 180, 0);
                }
    }
    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnTrash();
        }
        //move the AI in the direction it was instantiated
        if (dir == "right")
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        }
        else if (dir == "left")
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        }
        else if (dir == "up")
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
        }
        else if (dir == "down")
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
        }


        //if the AI falls below the y-axis, destroy it
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnTrash()
    {
        spawnTimer = Random.Range(4.0f, 5.0f);
        int index = Random.Range(0, trashList.Length);
        GameObject trash = Instantiate(trashList[index], transform.position, Quaternion.identity);
        Rigidbody rb = trash.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float randomX = Random.Range(-7.0f, 7.0f);
            float randomY = Random.Range(-7.0f, 7.0f);
            float randomZ = Random.Range(-7.0f, 7.0f);
            Vector3 randomVelocity = new Vector3(randomX, randomY, randomZ);
            rb.velocity = randomVelocity;
        }
    }
}
