using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New DialogInventory", menuName = "DialogInventory/New DialogInventory")]
public class DialogInventory : ScriptableObject
{
    public List<DialogItem> itemList = new List<DialogItem>();
}