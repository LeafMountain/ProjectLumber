using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Lumber/ItemData")]
public class ItemData : ScriptableObject
{
    new public string name;
    public GameObject prefab;
    public Sprite icon;
}
