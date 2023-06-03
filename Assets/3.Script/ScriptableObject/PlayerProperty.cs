using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
[CreateAssetMenu(fileName = "Player Property", menuName = "Property/Player Property")] 
public class PlayerProperty : ScriptableObject
{
    public int hp;
    public int maxHP ;
    public bool getAxe;
    public bool getSword;
    public float maxEnergy;
    public float energy;
    public float energyCure;
    [TextArea]
    public string sceneName;

    public string attackPower;
}
