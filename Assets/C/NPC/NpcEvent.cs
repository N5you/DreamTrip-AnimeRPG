using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NpcEvent : MonoBehaviour
{
    public static bool whetherItIsClicked = true;//面板是否有被点开

    public BeLeftClick beLeftClick; //被鼠标左击

    private Texture2D normalCursorb;//正常鼠标
    private Texture2D npcCursorb;//NPC鼠标
    private void Start()
    {
        normalCursorb = Resources.Load<Texture2D>("icon/mouse/mouse01");
        npcCursorb = Resources.Load<Texture2D>("icon/mouse/mouse50");
    }

    [SerializeField] private EventSystem eventSystem;//禁止鼠标穿透UGUI
    [SerializeField] private GraphicRaycaster RaycastInCanvas;//Canvas上有这个组件
    private bool CheckGuiRaycastObjects()//测试UI射线
    {
        if (eventSystem == null || RaycastInCanvas == null) return false;
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        RaycastInCanvas.Raycast(eventData, list);
        //Debug.Log(list.Count);
        return list.Count > 0;
    }

    private void OnMouseDown()//鼠标点击
    {
        if (CheckGuiRaycastObjects()) return;
        beLeftClick.Invoke(gameObject);
        Cursor.SetCursor(normalCursorb, Vector2.zero, CursorMode.Auto);
        NpcEvent.whetherItIsClicked = false;
    }

    private void OnMouseEnter()//鼠标进入
    {
        if (CheckGuiRaycastObjects()) return;
        if (!whetherItIsClicked) return;
        Cursor.SetCursor(npcCursorb, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit() //鼠标离开
    {
        if (CheckGuiRaycastObjects()) return;
        Cursor.SetCursor(normalCursorb, Vector2.zero, CursorMode.Auto);
    }
}

[System.Serializable]
public class BeLeftClick : UnityEvent<GameObject> { }