using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LearningSkillsBgImage : MonoBehaviour
{
    public MouseControlledMovement mouseControlledMovement;//玩家控制器

    private Dropdown dropdown;//选择框
    private Image icoImage;//图标
    private Text typeText;//类型
    private Text lvText;//等级
    private Text shortcutsText;//快捷键
    private Text cDText;//冷却
    private Text consumptionText;//MP消耗
    private Text introduceText;//介绍
    private Text costText;//花费
    private Text titleText;//标题
    //private int index = 0;//索引
    private HeroSkills[] heroSkills = new HeroSkills[6];
    private void Start()
    {
        //MouseControlledMovement mouseControlledMovement = GameObject.Find("PlayerObject").GetComponent<MouseControlledMovement>();
        dropdown = transform.Find("Dropdown").GetComponent<Dropdown>();
        icoImage = transform.Find("IcoImage").GetComponent<Image>();
        typeText = transform.Find("TypeText").GetComponent<Text>();
        lvText = transform.Find("LvText").GetComponent<Text>();
        shortcutsText = transform.Find("ShortcutsText").GetComponent<Text>();
        cDText = transform.Find("CDText").GetComponent<Text>();
        consumptionText = transform.Find("ConsumptionText").GetComponent<Text>();
        introduceText = transform.Find("IntroduceText").GetComponent<Text>();
        costText = transform.Find("CostText").GetComponent<Text>();
        titleText = transform.Find("TitleText").GetComponent<Text>();

        int id = mouseControlledMovement.GetHeroAttribute.AttributeList.typeID;//根据英雄ID排列技能ID
        string[] skillsTypeList = Resources.Load<TextAsset>("skills").ToString().Split('\n');
        string[] skillsID = skillsTypeList[id].Split(',');

        for (int i = 0; i <= skillsID.Length - 1; i++)
        {
            heroSkills[i] = HeroSkillsType.SkillsID(int.Parse(skillsID[i]));//获取技能数据
            dropdown.options.Add(new Dropdown.OptionData(heroSkills[i].name, Resources.Load<Sprite>(heroSkills[i].ico)));//创建按钮
        }
        //dropdown.onValueChanged.AddListener(OnUpDropdown);
        OnUpDropdown();
    }

    public void OnUpDropdown()//根据索引切换 (更新信息)
    {
        int index = dropdown.value;//获得当前选择按钮的索引（位置）
        icoImage.sprite = Resources.Load<Sprite>(heroSkills[index].ico);//图标
        //typeText = transform.Find("TypeText").GetComponent<Text>();//技能类型
        //lvText.text = heroSkills[index].//技能等级
        shortcutsText.text = "快捷键：" + heroSkills[index].shortcuts;//快捷键
        cDText.text = "CD：" + heroSkills[index].cooling;
        consumptionText.text = "MP：" + heroSkills[index].consume;
        introduceText.text = heroSkills[index].describe;
        //costText = transform.Find("CostText").GetComponent<Text>();//消耗
        //titleText = transform.Find("TitleText").GetComponent<Text>();标题
    }

    public void OnShuTDown()//关闭按钮
    {
        gameObject.SetActive(false);
    }

    //技能升级按钮
}