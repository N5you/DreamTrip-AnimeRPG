using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class On01SampleSceneCanvasButton : MonoBehaviour
{
    public int archiveNumber;//存档数量
    public RectTransform archiveList;//存档列表

    public Transform currentRoleShowA;//当前角色展示位

    private GameObject archiveButton;//存档按钮

    private GameObject ArchiveImage;//存档选择Image（页面）
    private GameObject CreateHeroBg;//创建角色Image

    private Text roleNameText;//角色名字
    private Text roleLevelText;//角色等级
    private Text roleRebirthText;//角色重生

    private GameObject previewGameObject;//预览角色
    private GameObject coursePreview;//当前预览

    private string[] heroTypeList;//英雄类型列表
    private void Start()
    {
        ArchiveImage = transform.Find("ArchiveImage").gameObject;
        CreateHeroBg = transform.Find("CreateHeroBg").gameObject;

        archiveButton = Resources.Load<GameObject>("preset/UI/ArchiveButton");

        roleLevelText = transform.Find("ArchiveImage/BottomImage/levelText").GetComponent<Text>();
        roleNameText = transform.Find("ArchiveImage/BottomImage/NameText").GetComponent<Text>();
        roleRebirthText = transform.Find("ArchiveImage/BottomImage/againText").GetComponent<Text>();

        previewGameObject = transform.Find("ArchiveImage/GameObject").gameObject;

        //读取角色存档列表，空存档的按钮为“创建角色”，仅显示角色名字【玩家账号 + 索引（Int）】
        for (int i = 0; i <= archiveNumber; i++)
        {
            GameObject go = Instantiate(archiveButton);//创建按钮
            go.transform.SetParent(archiveList, false);

            //Debug.Log(ServerStoreRead.GetHeroArchiveIfThereIs(3));
            int iiii = i;
            if (ServerStoreRead.GetHeroArchiveIfThereIs(iiii))//先判断是否有该索引存档存在
            {
                go.transform.Find("Text").GetComponent<Text>().text = ServerStoreRead.GetRoleName(iiii.ToString());//玩家账号+索引
                go.GetComponent<Button>().onClick.AddListener(delegate () { OnSelectRole(iiii); });//添加带参数方法
            }
            else
            {
                go.GetComponent<Button>().onClick.AddListener(delegate () { OnCreateArchive(iiii); });//给按钮添加创建方法
            }
        }

        heroTypeList = Resources.Load<TextAsset>("hero").ToString().Split('\n');

        string str = ServerStoreRead.GetCurrentRole();
        if (str.Length != 0)//读取是否有选当前存档 //str.Equals(string.Empty)
        {
            string[] stt = str.Split(',');//name,玩家,索引
            OnSelectRole(int.Parse(stt[2]));//
        }
    }

    public void OnCreateArchive(int index)//创建存档 //打开创建英雄Image
    {
        //挑转到角色创建页面.
        ArchiveImage.SetActive(false);
        CreateHeroBg.SetActive(true);
        ServerStoreRead.SetCurrentRole(index);//设置当前选择的角色存档
        ServerStoreRead.currentHeroIndex = index;//设置当前角色存档的索引
    }

    public void OnCreateHeroBackButton()//创建英雄Image的返回按钮
    {
        CreateHeroBg.SetActive(false);
        ArchiveImage.SetActive(true);
    }

    public void OnSelectRole(int key)//选择角色（存档）
    {
        HeroAttributeList heroAttributeListL = ServerStoreRead.GetSpecifiedRoleAttribute(key.ToString());//获取指定角色（索引）属性列表

        roleNameText.text = "昵称：" + ServerStoreRead.GetRoleName(key.ToString());//角色昵称设置
        roleLevelText.text = "等级：" + heroAttributeListL.levelInt.ToString();
        roleRebirthText.text = "重生：" + heroAttributeListL.rebirthValueInt.ToString();

        string str = heroTypeList[heroAttributeListL.typeID];
        string[] kv = str.Split('|');

        if (coursePreview != null) Destroy(coursePreview);

        //预设体|名字|图标|生命值|魔法值|攻击力|攻击距离|攻击速度|移动速度|护甲|力量|敏捷|智力|职业|伤害类型
        coursePreview = Instantiate(Resources.Load<GameObject>(kv[0]));//创建
        coursePreview.transform.SetParent(currentRoleShowA);

        //coursePreview.transform.SetParent(archiveList);//archiveList 设置父物体
        coursePreview.transform.localPosition = new Vector3(0, 1, 1);
        coursePreview.transform.Rotate(new Vector3(0, -175, 0));
        coursePreview.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);//设置缩放

        ServerStoreRead.currentHeroIndex = key;//设置当前角色存档的索引
        ServerStoreRead.SetCurrentRole(key);//设置当前选择的存档
    }

    public void OnStartGame()//开始游戏
    {
        string str = ServerStoreRead.GetCurrentRole();
        if (str.Length != 0)//读取是否有选当前存档 //str.Equals(string.Empty)
        {
            SceneManager.LoadScene("TownPage05");//进入(新手村)场景
            return;
        }
        roleNameText.text = "请先选择角色，或创建角色";
    }

    public void OnDeleteRoleArchive()//删除角色存档
    {
        roleNameText.text = "该功能不存在";
    }
}