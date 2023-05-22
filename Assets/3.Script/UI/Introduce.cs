using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Introduce : MonoBehaviour
{
    private int count;
    private int index;
    
    private void Awake()
    {
        count = transform.childCount;
        index = 0;
        foreach (var t in GetComponentsInChildren<Text>())
            t.color -= new Color(0, 0, 0, 1);
        StartCoroutine(TextAppear(transform.GetChild(index++).GetComponent<Text>()));
        // Destroy(GameObject.Find("menu"));
    }

    private IEnumerator TextAppear(Text t)
    {
        if(index==0)
            yield return new WaitForSeconds(1.5f);
        while (t.color.a < 1)
        {
            t.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.6f);
        if (index <= count - 1)
            yield return StartCoroutine(TextAppear(transform.GetChild(index++).GetComponent<Text>()));
        else
        {
            index = 0;
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(TextDisappear(transform.GetChild(index++).GetComponent<Text>()));
        }
    }
    private IEnumerator TextDisappear(Text t)
    {
        while (t.color.a > 0)
        {
            t.color -= new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.6f);
        if (index <= count - 1)
            yield return StartCoroutine(TextDisappear(transform.GetChild(index++).GetComponent<Text>()));
    }
    
}
