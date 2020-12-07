using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AStatusBgImage : MonoBehaviour
{
    private Image icoImage;//头像
    private Image expImage;//

    private Text expText;//
    private Text lvText;//等级
    private Text idText;//名字
    private Text hpText;//生命值
    private Text mpText;//魔法值
    private Text critText;//暴击率
    private Text throughText;//穿透力
    private Text luckyText;//幸运值
    private Text defenseText;//防御值
    private Text rateText;//攻击速度
    private Text speedText; //移动速度
    private Text rebirthText;//重生
    private Text damageText;//暴击效果
    private Text effectText;//虚拟伤害
    private Text powerText;//力量
    private Text agileText;//敏捷
    private Text brainsText;//智力

    private Text professionalText;//职业
    private Text awakeningText;//觉醒

    private GameObject renameBgImage;//改名面板
    public void OnStart(string heroAttributeListL)
    {
        icoImage = transform.Find("IcoImage").GetComponent<Image>();
        expImage = transform.Find("ExeText/ExpImage").GetComponent<Image>();
        expText = transform.Find("ExeText/Text").GetComponent<Text>();
        lvText = transform.Find("LvText").GetComponent<Text>();

        idText = transform.Find("IDText").GetComponent<Text>();
        hpText = transform.Find("HPText").GetComponent<Text>();
        mpText = transform.Find("MPText").GetComponent<Text>();
        critText = transform.Find("CritText").GetComponent<Text>();
        throughText = transform.Find("ThroughText").GetComponent<Text>();
        luckyText = transform.Find("LuckyText").GetComponent<Text>();
        defenseText = transform.Find("DefenseText").GetComponent<Text>();
        rateText = transform.Find("RateText").GetComponent<Text>();
        speedText = transform.Find("SpeedText").GetComponent<Text>();
        rebirthText = transform.Find("RebirthText").GetComponent<Text>();
        damageText = transform.Find("DamageText").GetComponent<Text>();
        effectText = transform.Find("EffectText").GetComponent<Text>();
        powerText = transform.Find("PowerText").GetComponent<Text>();
        agileText = transform.Find("AgileText").GetComponent<Text>();
        brainsText = transform.Find("BrainsText").GetComponent<Text>();
        professionalText = transform.Find("ProfessionalText").GetComponent<Text>();
        awakeningText = transform.Find("AwakeningText").GetComponent<Text>();

        renameBgImage = transform.Find("RenameBgImage").gameObject;

        OnLoadingAttributeAndUpdate(heroAttributeListL);
    }
    private void OnLoadingAttributeAndUpdate(string heroAttributeListL)
    {
        //0最大生命值|1最大魔法值|2攻击力|3攻击距离|4攻击速度|5移动速度|6护甲|7虚拟伤害|8穿透力|9重生数|10暴击率|11暴击效果|12幸运值|13力量|14敏捷|15智力|16职业|17觉醒|18等级|19经验值|20伤害类型|21类型ID
        string[] str = heroAttributeListL.Split(',');//分割

        string[] textAsset = Resources.Load<TextAsset>("hero").ToString().Split('\n');
        string icoStringL = textAsset[int.Parse(str[21])].Split('|')[2];
        icoImage.sprite = Resources.Load<Sprite>(icoStringL);//头像

        float expFloatL = float.Parse(str[19]) / (100f + int.Parse(str[18]));
        expImage.fillAmount = expFloatL;//经验值
        expText.text = ((int)expFloatL * 100) + "%";

        lvText.text = str[18];//等级
        idText.text = ServerStoreRead.GetRoleName();//名字
        hpText.text = "最大生命值：" + str[0];//生命值
        mpText.text = "最大魔法值：" + str[1];//魔法值
        critText.text = "暴击率：" + str[10] + "%";//暴击率
        throughText.text = "穿透力：" + str[8] + "%";//穿透力
        luckyText.text = "幸运值：" + str[12];//幸运值
        defenseText.text = "防御值：" + str[6];//防御值
        rateText.text = "攻速值：" + str[4];//攻击速度
        speedText.text = "移速值：" + str[5]; //移动速度
        rebirthText.text = "重生数：" + str[9];//重生
        damageText.text = "暴击效果：" + str[11] + "00%";//暴击效果
        effectText.text = "虚拟伤害：" + str[7];//虚拟伤害
        powerText.text = "力量：" + str[13];//力量
        agileText.text = "敏捷：" + str[14];//敏捷
        brainsText.text = "智力：" + str[15];//智力
        professionalText.text = "职业：" + str[16];//职业
        awakeningText.text = "觉醒：" + str[17];//觉醒
    }
    public void OnShutDown()//关闭按钮
    {
        Destroy(gameObject);
    }

    public void OnRename()//打开改名面板
    {
        renameBgImage.SetActive(true);
    }

    public void OnDetermineRename()//确定改名
    {
        string str = renameBgImage.transform.Find("InputField").GetComponent<InputField>().text;
        ServerStoreRead.SetRoleName(str);
        idText.text = str;
        OnRenameShutDown();
    }

    public void OnRenameShutDown()//改名面板关闭
    {
        renameBgImage.SetActive(false);
    }
}