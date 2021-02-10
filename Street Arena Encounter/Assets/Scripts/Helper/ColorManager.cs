using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public enum EColorObjectType
{
    BackGround,
    Header,
    Element0,
    Element1,
    Element2,
    Text0,
    Text1,
    Text2,
}
[Serializable]
struct SColorObject
{
    public GameObject go;
    public EColorObjectType cot;
}

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get; private set; }
    [SerializeField]
    List<SColorObject> list = new List<SColorObject>();
    [SerializeField]
    List<Color> colors = new List<Color>(sizeof(EColorObjectType));

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnValidate()
    {
        UpdateColors();
    }

    void Update()
    {
        UpdateColors();
    }

    void UpdateColors()
    {
        foreach (var item in list)
        {
            if (item.go.GetComponent<Image>() != null)
                item.go.GetComponent<Image>().color = colors[(int)item.cot];
            if (item.go.GetComponent<TextMeshProUGUI>() != null)
                item.go.GetComponent<TextMeshProUGUI>().color = colors[(int)item.cot];
            if (item.go.GetComponent<Button>() != null)
            {
                ColorBlock cb = new ColorBlock();
                Color c = colors[(int)item.cot];
                cb.normalColor = new Color(c.r, c.g, c.b, 0);
                cb.highlightedColor = c;
                cb.pressedColor = c;
                cb.selectedColor = c;
                cb.disabledColor = new Color(0, 0, 0, 0);
                cb.colorMultiplier = 1;
                cb.fadeDuration = 0.1f;
                item.go.GetComponent<Button>().colors = cb;

            }
        }
    }
}
