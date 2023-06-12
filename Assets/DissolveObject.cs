using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveObject : MonoBehaviour
{
    private Material material;
    public GameObject[] openGameObject;
    public bool isDissolving = false;

    private float fade = 1f;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

        if (isDissolving)
        {
            fade -= Time.deltaTime;

            if (fade<=0f)
            {
                fade = 0f;
                isDissolving = false;
            }
            material.SetFloat("_Fade",fade);
            if (openGameObject != null)
            {
                foreach (var variGameObject in openGameObject)
                {
                    variGameObject.SetActive(true);
                }
            }
        }
    
    }
}
