using UnityEngine;

public class Chest : Collectable
{
    [SerializeField] private Sprite emptyChest;
    public int pesosAmount;

    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.pesos += pesosAmount;
            GameManager.instance.ShowText("+" + pesosAmount + " pesos!", 20, Color.yellow, this.transform.position, Vector3.up * 35, 1.2f);
        }
    }
}
