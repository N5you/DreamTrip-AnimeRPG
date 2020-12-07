using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsImage : MonoBehaviour
{
    public ItemsListBgImage itemsListBgImage;//绑定这个可以让打开商城的同时，禁止背包存在
    public CurrencyTopBg currencyTopBg;

    private GameObject presetMall;//商城预设体
    private FlowInflationAttribute flowInflationAttribute;
    public FlowInflationAttribute FlowInflationAttribute
    {
        get
        {
            return flowInflationAttribute;
        }
    }
    public void OnOpenMall()//打开商城
    {
        if (presetMall != null) return;//只允许打开一个页面
        DeleteBackpack();
        presetMall = Instantiate(Resources.Load<GameObject>("preset/UI/global/items/MallBgImage"));
        presetMall.transform.SetParent(transform.parent,false);
        presetMall.transform.localPosition = new Vector3(0, 0, 0);
        presetMall.GetComponent<MallBgImage>().optionsImage = this;
        presetMall.transform.localScale = new Vector3(1, 1, 1);
    }

    public void DeleteBackpack()
    {
        if (currencyTopBg != null)
        {
            currencyTopBg.gameObject.SetActive(false);
            flowInflationAttribute = currencyTopBg.FlowInflationAttribute;
        }

        if (itemsListBgImage == null) return;
        itemsListBgImage.transform.Find("BackpackButton").GetComponent<Button>().interactable = false;//BackpackButton
        if (itemsListBgImage.backpackBgImage == null) return;
        Destroy(itemsListBgImage.backpackBgImage.gameObject);
    }

    private int GetTradingCurrencyNumber()//获取交易货币数量
    {
        int key = 0;
        return key;
    }

    public void AllowBackpack()
    {
        if (currencyTopBg != null)
        {
            currencyTopBg.OnUpdateCurrency(flowInflationAttribute);
            currencyTopBg.gameObject.SetActive(true);
        }

        if (itemsListBgImage == null) return;
        itemsListBgImage.transform.Find("BackpackButton").GetComponent<Button>().interactable = true;
    }
}