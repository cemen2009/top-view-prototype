using UnityEngine;

public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    private bool isAlive = true;

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;

        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitPointChange();
    }

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (isAlive)
            UpdateMotor(new Vector3(x, y, 0));
    }

    public void SwapSprite(int skinIndex)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinIndex];
    }

    public void OnLevelUp()
    {
        maxHitPoint += 2;
        hitPoint = maxHitPoint;
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
            OnLevelUp();
    }

    public void Heal(int healingAmount)
    {
        if (hitPoint == maxHitPoint)
            return;

        hitPoint += healingAmount;

        if (hitPoint > maxHitPoint)
            hitPoint = maxHitPoint;

        GameManager.instance.ShowText("+" + healingAmount.ToString() + "HP", 25, Color.green, transform.position, Vector3.up * 30, 1f);
        GameManager.instance.OnHitPointChange();
    }

    protected override void Death()
    {
        GameManager.instance.deathMenuAnimator.SetTrigger("Show");
        isAlive = false;
    }

    public void Respawn()
    {
        Heal(maxHitPoint);
        isAlive = true;
        immuneTime = Time.time;
        pushDirection = Vector3.zero;
    }
}
