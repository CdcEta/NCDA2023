using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")] 
public class Item : ScriptableObject
{
    public string itemName;
    public int money;
    public Sprite itemImage;
    public GameObject itemOnWorld;
    public int id;
    public bool isBow;
    public Sprite ArrowImage;
    [TextArea]
    public string itemInfo;
    
    
    
    public int level;
    public int WeaponAttackPower;


}
