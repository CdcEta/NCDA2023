using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenCoin : MonoBehaviour
{
    // Start is called before the first frame update

    public int value = 10;
    void Start()
    {
        int coinSpeed = Random.Range(-150, 150);
        GetComponent<Rigidbody2D>().velocity = new Vector3(coinSpeed, 0, 0);
        Invoke("CoinTag", 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CoinTag()
    {
        gameObject.tag = "GoldenCoin";
    }
}
