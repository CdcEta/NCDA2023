using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
