using UnityEngine;
using UnityEngine.UI;

public class ResourceBubble : MonoBehaviour, IUIHoverInfo
{
    public ItemData item;
    public Image icon;
    public Image background;

    public string GetTooltip()
    {
        return item.name;
    }

    public void Setup(ItemData item)
    {
        this.item = item;
        icon.sprite = item.icon;
    }
}
