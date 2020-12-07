using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadPortraitBgImage : MonoBehaviour//头像框
{
    private MouseControlledMovement mouseControlledMovement;//玩家控制器

    private Text lvText;//等级

    private Image headPortraitImage;//头像

    private void Awake()//如果不在mouseControlledMovement初始化前先添加OnStart方法，就会错过mouseControlledMovement初始化时调用OnStart方法
    {
        mouseControlledMovement = GameObject.Find("PlayerObject").GetComponent<MouseControlledMovement>();

        lvText = transform.Find("LvText").GetComponent<Text>();
        headPortraitImage = transform.Find("HeadPortraitImage").GetComponent<Image>();

        mouseControlledMovement.initialize.AddListener(OnStart);//添加初始方法，在玩家控制器初始化后再初始化，不然空指针
    }

    public void OnStart()//玩家控制器初始化时调用
    {
        lvText.text = mouseControlledMovement.GetHeroAttribute.AttributeList.levelInt.ToString();

        mouseControlledMovement.upGradeLevel.AddListener(OnLevelAccording);//添加升级时执行的方法

        OnHeadPortraitUpdate();
    }

    public void OnLevelAccording()//等级提示，升级时调用
    {
        lvText.text = mouseControlledMovement.GetHeroAttribute.AttributeList.levelInt.ToString();
    }

    public void OnHeadPortraitUpdate()//英雄头像更新
    {
        string[] heroTypeList = Resources.Load<TextAsset>("hero").ToString().Split('\n');//获取英雄列表

        int idI = mouseControlledMovement.GetHeroAttribute.AttributeList.typeID;//获取英雄ID

        string heroStringL = heroTypeList[idI];//根据ID获得英雄类型

        string[] heroL = heroStringL.Split('|');

        headPortraitImage.sprite = Resources.Load<Sprite>(heroL[2]);//加载头像
    }

    private GameObject informationPanel;
    public void OnOpenHeroPortrait()
    {
        if (informationPanel != null) return;
        informationPanel = Instantiate(Resources.Load<GameObject>("preset/UI/global/AStatusBgImage"));
        informationPanel.transform.SetParent(transform.parent);
        informationPanel.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        informationPanel.transform.localScale = new Vector3(1, 1, 1);
        informationPanel.GetComponent<AStatusBgImage>().OnStart(ServerStoreRead.GetCurrentRoleAttributeString());
    }
}