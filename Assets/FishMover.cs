using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FishMover : MonoBehaviour
{
    public float speed;
    private Vector2 direction;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float margin = 0.05f;
    [SerializeField] private float minDirection = 0.3f;

    void Start()
    {
        speed = Random.Range(0.5f, 1.5f);
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

        if (viewPos.y < (margin + 0.08f))
        {
            direction.y = Mathf.Abs(direction.y);
            bounced = true;
        }
        else if (viewPos.y > 1f - (margin + 0.05f))
        {
            direction.y = -Mathf.Abs(direction.y);
            bounced = true;
        }

        if (bounced)
        {
            direction = direction.normalized;
            if (direction.magnitude < minDirection)
                direction = GetRandomDirection();
            UpdateFacingDirection();
        }
    }

    Vector2 GetRandomDirection()
    {
        Vector2 dir;
        do
        {
            dir = Random.insideUnitCircle.normalized;
        } while (Mathf.Abs(dir.x) < 0.2f && Mathf.Abs(dir.y) < 0.2f);

        return dir;
    }

    void UpdateFacingDirection()
    {
        Vector3 scale = transform.localScale;
        float absX = Mathf.Abs(scale.x);

        scale.x = (direction.x < 0) ? -absX : absX;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            // Flip direction away from the other fish
            Vector2 away = (transform.position - other.transform.position).normalized;
            direction = away;

            // Keep speed consistent
            direction = direction.normalized;

            UpdateFacingDirection();
        }
    }
}
