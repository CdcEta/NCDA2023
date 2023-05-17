using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponGridUI : MonoBehaviour
{
    public static Image weaponGrid;
    private void Start()
    {
        weaponGrid = GetComponent<Image>();
    }
}
