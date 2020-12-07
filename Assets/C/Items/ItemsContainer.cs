using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemsContainer : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler //物品容器（物品栏）
{
    //public ItemsAttribute items = null;//物品

    public SpecificItems specificItems = null;

    public KeyCode keyCode;//快捷键
    public int wearPosition;//穿戴位置

    [SerializeField] private MouseControlledMovement MouseControlledMovement;

    //private bool usingInterval;//使用间隔
    private Image cdImage;//冷却显示
    private float skillsCd;//技能CD
    private float cdDelta;//冷却了多久了
    private bool availableOrNot = true;//是否可用，默认可用
    private Text cdText;//当前CD时
    private Image cdImgae;//
    private Text numberText;//数量

    private ItemsListBgImage itemsListBgImage;//父面板
    public ItemsListBgImage ParentPanel
    {
        get { return itemsListBgImage; }
    }
    private void Awake()
    {
        itemsListBgImage = transform.parent.parent.GetComponent<ItemsListBgImage>();

        cdImgae = transform.Find("CDImage").GetComponent<Image>();
        cdText = transform.Find("CDText").GetComponent<Text>();
        numberText = transform.Find("NumberText").GetComponent<Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            OnUseItems();
        }
    }

    public void SetItems(SpecificItems itemsAttribute)//设置容器上的物品
    {
        if (specificItems.items != null) OnEmptyItems();//先保证置空
        specificItems = itemsAttribute;
        if (specificItems.items.itemsGainAttribute.Length == 0) return;
        foreach (ItemsGainAttribute varIL in specificItems.items.itemsGainAttribute)//默认效果增加
        {
            if (varIL.itemsEffectType == ItemsEffectType.DefaultEffect)
            {
            }
        }
        //items.Items.wearPosition = wearPosition;//设置穿戴位置
    }

    public void OnUseItems()//使用物品
    {
        if (!availableOrNot) return;//CD时间中
        if (specificItems.items.ID == -1) return;//没有物品就跳过
        if (specificItems.items.itemsGainAttribute.Length == 0) return;//没有效果就不执行

        foreach (ItemsGainAttribute varIL in specificItems.items.itemsGainAttribute)//主动效果触发
        {
            if (varIL.itemsEffectType == ItemsEffectType.ActiveEffect)//判断是不是主动效果
            {
                //if(varIL.itemsGainAttributeListType == ItemsGainAttributeListType.力量)//itemsGainAttributeListType
                switch (varIL.itemsGainAttributeListType)
                {
                    case ItemsGainAttributeListType.生命值:
                        MouseControlledMovement.GetHeroAttribute.HpAcceptDamage(( -1 * varIL.value), DamageClassType.真实伤害, MouseControlledMovement.GetHeroAttribute.gameObject); 
                        break;
                    case ItemsGainAttributeListType.魔法值:
                        MouseControlledMovement.GetHeroAttribute.MagicConsumption(-1 * varIL.value);
                        break;
                }
                skillsCd = varIL.cd;
            }
        }

        if (specificItems.number > 0)
        {
            specificItems.number -= 1;//减少数量（数量消耗）

            if (specificItems.number <= 0)
            {
                specificItems = new SpecificItems();//使用完了要消失
                specificItems.items.ID = -1;
                ItemsDisplay();//显示设置
                //numberText.text = "";
            }
            else
            {
                numberText.text = specificItems.number.ToString();//显示数量
            }

            itemsListBgImage.OnStorage();//存储
        }

        coroutine = StartCoroutine(SkillsCD());//冷却触发
    }

    public void OnEmptyItems()//置空物品容器
    {
        if (specificItems == null) return;//为空则逃过
        if (specificItems.items.itemsGainAttribute.Length > 0)//默认效果删除
        {
            foreach (ItemsGainAttribute varIL in specificItems.items.itemsGainAttribute)
            {
                if (varIL.itemsEffectType == ItemsEffectType.DefaultEffect)
                {
                }
            }
        }
        //items.Items.wearPosition = 0;//置空穿戴位置
        specificItems = null;
    }

    private GameObject originalGameObject = null;//拖拉显示体
    public void OnBeginDrag(PointerEventData eventData)//开始拖拉
    {
        if (specificItems.items.ID == -1) return;
        if (originalGameObject != null) return;
        originalGameObject = Instantiate(Resources.Load<GameObject>("preset/UI/global/items/BackpackSpace"));
        originalGameObject.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
        originalGameObject.transform.SetParent(transform.parent.parent.parent);
        originalGameObject.transform.localPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)//拖拉进行中
    {
        if (originalGameObject == null) return;
        originalGameObject.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)//拖拉结束
    {
        if (specificItems.items.ID == -1) return;
        
        if (itemsListBgImage.backpackBgImage != null)
        {
            foreach (var backp in itemsListBgImage.backpackBgImage.BackpackSpace)
            {
                if (ItemsAttribute.IsRectTransformOverlap(originalGameObject.GetComponent<RectTransform>(), backp.GetComponent<RectTransform>()))
                {
                    BackpackSpace backpackSpace = backp.GetComponent<BackpackSpace>();
                    SpecificItems specificItemsL = backpackSpace.items;
                    backpackSpace.items = specificItems;
                    specificItems = specificItemsL;//替换

                    itemsListBgImage.OnStorage();//存储
                    backpackSpace.ParentPanel.OnStorage();

                    ItemsDisplay();//if()
                    backpackSpace.ItemsDisplay();//显示

                    Destroy(originalGameObject);
                    originalGameObject = null;
                    return;
                }
            }
        }

        foreach (var backp in itemsListBgImage.ItemsContainer)
        {
            if (ItemsAttribute.IsRectTransformOverlap(originalGameObject.GetComponent<RectTransform>(), backp.GetComponent<RectTransform>()))
            {
                ItemsContainer backpackSpace = backp.GetComponent<ItemsContainer>();
                SpecificItems specificItemsL = backpackSpace.specificItems;
                backpackSpace.specificItems = specificItems;
                specificItems = specificItemsL;//替换

                itemsListBgImage.OnStorage();//存储

                ItemsDisplay();//if()
                backpackSpace.ItemsDisplay();//显示

                Destroy(originalGameObject);
                originalGameObject = null;
                return;
            }
        }

        Destroy(originalGameObject);
        originalGameObject = null;
    }

    public void ItemsDisplay()//物品显示
    {
        if (specificItems.items.ID == -1)
        {
            numberText.text = "";
            //transform.Find("number").gameObject.SetActive(false);
            transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("icon/items/bg_道具");
            return;
        }

        if (specificItems.number > 0) numberText.text = specificItems.number.ToString();

        transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(specificItems.items.ico);//显示图标
        //if (items.number > 0)
        //{
        //    Text text = transform.Find("number").GetComponent<Text>();
        //    text.text = items.number.ToString();//显示数量
        //    text.gameObject.SetActive(true);
        //}
        //if (items.wearLocation > 0) transform.Find("WearImage").gameObject.SetActive(true);//是否装备
    }

    private Coroutine coroutine;//用返回的Coroutine来yield return可以等待协同程序运行后，再继续运行
    private IEnumerator SkillsCD()
    {
        yield return 1;

        cdDelta += Time.deltaTime;

        int cdInt = (int)(skillsCd - cdDelta);
        cdText.text = cdInt.ToString();

        cdImgae.fillAmount = (skillsCd - cdDelta) / skillsCd;

        if (cdDelta >= skillsCd)
        {
            ResetCooling();
        }
        else
        {
            coroutine = StartCoroutine(SkillsCD());
        }
    }

    public void ResetCooling() //重置技能冷却时间
    {
        availableOrNot = true;
        //availableOrYot = false;
        cdText.gameObject.SetActive(false);
        cdImgae.gameObject.SetActive(false);
        cdDelta = 0f;
        StopCoroutine(coroutine);
    }

    private void OnDestroy()//脚本或物体被销毁时调用（在执行OnDestroy前会先执行OnDisable）
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }

}