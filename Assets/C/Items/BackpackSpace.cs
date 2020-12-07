using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BackpackSpace : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler//背包的物品栏格子 //被点击调用的接口IPointerDownHandler
{
    public SpecificItems items;

    //[SerializeField] private ItemsAttribute items = new ItemsAttribute(); //物品信息
    //public ItemsAttribute Items
    //{
    //    get { return items; }
    //    set { items = value; }
    //}

    //[SerializeField] private int wearLocation = 0; //是否穿戴（穿戴位置），为0则默认没有
    //public int WearLocation
    //{
    //    get { return wearLocation; }
    //    set { wearLocation = value; }
    //}

    //public int number;//数量，为0则无限
    private BackpackBgImage itemsListBgImage;
    public BackpackBgImage ParentPanel
    {
        get { return itemsListBgImage; }
        set { itemsListBgImage = value; }
    }

    public bool doNotDrag = false;
    private GameObject originalGameObject = null;//拖拉显示体
    public void OnBeginDrag(PointerEventData eventData)//开始拖拉
    {
        if (doNotDrag) return;
        if (items.items.ID == -1) return;
        if (originalGameObject != null) return;
        originalGameObject = Instantiate(Resources.Load<GameObject>("preset/UI/global/items/BackpackSpace"));
        originalGameObject.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
        originalGameObject.transform.SetParent(transform.parent.parent.parent);
        originalGameObject.transform.localPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)//拖拉进行中
    {
        if (doNotDrag) return;
        if (originalGameObject == null) return;
        originalGameObject.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)//拖拉结束
    {
        if (doNotDrag) return;

        if (items.items.ID == -1)
        {
            Destroy(originalGameObject);
            originalGameObject = null;
            return;
        }

        if (itemsListBgImage.itemsListBgImage != null)
        {
            foreach (var backp in itemsListBgImage.itemsListBgImage.ItemsContainer)
            {
                if (ItemsAttribute.IsRectTransformOverlap(originalGameObject.GetComponent<RectTransform>(), backp.GetComponent<RectTransform>()))
                {
                    ItemsContainer itemsContainer = backp.GetComponent<ItemsContainer>();
                    SpecificItems specificItemsL = itemsContainer.specificItems;
                    itemsContainer.specificItems = items;
                    items = specificItemsL;//替换

                    itemsListBgImage.OnStorage();//存储
                    itemsContainer.ParentPanel.OnStorage();

                    ItemsDisplay();//if()
                    itemsContainer.ItemsDisplay();//显示

                    Destroy(originalGameObject);
                    originalGameObject = null;
                    itemsListBgImage.OnStorage();//存储
                    return;
                }
            }
        }

        Destroy(originalGameObject);
        originalGameObject = null;
    }

    private GameObject preset;//信息显示面板
    public void OnByClicking()//被点击
    {
        preset = Instantiate(Resources.Load<GameObject>("preset/UI/items/DetailedInformationBgImage"));
        DetailedInformationBgImage detailedInformationBgImage = preset.GetComponent<DetailedInformationBgImage>();
        preset.transform.SetParent(transform.parent.parent);
        preset.transform.localPosition = new Vector3(0, 0, 0);
        detailedInformationBgImage.SetBackpackSpace(this);
    }

    public void SetItemsAttribute(string str, int numB, int nu)//设置物品属性（类型数据，穿戴位置，数量）
    {
        if (str.Length == 0) return;//为空则跳过
        string[] key = str.Split('|');//类型ID，名字，图标，物品类型，装备类型，物品价格，最后是增益属性 //格式（物品ID | 数量 | 穿戴位置）

        items.items = new ItemsAttribute();
        items.items.ID = int.Parse(key[0]);
        items.items.name = key[1];
        items.items.ico = key[2];
        items.items.itemsType = (ItemsType)System.Enum.Parse(typeof(ItemsType), key[3]);
        items.items.equipmentType = (EquipmentType)System.Enum.Parse(typeof(EquipmentType), key[4]);
        items.items.price = int.Parse(key[5]);

        string[] effectL = key[6].Split(',');
        items.items.itemsGainAttribute = new ItemsGainAttribute[effectL.Length];

        for (int indexes = 0; indexes <= effectL.Length - 1; indexes++)//增益属性*增益属性效果*增益值*使用间隔|分割
        {
            string[] gainS = effectL[indexes].Split('*');
            items.items.itemsGainAttribute[indexes] = new ItemsGainAttribute(
                (ItemsEffectType)System.Enum.Parse(typeof(ItemsEffectType), gainS[0]),
                (ItemsGainAttributeListType)System.Enum.Parse(typeof(ItemsGainAttributeListType), gainS[1]), int.Parse(gainS[2]), float.Parse(gainS[3])
                );
        }

        items.number = nu; //设置数量
        ItemsDisplay();//设置显示
        if (numB > 0) SetWearLocation(numB);//设置穿戴位置
    }

    public void SetWearLocation(int index)//设置穿戴位置
    {
        items.wearLocation = index;
        Image image = transform.Find("WearImage").GetComponent<Image>();
        Text text = transform.Find("number").GetComponent<Text>();

        if (index > 0)
        {
            //image.gameObject.SetActive(true);//显示已装备
        }
        else
        {
            image.gameObject.SetActive(false);
        }

        if (items.number > 0)
        {
            text.text = items.number.ToString();//显示数量
            text.gameObject.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }

    public void AEmpty()//设空
    {
        items = new SpecificItems();
        ItemsDisplay();
    }

    public void ItemsDisplay()//物品显示
    {
        if (items.items.ID == -1)
        {
            transform.Find("number").gameObject.SetActive(false);
            transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("icon/items/bg_道具");
            return;
        }

        transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(items.items.ico);//显示图标
        if (items.number > 0)
        {
            Text text = transform.Find("number").GetComponent<Text>();
            text.text = items.number.ToString();//显示数量
            text.gameObject.SetActive(true);
        }
        //if (items.wearLocation > 0) transform.Find("WearImage").gameObject.SetActive(true);//是否装备
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.currentInputModule.input.GetMouseButtonDown(1))
        {
            if (items.items.ID == -1) return;
            if (preset != null) return;
            doNotDrag = true;
            OnByClicking();
            this.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
    }
}
