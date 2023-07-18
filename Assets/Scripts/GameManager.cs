using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(this.gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(menu);
            Destroy(hud);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
    }

    // resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // references
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public Animator deathMenuAnimator;
    public GameObject hud;
    public GameObject menu;

    // logic
    public int pesos;
    public int experience;

    /// <summary>
    /// Max available font size is 27
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="fontSize"></param>
    /// <param name="color"></param>
    /// <param name="position"></param>
    /// <param name="motion"></param>
    /// <param name="duration"></param>
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // upgrade weapon
    public bool TryUpgradeWeapon()
    {
        // if max lvl
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;
        
        // if enough money
        if (pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    // experience system
    public int GetCurrentLevel()
    {
        int level = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[level];
            level++;

            // max level
            if (level == xpTable.Count)
                return level;
        }

        return level;
    }

    public int GetXPToLevel(int level)
    {
        int r = 0;
        int xp = 0;
        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }

    public void GrantXp(int xp)
    {
        int currentLevel = GetCurrentLevel();
        experience += xp;

        if (currentLevel < GetCurrentLevel())
            OnLevelUp();
    }

    private void OnLevelUp()
    {
        player.OnLevelUp();
        OnHitPointChange();
    }

    public void OnHitPointChange()
    {
        float ratio = (float)player.hitPoint / (float)player.maxHitPoint;
        hitpointBar.localScale = new Vector3(1f, ratio, 1);
    }

    // Save state
    /*
    * INT preferedSkin[0]
    * INT pesos[1]
    * INT experience[2]
    * INT weaponLevel[3]
     */

    public void Respawn()
    {
        deathMenuAnimator.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        player.Respawn();
    }

    public void SaveState()
    {
        string saveString = "";

        saveString += "0" + "|";
        saveString += pesos.ToString() + "|";
        saveString += experience.ToString() + "|";
        saveString += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", saveString);
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // examples of data: "0|10|15|2"

        // change player skin
        pesos = int.Parse(data[1]);

        // experience
        experience = int.Parse(data[2]);
        if (GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

        // change the weapon level
        weapon.SetWeaponLevel(int.Parse(data[3]));

        // reset player position
        GameManager.instance.player.transform.position = Vector3.zero;
    }
}
