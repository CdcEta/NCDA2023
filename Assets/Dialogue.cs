using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public float duration = 5f;
    private void OnEnable()
    {
        foreach (var t in GetComponentsInChildren<Text>())
            t.color -= new Color(0, 0, 0, 1);
        StartCoroutine(TextAppear(transform.GetChild(0).GetComponent<Text>()));
        // Destroy(GameObject.Find("menu"));
    }
    // Start is called before the first frame update
    private IEnumerator TextAppear(Text t)
    {
        while (t.color.a < 1)
        {
            t.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(duration);
        while (t.color.a > 0)
        {
            t.color -= new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
