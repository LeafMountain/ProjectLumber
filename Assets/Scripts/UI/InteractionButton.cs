using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour, IUIHoverInfo
{
    public TMP_Text text;
    public Image icon;
    public Button button;
    public string info;

    public string GetTooltip()
    {
        return info;
    }
}
