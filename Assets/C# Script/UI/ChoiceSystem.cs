using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChoiceSystem : DialogSystem
{
    public Text text1, text2, text3;
    public GameObject weapon;
    public GameObject owner;
    public Animator anim;
    public Animator anim2;
    public Animator anim3;
    public Animator anim4;
    public Animator anim5;
    public Animator anim6;
    public Animator anim7;
    public Animator anim8;
    public GameObject dialog;
    public GameObject canvas;
    public GameObject dead;
    public GameObject deadman;
    public GameObject result;
    public GameObject mon;
    public GameObject beg;
    public GameObject rob;
    public GameObject sol;
    public GameObject sol1;
    public GameObject chi;
    public GameObject sal;
    public GameObject ste;
    public GameObject nob;
    public GameObject Coi;
    public GameObject coi;
    public GameObject coi2;
    public Text tip;

    private void Awake()
    {

    }
    protected override void OnEnable()
    {
        GetZJZNiTianDeGameObject();
        Debug.Log(npcID);
            text1.text = dialogInventory.itemList[npcID - 1].choice[0];
            text2.text = dialogInventory.itemList[npcID - 1].choice[1];
            text3.text = dialogInventory.itemList[npcID - 1].choice[2];

    }


    private void GetZJZNiTianDeGameObject()
    {
        weapon = GameObject.Find("WeaponPackage");
        if(weapon!=null)
        weapon = weapon.transform.Find("Weapon").gameObject;
        owner = GameObject.Find("owner");
        if(GameObject.Find("ShopOwner"))
        anim = GameObject.Find("ShopOwner").GetComponent<Animator>();
        if (GameObject.Find("monster"))
            anim2 = GameObject.Find("monster").GetComponent<Animator>();
        if (GameObject.Find("Beggar"))
            anim3 = GameObject.Find("Beggar").GetComponent<Animator>();
        if (GameObject.Find("Robber"))
            anim4 = GameObject.Find("Robber").GetComponent<Animator>();
        if (GameObject.Find("solider1"))
            anim5 = GameObject.Find("solider1").GetComponent<Animator>();
        if (GameObject.Find("Sale"))
            anim6 = GameObject.Find("Sale").GetComponent<Animator>();
        if (GameObject.Find("Steal"))
            anim7 = GameObject.Find("Steal").GetComponent<Animator>();
        if (GameObject.Find("Noble"))
            anim8 = GameObject.Find("Noble").GetComponent<Animator>();
        canvas = GameObject.Find("DiaryCanvas"); 
        dead = GameObject.Find("Dead");
        deadman = GameObject.Find("DeadMan");
        result = GameObject.Find("Result");
        if (result != null)
            result = result.transform.Find("result").gameObject;
        mon = GameObject.Find("mon");
        beg = GameObject.Find("Beg");
        rob = GameObject.Find("Rob");
        sol = GameObject.Find("solider");
        sol1 = GameObject.Find("solider1");
        chi = GameObject.Find("Children");
        sal = GameObject.Find("sale");
        ste = GameObject.Find("steal");
        nob = GameObject.Find("noble");                
        Coi = GameObject.Find("Coin");
        coi = GameObject.Find("coin");
        coi2 = GameObject.Find("Coin2");
        if (coi2 != null)
            coi2 = coi2.transform.Find("coin2").gameObject;
        if (GameObject.Find("SoldierTip"))
            tip = GameObject.Find("SoldierTip").GetComponent<Text>();

    //weapon = weapon
}
    public void ChoiceSwitch01()
    {
        switch (npcID)
        {
            case 1:
                weapon.SetActive(true);
                owner.SetActive(false);
                playerController.isTalking = false;
                choice.SetActive(false);
                dialog.GetComponent<DialogSystem>().isStop01 = false;
                playerController.Kind(1, 1);
                break;
            case 3:
                playerController.isTalking = false;
                dead.SetActive(false);
                choice.SetActive(false);
                break;
            case 4:
                playerController.isTalking = false;
                anim2.SetTrigger("isHelping");
                mon.SetActive(false);
                choice.SetActive(false);
                for (int i = 1; i <= 2; i++)
                {
                    goldenCoin.GetComponent<GoldenCoin>().value = 100;
                    Instantiate(goldenCoin, playerController.transform.position, playerController.transform.rotation);

                }
                playerController.Kind(3, 3);
                break;
            case 5:
                playerController.isTalking = false;
                beg.SetActive(false);
                playerController.money -= 200;
                playerController.moneyPoint.GetComponent<Text>().text = "-200";
                playerController.moneyText.text = playerController.money.ToString();
                choice.SetActive(false);
                playerController.Kind(2, 4);
                break;

            case 6:
                playerController.isTalking = false;
                rob.SetActive(false);
                playerController.money -= 50;
                playerController.moneyPoint.GetComponent<Text>().text = "-50";
                playerController.moneyText.text = playerController.money.ToString();
                anim4.SetTrigger("isMercy");                                                     
                choice.SetActive(false);
                playerController.Kind(1, 5);
                break;
            case 7:
                sol.GetComponent<BoxCollider2D>().enabled = false;
                playerController.isTalking = false;
                choice.SetActive(false);
                break;
            case 8:
                anim6.SetTrigger("isgood");
                sal.SetActive(false);
                playerController.isTalking = false;
                for (int i = 1; i <= 10; i++)
                {
                    goldenCoin.GetComponent<GoldenCoin>().value = 20;
                    Instantiate(goldenCoin, playerController.transform.position, playerController.transform.rotation);

                }
                choice.SetActive(false);
                playerController.Kind(1, 7);
                break;
            case 9:
                ste.SetActive(false);
                playerController.isTalking = false;
                for (int i = 1; i <= 10; i++)
                {
                    goldenCoin.GetComponent<GoldenCoin>().value = 20;
                    Instantiate(goldenCoin, playerController.transform.position, playerController.transform.rotation);

                }
                choice.SetActive(false);
                playerController.Evil(1, 8);
                break;
            case 10:
                nob.SetActive(false);
                playerController.isTalking = false;
                choice.SetActive(false);
                playerController.Kind(1, 9);
                break;
            case 11:
                coi.SetActive(false);
                playerController.isTalking = false;
                for (int i = 1; i <= 10; i++)
                {
                    goldenCoin.GetComponent<GoldenCoin>().value = 20;
                    Instantiate(goldenCoin, playerController.transform.position, playerController.transform.rotation);

                }
                Coi.SetActive(false);
                coi2.SetActive(true);
                choice.SetActive(false);
                playerController.Evil(2, 10);
                break;
        }
    }
    public void ChoiceSwitch02()
    {
        switch (npcID)
        {
            case 1:
                weapon.SetActive(true);
                owner.SetActive(false);
                playerController.isTalking = false;
                ItemOnWorld.isPrice01 = true;
               
                dialog.SetActive(true);
                dialog.GetComponent<DialogSystem>().isStop01 = false;
                gameObject.SetActive(false);
                break;
            case 3:
                playerController.isTalking = false;
                deadman.SetActive(false);
                choice.SetActive(false);
                playerController.Kind(1, 2);
                break;

            case 4:
                playerController.isTalking = false;
                anim2.SetTrigger("isIgnoring");
                mon.SetActive(false);
                choice.SetActive(false);

                break;
            case 5:
                playerController.isTalking = false;
                beg.SetActive(false);
                playerController.moneyText.text = playerController.money.ToString();
                choice.SetActive(false);
                break;
            case 6:
                playerController.isTalking = false;
                rob.SetActive(false);
                choice.SetActive(false);
              
                break;
            case 7:
                sol.GetComponent<BoxCollider2D>().enabled = false;
                playerController.isTalking = false;
                sol1.SetActive(false);
                tip.text = "你赶走了士兵，父子很感谢你";
                choice.SetActive(false);
                playerController.Kind(2, 7);
                break;
            case 8:
                if (playerController.money >= 100)
                {
                    playerController.money -= 100;
                    playerController.hp += playerController.maxHP[0] / 2;
                }
                choice.SetActive(false);
                break;
            case 9:
                ste.SetActive(false);
                playerController.isTalking = false;
                anim7.SetTrigger("isdead");
                choice.SetActive(false);
                playerController.Kind(2, 8);
                break;
            case 10:
                nob.SetActive(false);
                playerController.isTalking = false;
                for (int i = 1; i <= 10; i++)
                {
                    goldenCoin.GetComponent<GoldenCoin>().value = 50;
                    Instantiate(goldenCoin, playerController.transform.position, playerController.transform.rotation);

                }
                choice.SetActive(false);
                break;
            case 11:
                coi.SetActive(false);
                playerController.isTalking = false;
                choice.SetActive(false);
                playerController.Kind(2, 10);
                break;
        }
    }
    public void ChoiceSwitch03()
    {
        switch (npcID)
        {
            case 1:
                weapon.SetActive(true);
                owner.SetActive(false);
                playerController.isTalking = false;
                ItemOnWorld.isKill01 = true;
                anim.SetTrigger("isdead");
                choice.SetActive(false);
                dialog.GetComponent<DialogSystem>().isStop01 = false;
                playerController.Evil(2,1);
                break;
            case 3:
                playerController.isTalking = false;
                dead.SetActive(false);
                result.SetActive(true);
                playerController.hp -= 20;
                choice.SetActive(false);
                playerController.Evil(1, 2);
                break;
            case 4:
                playerController.isTalking = false;
                anim2.SetTrigger("isHelping");
                mon.SetActive(false);
                choice.SetActive(false);
                for (int i = 1; i <= 5; i++)
                {
                    Instantiate(goldenCoin, playerController.transform.position, playerController.transform.rotation);
                    goldenCoin.GetComponent<GoldenCoin>().value = 100;
                }
                playerController.Evil(1, 3);
                break;
            case 5:
                playerController.isTalking = false;
                beg.SetActive(false);
                playerController.moneyText.text = playerController.money.ToString();
                choice.SetActive(false);
                anim3.SetTrigger("isKilling");
                for (int i = 1; i <= 5; i++)
                {
                    Instantiate(goldenCoin, playerController.transform.position, playerController.transform.rotation);
                    goldenCoin.GetComponent<GoldenCoin>().value = 10;
                }
                playerController.Evil(3, 4);
                break;
            case 6:
                playerController.isTalking = false;
                rob.SetActive(false);
                for (int i = 1; i <= 10; i++)
                {
                    Instantiate(goldenCoin, playerController.transform.position, playerController.transform.rotation);
                    goldenCoin.GetComponent<GoldenCoin>().value = 10;
                }
                anim4.SetTrigger("isDefenced");
                choice.SetActive(false);
                playerController.Evil(1, 5);
                break;
            case 7:
                sol.GetComponent<BoxCollider2D>().enabled = false;
                playerController.isTalking = false;
                anim5.SetTrigger("isKilled");
                tip.text = "你杀死了士兵，将孩子卖做苦力赚取了200金币";
                for (int i = 1; i <= 10; i++)
                {
                    Instantiate(goldenCoin, playerController.transform.position, playerController.transform.rotation);
                    goldenCoin.GetComponent<GoldenCoin>().value = 20;
                }
                chi.SetActive(false);
                choice.SetActive(false);
                break;
            case 8:
                anim6.SetTrigger("isclear");
                sal.SetActive(false);
                playerController.isTalking = false;
                for (int i = 1; i <= 10; i++)
                {
                    Instantiate(goldenCoin, playerController.transform.position, playerController.transform.rotation);
                    goldenCoin.GetComponent<GoldenCoin>().value = 20;
                }
                choice.SetActive(false);
                playerController.Evil(3, 4);
                break;
            case 9:
                ste.SetActive(false);
                playerController.isTalking = false;
                choice.SetActive(false);
                break;
            case 10:
                nob.SetActive(false);
                anim8.SetTrigger("isdead");
                playerController.isTalking = false;
                for (int i = 1; i <= 10; i++)
                {
                    goldenCoin.GetComponent<GoldenCoin>().value = 50;
                    Instantiate(goldenCoin, playerController.transform.position, playerController.transform.rotation);
                }
                choice.SetActive(false);
                playerController.Evil(1, 9);
                break;
        }
    }
    public void Quit()
    {
        canvas.SetActive(false);
    }
}
