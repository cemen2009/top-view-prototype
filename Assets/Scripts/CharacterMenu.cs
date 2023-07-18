using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    // text fields
    public Text levelText, hitpointText, pesosText, upgradeCostText, xpText;

    // logic
    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    // character slection
    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;

            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;

            //currentCharacterSelection = currentCharacterSelection == GameManager.instance.playerSprites.Count - 1 ? currentCharacterSelection + 1 : 0;
            OnSelectionChanged();
        }
        else
        {
            currentCharacterSelection--;

            if (currentCharacterSelection < 0)
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;

            //currentCharacterSelection = currentCharacterSelection < 0 ? GameManager.instance.playerSprites.Count - 1 : currentCharacterSelection--;
            OnSelectionChanged();
        }
    }

    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    // weapon upgrade
    public void OnUpgradeClick()
    {
        // TODO: upgrade weapon via reference
        if (GameManager.instance.TryUpgradeWeapon())
            UpdateMenu();
    }

    // upgrade the character information
    public void UpdateMenu()
    {
        // weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        // check is it max price
        upgradeCostText.text = GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count ? 
            "MAX" : GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();

        // meta
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.player.hitPoint.ToString() + "/" + GameManager.instance.player.maxHitPoint.ToString();
        pesosText.text = GameManager.instance.pesos.ToString();

        // xp Bar
        int currentLevel = GameManager.instance.GetCurrentLevel();
        if (currentLevel == GameManager.instance.xpTable.Count)
        {
            // max level
            xpText.text = GameManager.instance.experience.ToString() + " total experience points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int previousLevelXp = GameManager.instance.GetXPToLevel(currentLevel - 1);
            int currentLevelXp = GameManager.instance.GetXPToLevel(currentLevel);

            int difference = currentLevelXp - previousLevelXp;
            int currentXPIntoLevel = GameManager.instance.experience - previousLevelXp;

            float completionRatio = (float)currentXPIntoLevel / (float)difference;
            xpBar.localScale = new Vector3(completionRatio, 1f, 1f);

            xpText.text = currentXPIntoLevel.ToString() + "/" + difference.ToString();
        }
    }
}
