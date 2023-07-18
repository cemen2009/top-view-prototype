using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private Transform lookAt;
    private float boundX = 0.32f;
    private float boundY = 0.16f;

    private void Start()
    {
        lookAt = GameObject.Find("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 delta = Vector3.zero;

        float deltaX = lookAt.position.x - this.transform.position.x;
        if (deltaX > boundX)
            delta.x = deltaX - boundX;
        else if (deltaX < -boundX)
            delta.x = deltaX + boundX;

        float deltaY = lookAt.position.y - this.transform.position.y;
        if (deltaY > boundY)
            delta.y = deltaY - boundY;
        else if (deltaY < boundY)
            delta.y = deltaY + boundY;

        transform.position += new Vector3(delta.x, delta.y, 0f);

        // if not inside bounds -> move towards lookAt.position (player position)
        /*if ((Mathf.Abs(deltaX) > boundX) || (Mathf.Abs(deltaY) > boundY))
        {
            Vector3 deltaMove = Vector3.MoveTowards(this.transform.position, lookAt.position, speed * Time.deltaTime);
            deltaMove.z = -10f;
            transform.position = deltaMove;
        }*/
    }
}
