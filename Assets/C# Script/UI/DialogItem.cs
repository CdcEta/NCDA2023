using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New DialogItem", menuName = "DialogInventory/New DialogItem")]
public class DialogItem : ScriptableObject
{
    public int dialogLength;

    public string[] dialog;
    public int choiceLength;
    public string[] choice;

}
