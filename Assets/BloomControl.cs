using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BloomControl : MonoBehaviour
{
    public float speed = 0.004f;
    public Volume myVolume;
    public float duration = 5f;
    private Bloom bloom;
    public bool shine;
    public bool setDefualt;
    public bool isMouseControll;
    private void Start()
    {   
        SetDefault(1f,5f);
        myVolume = this.GetComponent<Volume>();
        myVolume.profile.TryGet<Bloom>(out bloom);
        
        // foreach (var t in GetComponentsInChildren<Text>())
        //     t.color -= new Color(0, 0, 0, 1);
        // StartCoroutine(TextAppear(transform.GetChild(0).GetComponent<Text>()));
        // Destroy(GameObject.Find("menu"));
    }

    private void SetDefault(float threshold,float intensity)
    {
        bloom.threshold.value = threshold;
        bloom.intensity.value = intensity;
        setDefualt = false;
    }
    public void Update()
    {
        if (isMouseControll)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(BloomAppear(bloom));
            }
        }
        if (shine)
        {
            StartCoroutine(BloomAppear(bloom));
        }

        if (setDefualt)
        {
            SetDefault(1f,5f);
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
