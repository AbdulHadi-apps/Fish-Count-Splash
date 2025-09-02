using UnityEngine;

public class NumberMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotationSpeed = 30f; // slow rotation
    public Vector2 movingObjects;
    void Update()
    {
        transform.Translate(movingObjects * moveSpeed * Time.deltaTime);
        // transform.Rotate(0f, rotationSpeed * Time.deltaTime, 15f * Time.deltaTime); // slight rotation
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

 

}
