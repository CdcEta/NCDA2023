using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Introduce : MonoBehaviour
{
    private int count;
    private int index;
    public float speed=1f;
    public float duration = 0.5f;
    
    private void OnEnable()
    {
        count = transform.childCount-4;

        index = 0;
        foreach (var t in GetComponentsInChildren<Text>())
            t.color -= new Color(0, 0, 0, 1);
        foreach (var t in GetComponentsInChildren<Image>())
            t.color -= new Color(0, 0, 0, 1);
        foreach (var t in GetComponentsInChildren<Image>())
            StartCoroutine(ImageAppear(t));
        StartCoroutine(TextAppear(transform.GetChild(index).GetComponent<Text>()));
        // Destroy(GameObject.Find("menu"));
    }
    private IEnumerator ImageAppear(Image t)
    {
        while (t.color.a < 1)
        {
            t.color += new Color(0, 0, 0, speed * Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator ImageDisappear(Image t)
    {
        yield return new WaitForSeconds(duration);
        while (t.color.a > 0)
        {
            t.color -= new Color(0, 0, 0, speed * Time.deltaTime);
            yield return null;
        }
        gameObject.SetActive(false);
        yield return null;
    }
    private IEnumerator TextAppear(Text t)
    { 
        while (t.color.a < 1)
        {
            t.color += new Color(0, 0, 0, speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(duration);
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
        if (index == count)
        {
            foreach (var i in GetComponentsInChildren<Image>())
                StartCoroutine(ImageDisappear(i));
        }
        while (t.color.a > 0)
        {
            t.color -= new Color(0, 0, 0, speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(duration);
        if (index <= count - 1)
            yield return StartCoroutine(TextDisappear(transform.GetChild(index++).GetComponent<Text>()));
    }
    
}
