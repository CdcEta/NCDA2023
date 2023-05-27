using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Player Property", menuName = "Property/Player Property")] 
public class PlayerProperty : MonoBehaviour
{
    public float hp;
    public float maxHP ;
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
