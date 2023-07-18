using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // damage struct
    public int[] damagePoint = { 1, 2, 4, 8, 16, 32, 64};
    public float[] pushForce = { 1.5f, 1.7f, 1.9f, 2.1f, 2.4f, 2.7f, 3f};

    // upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    // swing
    private Animator animator;
    private float cooldown = 0.5f;
    private float lastSwing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>(); 
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Fire2"))
        {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.CompareTag("Fighter"))
        {
            if (coll.name == "Player")
                return;

            // create new Damage object, then we'll send it to the fighter we've hit
            Damage dmg = new Damage
            {
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForce = pushForce[weaponLevel]
            };

            coll.SendMessage("ReceiveDamage", dmg);
        }
    }

    private void Swing()
    {
        animator.SetTrigger("Swing");
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }
}
