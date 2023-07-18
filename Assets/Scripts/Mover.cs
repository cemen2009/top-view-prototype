using UnityEngine;

public abstract class Mover : Fighter
{
    private Vector3 originalScale;

    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;

    [SerializeField] protected float ySpeed = 0.75f;
    [SerializeField] protected float xSpeed = 1f;

    protected virtual void Start()
    {
        originalScale = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        // reset moveDelta
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0f);

        // swap sprite direction, whether you're going right or left
        if (moveDelta.x > 0)
            transform.localScale = originalScale;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        // add push vector, if any
        moveDelta += pushDirection;
        //reduce push force for every fixed frame, based off recovery speed
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        // make sure we can move in this direction, by casting a box there first, if the box return null, we're free to move
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.fixedDeltaTime), LayerMask.GetMask("Blocking", "Actor"));
        if (hit.collider == null)
        {
            // can move
            transform.Translate(0f, moveDelta.y * Time.fixedDeltaTime, 0f);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0f), Mathf.Abs(moveDelta.x * Time.fixedDeltaTime), LayerMask.GetMask("Blocking", "Actor"));
        if (hit.collider == null)
        {
            // can move
            transform.Translate(moveDelta.x * Time.fixedDeltaTime, 0f, 0f);
        }
    }
}
