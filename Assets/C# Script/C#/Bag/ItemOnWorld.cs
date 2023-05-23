
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemOnWorld : UI
{
    public Rigidbody2D rigid;
    public Inventory playerInventory;
    public Item thisItem;
    public  Image WeaponGrid;
    public static bool isKill01;
    public static bool isPrice01;
    public bool isBought01;
    public bool iskilled01;
    public bool isNotFree;
    private Animator animThis;
    private Item temp;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //  player = GameObject.Find("NewPlayer");
        animThis = GetComponent<Animator>();
        animThis.SetInteger("ID", thisItem.id);
        if ((!isBought01 && !isKill01)||isNotFree)
        {
            tip.GetComponent<Text>().text = "  $"+thisItem.money.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isNotFree)
        {
            Bought();
        }
        else if (!isBought01&&!isKill01)
        {
            if (isPrice01)
            {
                PriceBought();
            }
            else
            Bought();
            PlayerController.playerController.moneyText.text = PlayerController.playerController.money.ToString();
        }
        else
        {
            tip.GetComponent<Text>().text = "°´FÊ¹ÓÃ";
            rigid.gravityScale = 100;
            Stay();
        }
    }


    private void Bought()
    {
        if (isStay)
        {
            if (Input.GetKeyDown(KeyCode.F) && isStay)
            {
                if (PlayerController.playerController.money > thisItem.money)
                {
                    PlayerController.playerController.money -= thisItem.money;
                    isBought01 = true;
                    isNotFree = false;
                }
            }
        }
    }

    private void PriceBought()
    {
        if (isStay)
        {
            if (Input.GetKeyDown(KeyCode.F) && isStay)
            {
                if (PlayerController.playerController.money > thisItem.money/2)
                {
                    PlayerController.playerController.money -= thisItem.money/2;
                    isBought01 = true;
                }
            }
        }
    }
    private void Stay()
    {
        if (isStay)
        {
            if (Input.GetKeyDown(KeyCode.F) && isStay)
            {
                if (!playerInventory.itemList.Contains(thisItem))
                {
                    WeaponGridUI.weaponGrid.sprite = thisItem.itemImage;
                    if (playerInventory.itemList[0])
                    {
                        PlayerController.playerController.id = thisItem.id;
                        PlayerController.playerController.comboStep = 0;
                        anim.SetInteger("ID", playerInventory.itemList[0].id);
                        transform.position = PlayerController.playerController.transform.position;
                        temp = playerInventory.itemList[0];
                        playerInventory.itemList[0] = thisItem;
                        thisItem = temp;
                    }
                    else
                    {
                        PlayerController.playerController.id = thisItem.id;
                        PlayerController.playerController.comboStep = 0;
                        playerInventory.itemList[0] = thisItem;
                        Destroy(gameObject);
                    }


                }

            }
        }
    }

        


}
