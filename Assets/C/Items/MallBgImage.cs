using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MallBgImage : MonoBehaviour//商城面板
{
    public OptionsImage optionsImage;//打开商城按钮
    public GameObject message;//物品信息提示面板

    private int currentPage = 0;//当前页面

    private RectTransform listBgImage;

    private CurrencyType currencyType;//使用货币
    public CurrencyType CurrencyType
    {
        get { return currencyType; }
    }
    private void Start()
    {
        listBgImage = transform.Find("ListBgImage").GetComponent<RectTransform>();
        UpdateGoodsPage();

        transform.Find("ShutDownButton").GetComponent<Button>().onClick.AddListener(OnShutDownPPPPPPPPPPPPPPP);
    }

    public void OnSwitchMall(int index)//商店切换（金币/钻石/点卷）
    {
        if (index == currentPage) return;
        currentPage = index;
        currencyType = (CurrencyType)System.Enum.ToObject(typeof(CurrencyType), index);
        UpdateGoodsPage();
    }

    private void UpdateGoodsPage()//更新商品页面
    {
        string key = ServerStoreRead.GetStoreGoods(currentPage);//获取商店信息（从服务器）//每个空格代表一个物品，用X分割（物品ID x 价格 x 数量）
        
        MallSpace[] father = listBgImage.GetComponentsInChildren<MallSpace>();//删除子物体
        foreach (MallSpace child in father)
        {
            Destroy(child.gameObject);
        }

        if (key.Length > 0)//每页可以放八个
        {
            string[] vkey = key.Split(' ');
            string[] textItem = Resources.Load<TextAsset>("items").ToString().Split('\n');//按行分割
            for (int index = 0; index <= vkey.Length - 1; index++)
            {
                string[] contentS = vkey[index].Split('x');//（物品ID x 价格 x 数量）
                GameObject preset = Instantiate(Resources.Load<GameObject>("preset/UI/global/items/MallSpace"));//preset/UI/global/items/MallSpace
                preset.transform.SetParent(listBgImage);
                MallSpace mallSpace = preset.GetComponent<MallSpace>();
                mallSpace.mallBgImage = this;
                mallSpace.UpdateItemInformation(textItem[int.Parse(contentS[0])], int.Parse(contentS[2]), int.Parse(contentS[1]));
            }
        }
    }

    public void OnShutDown()//关闭按钮
    {
        if(optionsImage != null) optionsImage.AllowBackpack();
        Destroy(gameObject);
    }

    public void OnShutDownPPPPPPPPPPPPPPP()//关闭按钮
    {
        if (optionsImage != null) optionsImage.AllowBackpack();
        Destroy(gameObject);
    }

    private GameObject buyPrompt;//购买提示物体
}
