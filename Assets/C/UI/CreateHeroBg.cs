using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateHeroBg : MonoBehaviour
{
    private Text NameText;//名字
    private Text ProfessionalText;//职业
    private Text PowerText;//力量
    private Text AgileText;//敏捷
    private Text BrainsText;//智力
    private Text ArmorText;//护甲
    private Text LifeText;//生命值
    private Text ManaText;//魔法值
    private Text DamageText;//攻击力
    private Text RangeText;//攻击距离

    private InputField NnameInputField;//名字输入框

    private int chooseIndex = 0;//选择角色索引
    private int chooseIndexMax; //选择角色索引最大值

    private Transform roleAccording;//预览角色类型的位置（父物体）
    private GameObject currentRole;//当前

    private string[] heroTypeList;//英雄类型列表
    
    private void Start()
    {
        NameText = transform.Find("NameText").GetComponent<Text>();
        ProfessionalText = transform.Find("ProfessionalText").GetComponent<Text>();
        PowerText = transform.Find("PowerText").GetComponent<Text>();
        AgileText = transform.Find("AgileText").GetComponent<Text>();
        BrainsText = transform.Find("BrainsText").GetComponent<Text>();
        ArmorText = transform.Find("ArmorText").GetComponent<Text>();
        LifeText = transform.Find("LifeText").GetComponent<Text>();
        ManaText = transform.Find("ManaText").GetComponent<Text>();
        DamageText = transform.Find("DamageText").GetComponent<Text>();
        RangeText = transform.Find("RangeText").GetComponent<Text>();

        NnameInputField = transform.Find("NnameInputField").GetComponent<InputField>();

        roleAccording = transform.Find("GameObject");

        heroTypeList = Resources.Load<TextAsset>("hero").ToString().Split('\n');//获取角色类型列表
        chooseIndexMax = heroTypeList.Length - 1;
        OnChooseHeroSwitch(0);
    }

    public void OnChooseHeroSwitch(int index) //选择角色页面的选择角色切换
    {
        chooseIndex = chooseIndex + index;
        if (chooseIndex > chooseIndexMax)//不能超过最大值
        {
            chooseIndex = 0;
        }
        if (chooseIndex < 0)//不能小于最小值
        {
            chooseIndex = chooseIndexMax;
        }

        string str = heroTypeList[chooseIndex];
        string[] kv = str.Split('|');

        if (currentRole != null) Destroy(currentRole);
        //预设体|名字|图标|生命值|魔法值|攻击力|攻击距离|攻击速度|移动速度|护甲|力量|敏捷|智力|职业|伤害类型
        currentRole = Instantiate(Resources.Load<GameObject>(kv[0]));
        currentRole.transform.SetParent(roleAccording);
        currentRole.transform.localPosition = new Vector3(0, 1, 1);
        currentRole.transform.Rotate(new Vector3(0, -175, 0));
        currentRole.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);//设置缩放

        NameText.text = kv[1];//名字
        ProfessionalText.text = "职业：" + HeroAttributeList.ProfessionalIDTurnString(int.Parse(kv[13]));//职业
        PowerText.text = "力量：" + kv[10];//力量
        AgileText.text = "敏捷：" + kv[11];//敏捷
        BrainsText.text = "智力：" + kv[12];//智力
        ArmorText.text = "护甲：" + kv[9];//护甲
        LifeText.text = "生命值：" + kv[3];//生命值
        ManaText.text = "魔法值：" + kv[4];//魔法值
        DamageText.text = "攻击力：" + kv[5];//攻击力

        float f = float.Parse(kv[6]) * 100;
        RangeText.text = "攻击距离：" + f + "码";//攻击距离
    }

    public void OnCreateStart()//创建角色并开始游戏 //选的是哪个角色（通过索引确定）
    {
        string key = NnameInputField.text;//获取输入内容
        if (NnameInputField.text.Length != 0)//禁止为空
        {
            key = key.Replace(",", ".");//字符串里面禁止有,逗号，如果有，用.点号代替

            ServerStoreRead.SetRoleName(key);//存储当前选择存档（索引）的角色（英雄）名字
            //ServerStoreRead.SetRoleName(NnameInputField.text);//存储当前选择存档（索引）的角色（英雄）名字

            string str = heroTypeList[chooseIndex];
            string[] kv = str.Split('|');

            HeroAttributeList heroAttributeL = new HeroAttributeList(); //生命值|魔法值|攻击力|攻击距离|攻击速度|移动速度|护甲|力量|敏捷|智力|职业|伤害类型|类型ID
            heroAttributeL.hpMaxInt = int.Parse(kv[3]);//生命值
            heroAttributeL.mpMaxInt = int.Parse(kv[4]);//魔法值
            heroAttributeL.damageValueInt = int.Parse(kv[5]);//攻击力
            heroAttributeL.damageRangeFloat = float.Parse(kv[6]);//攻击距离（最大攻击距离）
            heroAttributeL.damageSpeedFloat = float.Parse(kv[7]);//攻击速度
            heroAttributeL.MovementSpeedFloat = float.Parse(kv[8]);//移动速度
            heroAttributeL.armorInt = int.Parse(kv[9]);//护甲
            heroAttributeL.typeID = int.Parse(kv[10]);//力量
            heroAttributeL.agileInt = int.Parse(kv[11]);//敏捷
            heroAttributeL.intelligenceInt = int.Parse(kv[12]);//智力
            heroAttributeL.professionalString = (HeroProfessionalType)int.Parse(kv[13]);//职业
            heroAttributeL.damageClassType = (DamageClassType)int.Parse(kv[14]);//伤害类型
            heroAttributeL.typeID = chooseIndex;

            ServerStoreRead.SetCurrentRoleAttribute(heroAttributeL);//存储角色 HeroAttributeList

            ServerStoreRead.SetHeroArchiveUpStorage(ServerStoreRead.currentHeroIndex, chooseIndex.ToString());//设置存档（索引，角色ID）

            SceneManager.LoadScene("TownPage05");//进入(新手村)场景
        }

    }
}