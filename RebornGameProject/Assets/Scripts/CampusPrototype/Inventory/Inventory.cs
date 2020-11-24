﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 작성자 : 이성호
/// 기능 : 인벤토리 관리
/// </summary>
public class Inventory : SingletonBase<Inventory>
{
    private List<ItemSlot> itemSlots;   // 아이템 슬롯 관리 리스트
    private RectTransform itemPanel;
    private ItemSlot selectedSlot;
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults;

    [SerializeField] private Canvas itemPopUpCanvas;
    private Image popUpImage;

    [SerializeField] private RectTransform inventoryTransform;
    [SerializeField, Range(1.0f, 100.0f)] private float inventoryMoveTime = 5.0f;
    private Vector2 inventoryDefaultPos;
    private Vector2 inventoryHidePos;

    private Coroutine currentCoroutine;
    public Coroutine CurrentCoroutine
    {
        get { return currentCoroutine; }
        set { currentCoroutine = value; }
    }

    [SerializeField, Range(1.0f, 10.0f)]
    private float waitTime = 2.0f;
    private WaitForSeconds waitForSecondsTime;

    private InventoryMouse inventoryMouse;


    private Coroutine tickCoroutine = null;
    [SerializeField]
    private float activateTime = 10.0f;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        itemSlots = new List<ItemSlot>(8);
        itemPanel = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();    // 0번째를 원본으로 복사본 생성
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(null);
        raycastResults = new List<RaycastResult>();

        itemSlots.Add(itemPanel.GetComponent<ItemSlot>());


        // 슬롯 추가
        for (int i = 1; i < itemSlots.Capacity; ++i)
        {
            RectTransform panel = Instantiate(itemPanel, transform.GetChild(0));
            panel.anchoredPosition = new Vector2(panel.anchoredPosition.x + 50 * i, panel.anchoredPosition.y);
            itemSlots.Add(panel.GetComponent<ItemSlot>());
        }

        transform.Find("DragImage").SetParent(transform.Find("InventoryPanel"));

        popUpImage = itemPopUpCanvas.transform.Find("PopUpImage").GetComponent<Image>();

        inventoryDefaultPos = inventoryTransform.anchoredPosition;
        inventoryHidePos = new Vector2(inventoryDefaultPos.x, inventoryDefaultPos.y - 80.0f);

        inventoryTransform.anchoredPosition = inventoryHidePos;

        waitForSecondsTime = new WaitForSeconds(waitTime);

        inventoryMouse = FindObjectOfType<InventoryMouse>();
    }

    public void DeactivateItemPopUp()
    {
        itemPopUpCanvas.enabled = false;
    }

    // 마우스 버튼 클릭 시
    public void MouseUp()
    {
        pointerEventData.position = Input.mousePosition;    // 이벤트 발생 위치를 마우스 위치로 지정
        raycastResults.Clear(); // 레이캐스팅 이전 결과 초기화
        graphicRaycaster.Raycast(pointerEventData, raycastResults); // UI 레이캐스팅

        // 레이캐스팅 된 UI가 없거나
        // ItemSlot을 가진 UI가 아니거나
        // ItemSlot에 아이템을 가지지 않은 경우 함수 종료
        if (raycastResults.Count == 0
            || raycastResults[0].gameObject.GetComponent<ItemSlot>() == null
            || raycastResults[0].gameObject.GetComponent<ItemSlot>().Item == null)
        {
            return;
        }

        ItemSlot resultSlot = raycastResults[0].gameObject.GetComponent<ItemSlot>();

        // 팝업 창 띄우기
        if (!Chat.Instance.IsActivateChat)
        {
            tickCoroutine = StartCoroutine(TickActivateTime());
            itemPopUpCanvas.enabled = true;
            //itemPopUpCanvas.GetComponent<DeactivateInvenItemPopUp>().enabled = true;
            popUpImage.sprite = resultSlot.Item.Sprite;
        }

        #region 슬롯선택
        // 다른 슬롯이 이미 선택되었는지 검사하는 반복문
        foreach (ItemSlot slot in itemSlots)
        {
            // 자기 자신은 검사 대상에서 제외
            if (slot.Equals(resultSlot))
            {
                continue;
            }

            // 다른 슬롯이 이미 선택되어 있을 경우
            if (slot.Selected == true)
            {
                // 선택되었었던 슬롯 비활성화
                slot.Selected = false;

                // 클릭한 슬롯 활성화
                resultSlot.Selected = true;
                selectedSlot = resultSlot;

                return;  // 어차피 하나만 활성화 중 일 것이므로 함수 종료
            }
        }

        if (resultSlot.Selected == true)
        {
            selectedSlot = null;
            resultSlot.Selected = false;

            DeactivateItemPopUp();
        }
        else
        {
            selectedSlot = resultSlot;
            resultSlot.Selected = true;
        }
        #endregion
    }

    // 아이템 획득
    public bool GetItem(ItemLSH item)
    {
        for (int i = 0; i < 8; ++i)
        {
            if (itemSlots[i].Item == null)
            {
                itemSlots[i].Item = item;

                return true;
            }
        }

        Debug.Log("ItemSlot is Full (WTF)");
        return false;
    }

    // 아이템 사용
    public bool UseItem(string itemName)
    {
        for (int i = 0; i < itemSlots.Count; ++i)
        {
            if (itemSlots[i].Item != null && itemName.Equals(itemSlots[i].Item.ItemName))
            {
                itemSlots[i].Item = null;
                DeactivateItemPopUp();
                UpAndDownInven();

                return true;
            }
        }

        return false;
    }

    // 선택되어 있는 슬롯의 아이템 사용
    public bool UseSelectedItem(string itemName)
    {
        // 슬롯이 선택되어 있지 않거나 아이템이 없다면 거짓 반환
        if (selectedSlot == null
            || selectedSlot.Item == null)
        {
            return false;
        }

        // 맞는 아이템 사용
        if (itemName.Equals(selectedSlot.Item.ItemName))
        {
            selectedSlot.Selected = false;
            selectedSlot.Item = null;
            selectedSlot = null;
            DeactivateItemPopUp();
            UpAndDownInven();

            return true;
        }

        return false;
    }

    // 아이템 찾기
    public bool FindItem(string itemName)
    {
        for (int i = 0; i < itemSlots.Count; ++i)
        {
            if (itemSlots[i].Item == null)
            {
                continue;
            }

            if (itemName.Equals(itemSlots[i].Item.ItemName))
            {
                return true;
            }
        }

        return false;
    }

    // 마우스가 패널에 들어와 올라가도록 구현
    public IEnumerator UpInventory()
    {
        while (inventoryTransform.anchoredPosition.y < inventoryDefaultPos.y - 0.5f)
        {
            inventoryTransform.anchoredPosition = Vector2.Lerp(inventoryTransform.anchoredPosition, inventoryDefaultPos, Time.deltaTime * inventoryMoveTime);

            yield return null;
        }

        inventoryTransform.anchoredPosition = inventoryDefaultPos;
    }

    // 마우스가 패널 위치에서 벗어나 내려가도록 구현
    public IEnumerator DownInventory()
    {
        while (inventoryTransform.anchoredPosition.y > inventoryHidePos.y + 0.5f)
        {
            inventoryTransform.anchoredPosition = Vector2.Lerp(inventoryTransform.anchoredPosition, inventoryHidePos, Time.deltaTime * inventoryMoveTime);

            yield return null;
        }

        inventoryTransform.anchoredPosition = inventoryHidePos;
    }

    // 아이템 획득 시 자동으로 위로 올라왔다가 몇 초 후 아래로 내려가도록 구현
    public IEnumerator UpAndDownInventory()
    {
        //inventoryMouse.enabled = false;

        while (inventoryTransform.anchoredPosition.y < inventoryDefaultPos.y - 0.5f)
        {
            inventoryTransform.anchoredPosition = Vector2.Lerp(inventoryTransform.anchoredPosition, inventoryDefaultPos, Time.deltaTime * inventoryMoveTime);

            yield return null;
        }

        inventoryTransform.anchoredPosition = inventoryDefaultPos;

        yield return waitForSecondsTime;

        while (inventoryTransform.anchoredPosition.y > inventoryHidePos.y + 0.5f)
        {
            inventoryTransform.anchoredPosition = Vector2.Lerp(inventoryTransform.anchoredPosition, inventoryHidePos, Time.deltaTime * inventoryMoveTime);

            yield return null;
        }

        inventoryTransform.anchoredPosition = inventoryHidePos;

        //inventoryMouse.enabled = true;
    }

    public void StopCoroutineInline()
    {
        if (CurrentCoroutine != null)
        {
            StopCoroutine(CurrentCoroutine);
            //StopAllCoroutines();    //TODO: 위험
            CurrentCoroutine = null;
        }
    }

    private IEnumerator TickActivateTime()
    {
        float tickTime = 0.0f;

        while (tickTime <= activateTime)
        {
            tickTime = tickTime + Time.deltaTime;

            yield return null;
        }

        DeactivateItemPopUp();
    }

    public Coroutine UpInven()
    {
        StopCoroutineInline();

        return CurrentCoroutine = StartCoroutine(UpInventory());
    }

    public Coroutine DownInven()
    {
        StopCoroutineInline();

        return CurrentCoroutine = StartCoroutine(DownInventory());
    }

    public Coroutine UpAndDownInven()
    {
        StopCoroutineInline();

        return CurrentCoroutine = StartCoroutine(UpAndDownInventory());
    }
}
