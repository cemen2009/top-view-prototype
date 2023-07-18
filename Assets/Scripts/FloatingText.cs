using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    [HideInInspector]
    public bool active;
    public GameObject go;
    
    // need to assign in FloatingTextManager.cs
    public Text txt;
    // assigned here
    public Vector3 motion;
    public float duration;
    public float lastShown;
    
    public void Show()
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
    }

    public void Hide()
    {
        active = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText()
    {
        if (!active)
            return;

        if (Time.time - lastShown > duration)
            Hide();

        go.transform.position += motion * Time.deltaTime;
    }
}
