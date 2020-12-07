using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DefensivePattern05Base : MonoBehaviour
{
    public GameObject failureToBan;

    private GameObject heroGameObject;
    private HeroAttribute heroAttribute;
    
    private void Start()
    {
        heroGameObject = gameObject;
        heroAttribute = heroGameObject.GetComponent<HeroAttribute>();//获取HeroAttribute
        heroAttribute.AIStart(heroGameObject.transform.Find("HpSlotCanvas"));//让HeroAttribute初始化

        heroAttribute.deathEvent.AddListener(OnDeath);//添加死亡事件

        heroGameObject.transform.Find("HpSlotCanvas/NameText").GetComponent<Text>().text = "我是基地保护我";
    }

    public void OnDeath(GameObject gameObjectL)//调用即死亡 //死亡就游戏失败
    {
        //Destroy(GameObject.Find("Canvas"));
        //Destroy(GameObject.Find("PlayerObject"));
        failureToBan.SetActive(false);

        GameObject.Find("PlayerObject").GetComponent<MouseControlledMovement>().enabled = false;

        GameObject failureEndCanvas = Instantiate(Resources.Load<GameObject>("preset/UI/global/FailureEndCanvas"));
        failureEndCanvas.SetActive(true);
        failureEndCanvas.transform.Find("FailureEndBgImage/ResurrectionButton/Text").GetComponent<Text>().text = "重新挑战";
    }
}