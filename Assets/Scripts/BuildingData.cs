using UnityEngine;

[CreateAssetMenu(menuName = "Lumber/Building Data")]
public class BuildingData : ScriptableObject
{
    new public string name;
    public Sprite icon;
    public Building prefab;
    public ItemData[] recipe;
}
