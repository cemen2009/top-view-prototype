using UnityEngine;

public class NPCTextPerson : Collidable
{
    [SerializeField] private string message;

    private float cooldown = 4f;
    private float lastShout;

    protected override void Start()
    {
        base.Start();
        lastShout = -cooldown;
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (Time.time - lastShout > cooldown)
        {
            lastShout = Time.time;
            GameManager.instance.ShowText(message, 25, Color.white, this.transform.position + Vector3.up * 0.16f , Vector3.zero, 4f);
        }
    }
}
