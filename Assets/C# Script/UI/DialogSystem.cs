using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public GameObject goldenCoin;
    public static int npcID;
    [Header("UI组件")]
    public Text textLabel;
    public Image faceImage;

    [Header("文本文件")]
    public DialogInventory dialogInventory;
    public TextAsset textFile;
    public int Index;
    public float textSpeed;
    public PlayerController playerController;

    bool textFinished;//打字是否完成
    bool cancelTyping;//取消打字
    [Header("选项")]
    public GameObject choice;
    public GameObject Talk;
    public GameObject Book;
    public GameObject Book2;
    public GameObject Book3;
    public bool isStop01;
    public GameObject boss;
    public static GameObject boss01;

    List<string> textList = new List<string>();

    // Start is called before the first frame update
    void Awake()
    {
        
    }
    protected virtual void OnEnable()
    {
         Transform bossTemp = transform.Find("BossAll");
        boss = GameObject.FindGameObjectWithTag("BossChat");
      //  boss01 = GameObject.FindGameObjectWithTag("Boss01");
      if(boss01!=null)
        boss01.SetActive(false);
        Debug.Log("NPC"+npcID);
        GetTextFormFile();
        // textLabel.text = textList[Index];
        // Index++;
        textFinished = true;
        StartCoroutine(SetTextUI());
    }
    // Update is called once per frame
    private void Update()
    {
        if (gameObject != false)
        {
            if (Input.GetKeyDown(KeyCode.E) && Index == dialogInventory.itemList[npcID - 1].dialogLength)
            {
                gameObject.SetActive(false);
                //   if (!isStop01 && npcID == 1)
                //  {
                //      Index = 0;
                //   else
                //      Index++;
                if (npcID == 1)
                {

                }
                else if (npcID == 2)
                {
                    //  for (int i = 1; i <= 10; i++)
                    //{
                    //    goldenCoin.GetComponent<GoldenCoin>().value = 100;
                    //     Instantiate(goldenCoin, playerController.transform.position, playerController.transform.rotation);
                    //       
                    //   }
                    GiveCoin(10, 100);
                    Book.SetActive(true);

                    playerController.isTalking = false;
                    gameObject.SetActive(false);
                }
                else if (npcID == 14)
                {
                    Book2.SetActive(true);
                    playerController.isTalking = false;
                    gameObject.SetActive(false);
                
                }
                else if (npcID == 15)
                {
                    Book3.SetActive(true);
                    playerController.isTalking = false;
                    gameObject.SetActive(false);

                }
                else if(npcID == 12)
                {
                      boss.SetActive(false);
                      boss01.SetActive(true);
                      gameObject.SetActive(false);
                }
                else if (npcID == 13)
                {
                    playerController.ChooseEnd();
                    boss01.SetActive(true);
                    gameObject.SetActive(false);
                }
                else
                {
                    choice.SetActive(true);
                    playerController.isTalking = false;
                }
            }
           if (Input.GetKeyDown(KeyCode.E)&&textFinished)
            {
                
                StartCoroutine(SetTextUI());
            }
            if (Input.GetKeyDown(KeyCode.E) && (!isStop01 || npcID == 1))
            {
                if (textFinished && !cancelTyping)
                {
                    StartCoroutine(SetTextUI());
                }
                else if (!textFinished && !cancelTyping)
                {
                    cancelTyping = true;
                }
            }
        }

    }


    private void GiveCoin(int num,int value)
    {
        int coinSpeed;
        for (int i = 1; i <= num; i++)
        {
            
            goldenCoin.GetComponent<GoldenCoin>().value = 100;

            Instantiate(goldenCoin, playerController.transform.position, playerController.transform.rotation);
        }
    }
    void GetTextFormFile(/*TextAsset file*/)
    {
      
        if (!(isStop01 && npcID == 1))
        {
            textList.Clear();
            Index = 0;
        }
        for (int i = 0; i < dialogInventory.itemList[npcID - 1].dialogLength; i++)
        {
            Debug.Log(npcID + "aaaaa");
            textList.Add(dialogInventory.itemList[npcID - 1].dialog[i]);
        }
    }
    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";
        switch (textList[Index].Trim())

        {

            case "A":

                faceImage.enabled = true;

                Index++;

                break;

            case "B":


                faceImage.enabled = false;

                Index++;

                break;
            case "C":

                choice.SetActive(true);
                isStop01 = true;

                faceImage.enabled = false;
                gameObject.SetActive(false);

                break;

        }

        for (int i = 0; i < textList[Index].Length; i++)
        {
            textLabel.text += textList[Index][i];

            yield return new WaitForSeconds(textSpeed);
        }
        int letter = 0;
        while (!cancelTyping && letter < textList[Index].Length - 1)
        {
            textLabel.text += textList[Index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[Index];
        cancelTyping = false;
        textFinished = true;
        Index++;
    }

}
