using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BloomControl : MonoBehaviour
{
    public float speed;
    public Volume myVolume;
    public float duration = 5f;
    private Bloom bloom;
    public bool shine;
    private void Start()
    {
        myVolume = this.GetComponent<Volume>();
        myVolume.profile.TryGet<Bloom>(out bloom);
        // foreach (var t in GetComponentsInChildren<Text>())
        //     t.color -= new Color(0, 0, 0, 1);
        // StartCoroutine(TextAppear(transform.GetChild(0).GetComponent<Text>()));
        // Destroy(GameObject.Find("menu"));
    }

    public void Update()
    {
        if (shine)
        {
            StartCoroutine(BloomAppear(bloom));
        }
    }
    // // Start is called before the first frame update
    private IEnumerator BloomAppear(Bloom bloom)
    {
        while (bloom.threshold.value > 0)
        {
            bloom.threshold.value  -= speed*Time.deltaTime;
            yield return null;
        }
        while (bloom.intensity.value<200)
        {
            bloom.intensity.value += speed*50*Time.deltaTime;
            yield return null;
        }
    }
}
