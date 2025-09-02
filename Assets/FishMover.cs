using UnityEngine;

public class FishMover : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 direction;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float margin = 0.05f;
    [SerializeField] private float minDirection = 0.3f; // Avoids zero movement

    void Start()
    {
        mainCamera = Camera.main;
        direction = GetRandomDirection();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateFacingDirection();
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        KeepWithinBounds();
    }

    void KeepWithinBounds()
    {
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
        bool bounced = false;

        // Horizontal bounds (unchanged)
        if (viewPos.x < margin)
        {
            direction.x = Mathf.Abs(direction.x);
            bounced = true;
        }
        else if (viewPos.x > 1f - margin)
        {
            direction.x = -Mathf.Abs(direction.x);
            bounced = true;
        }

        // Vertical bounds
        if (viewPos.y < margin) // Bottom bound
        {
            direction.y = Mathf.Abs(direction.y);
            bounced = true;
        }
        else if (viewPos.y > 1f - (margin + 0.05f)) // Top bound is smaller
        {
            direction.y = -Mathf.Abs(direction.y);
            bounced = true;
        }

        if (bounced)
        {
            direction = direction.normalized;
            if (direction.magnitude < minDirection)
                direction = GetRandomDirection(); // Prevent stuck
            UpdateFacingDirection();
        }
    }


    Vector2 GetRandomDirection()
    {
        Vector2 dir;
        do
        {
            dir = Random.insideUnitCircle.normalized;
        } while (Mathf.Abs(dir.x) < 0.2f && Mathf.Abs(dir.y) < 0.2f); // Avoid flat or near-zero vectors

        return dir;
    }

    void UpdateFacingDirection()
    {
        // Always keep the absolute original scale
        Vector3 scale = transform.localScale;
        float absX = Mathf.Abs(scale.x);

        if (direction.x < 0)
            scale.x = -absX; // Facing left
        else
            scale.x = absX;  // Facing right

        transform.localScale = scale;
    }

}
