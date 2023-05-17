using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowController : MonoBehaviour
{
    public Transform player;
    public float x;
    public float y;
    public float x1;
    public float y1;
    public float moveSpeed;
        public float moveSpeed2;
    private Vector3 followPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Follow02();
        }
        else
            Follow01();
    }

    void Follow01()
    {
        followPosition = new Vector3(player.position.x + player.localScale.x * x, player.position.y + y, 0);
        this.transform.position = Vector3.MoveTowards(this.transform.position, followPosition, moveSpeed * Time.deltaTime);
    }

    void Follow02()
    {
        followPosition = new Vector3(player.position.x + player.localScale.x * x1, player.position.y + y1, 0);
        this.transform.position = Vector3.MoveTowards(this.transform.position, followPosition, moveSpeed2 * Time.deltaTime);
    }
}
