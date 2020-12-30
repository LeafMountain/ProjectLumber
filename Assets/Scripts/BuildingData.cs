using UnityEngine;

[CreateAssetMenu(menuName = "Lumber/Building Data")]
public class BuildingData : ScriptableObject
{
    public string name;
    public Sprite icon;
    public Building prefab;
}
