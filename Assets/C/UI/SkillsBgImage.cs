using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillsBgImage : MonoBehaviour
{
    public GameObject skillsObject; //技能物体

    private MouseControlledMovement mouseControlledMovement;//玩家控制器

    //private HeroSkills[] heroSkills = new HeroSkills[6];
    //private SkillsImage[] skillsImage = new SkillsImage[6];

    private void Awake()//如果不在mouseControlledMovement初始化前先添加OnStart方法，就会错过mouseControlledMovement初始化时调用OnStart方法
    {
        mouseControlledMovement = GameObject.Find("PlayerObject").GetComponent<MouseControlledMovement>();
        mouseControlledMovement.initialize.AddListener(OnStart);//添加初始方法，在玩家控制器初始化后再初始化，不然空指针
    }

    public void OnStart()//加载技能数据
    {
        int id = mouseControlledMovement.GetHeroAttribute.AttributeList.typeID;//根据英雄ID排列技能ID
        string[] skillsTypeList = Resources.Load<TextAsset>("skills").ToString().Split('\n');
        string[] skillsID = skillsTypeList[id].Split(',');

        HeroSkills[] heroSkills = new HeroSkills[6];
        SkillsImage[] skillsImage = new SkillsImage[6];

        for (int i = 0; i <= skillsID.Length - 1; i++)
        {
            GameObject go = Instantiate(skillsObject);//创建技能物体
            go.transform.SetParent(transform);

            // 给技能物体配置技能数据
            heroSkills[i] = HeroSkillsType.SkillsID(int.Parse(skillsID[i]));
            go.GetComponent<Image>().sprite = Resources.Load<Sprite>(heroSkills[i].ico);
            go.transform.Find("ShortcutsText").GetComponent<Text>().text = heroSkills[i].shortcuts;

            skillsImage[i] = go.GetComponent<SkillsImage>();//魔法消耗 魔法冷却
            skillsImage[i].keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), heroSkills[i].shortcuts);
            skillsImage[i].skillsCd = heroSkills[i].cooling;

            //skillsImage.skillsEvent.AddListener((UnityEngine.Events.UnityAction<GameObject, int>)heroSkills.skills);
            //skillsImage[i].skillsEvent.AddListener(delegate (int nn) { OnSkills(i); });
            //Delegate del = Delegate.CreateDelegate(typeof(EventHandler), heroSkills[0].skills, heroSkills[0].methodlnfo);//转MethodInfo为方法

            skillsImage[i].heroSkills = heroSkills[i];
            skillsImage[i].mouseControlledMovement = mouseControlledMovement;
        }

        //skillsImage[0].skillsEvent.AddListener(delegate (int nn) { OnSkills(0); });
        //skillsImage[1].skillsEvent.AddListener(delegate (int nn) { OnSkills(1); });
        //skillsImage[2].skillsEvent.AddListener(delegate (int nn) { OnSkills(2); });
        //skillsImage[3].skillsEvent.AddListener(delegate (int nn) { OnSkills(3); });
        //skillsImage[4].skillsEvent.AddListener(delegate (int nn) { OnSkills(4); });
        //skillsImage[5].skillsEvent.AddListener(delegate (int nn) { OnSkills(5); });
        Destroy(this);
    }

    //public void OnSkills(int index)
    //{
    //    object[] obj = new object[2];
    //    obj[0] = mouseControlledMovement.GetHeroGameObject;
    //    obj[1] = mouseControlledMovement.GetHeroAttribute;
    //    object ni = heroSkills[index].methodlnfo.Invoke(heroSkills[index].skills, obj);
    //    if (!(bool)ni) skillsImage[index].ResetCooling();
    //}
}