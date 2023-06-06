using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ShineLight : MonoBehaviour
{
    public float maxintensity = 10f;
    public float speed = 1;
    public float outSpeed = 1;
    private void Start()
    {
        StartCoroutine(TextAppear(transform.GetComponent<Light2D>()));
        // Destroy(GameObject.Find("menu"));
    }
    // Start is called before the first frame update
    private IEnumerator TextAppear(Light2D t)
    {
        while (t.intensity<maxintensity)
        {
            t.intensity +=  speed* Time.deltaTime;
            yield return null;
        }
        while (t.intensity > 0)
        {
            t.intensity -= outSpeed*Time.deltaTime;
            yield return null;
        }
    }
}
