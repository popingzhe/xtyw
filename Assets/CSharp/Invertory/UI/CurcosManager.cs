using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class CurcosManager : MonoBehaviour
{
    public Sprite normal, tool, seed,Commodity;

    private Sprite currentSprite;

    private Image cursorImage;
    
    private RectTransform cursorCanvas;

    //是否启用
    public bool CursorEnable;
    //是否可用
    private bool cursorPositionOnValid;

    private Camera mianCamera;
    private Grid currentGrid;

    private Vector3 mosueWorldPosition;
    private Vector3Int mouseGirdPosition;

    private ItemDetails currentItem;
    private Transform playerTransform;
    private void OnEnable()
    {
        EventHander.ItemSelectEvent += OnItemSelectEvent;
        EventHander.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHander.AfterSceneloadEvent += OnAfterSceneloadEvent;
    }
    private void OnDisable()
    {
        EventHander.ItemSelectEvent -= OnItemSelectEvent;
        EventHander.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHander.AfterSceneloadEvent -= OnAfterSceneloadEvent;
    }

    private void OnBeforeSceneUnloadEvent()
    {
       CursorEnable = false;
    }

    private void OnAfterSceneloadEvent()
    {
      currentGrid = FindObjectOfType<Grid>();
    }

    private void OnItemSelectEvent(ItemDetails details, bool isSelected)
    {
        
        if (!isSelected)
        {
            CursorEnable = false;
            currentItem = null;
            currentSprite = normal;
        }
        else
        {
            currentItem = details;
            currentSprite = details.itemType switch
            {
                ItemType.Seed => seed,
                ItemType.Commodity => Commodity,
                ItemType.ChopTool => tool,
                ItemType.HoeTool => tool,
                ItemType.CollectTool => tool,
    
                _ => normal,
            };
            CursorEnable = true;
        }

    }

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        mianCamera = Camera.main;
        cursorCanvas = GameObject.FindGameObjectWithTag("CurcosCanvs").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        SetCurcosIamge(normal);
        currentSprite = normal;
    }

    private void Update()
    {
        if (cursorCanvas == null) return;

        cursorImage.transform.position = Input.mousePosition;

        if (InteractWithUI())
        {
            SetCurcosIamge(normal);
        }
        else
        {
            SetCurcosIamge(currentSprite);
            if(CursorEnable)
            {
                CheckCursorVaid();
                CheckPlayerInput();
            }
        }
        
    }


    private void CheckPlayerInput()
    {
        if(Input.GetMouseButtonDown(0)&& cursorPositionOnValid)
        {
            EventHander.CallMouseClickedEvent(mosueWorldPosition, currentItem);
        }
    }

    private void CheckCursorVaid()
    {
        mosueWorldPosition = mianCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        mosueWorldPosition.z = 0;
        mouseGirdPosition = currentGrid.WorldToCell(mosueWorldPosition);

        //    Debug.Log("world"+mosueWorldPosition+"   gird"+ mouseGirdPosition);
        var playerPos = currentGrid.WorldToCell(playerTransform.position);
        //使用范围限制
        if(Mathf.Abs(mouseGirdPosition.x - playerPos.x) > currentItem.itemUseRadius || 
            Mathf.Abs(mouseGirdPosition.y - playerPos.y) > currentItem.itemUseRadius)
        {
            SetCutsorInValid();
            return;
        }


        TileDetails currentTile = MapManager.GetTileDetailsOnMousePosition(mouseGirdPosition);

        if(currentTile != null)
        {
            CropDetails currentCrop = CropManager.Instance.GetCropDetails(currentTile.seedItem);
            switch (currentItem.itemType)
            {
                //物品类型判断
                case ItemType.Seed:
                    if(currentTile.daySinceDug > -1 &&currentTile.seedItem == -1) SetCutsorValid(); else SetCutsorInValid();
                    break;
                case ItemType.Commodity:
                    if (currentTile.canDropItem) SetCutsorValid();else SetCutsorInValid();
                    break;
                case ItemType.HoeTool:
                    if (currentTile.canDig) SetCutsorValid(); else SetCutsorInValid();
                    break;
                case ItemType.WaterTool:
                    if(currentTile.daySinceDug > -1 &&currentTile.daySinceWatered == -1 ) 
                        SetCutsorValid();
                    else SetCutsorInValid() ;
                    break;
                case ItemType.ChopTool:
                case ItemType.CollectTool:
                    if(currentCrop != null)
                    {
                        if(currentCrop.CheckToolAvailable(currentItem.itemID))
                        if(currentTile.growthDays >= currentCrop.TotalGrowthDays) SetCutsorValid(); else SetCutsorInValid();
                    }else SetCutsorInValid();
                    break;
            }
        }
        else
        {
            SetCutsorInValid();
        }
    }

    //鼠标样式
    #region
    private void SetCurcosIamge(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }

    private void SetCutsorValid()
    {
        cursorPositionOnValid = true;
        cursorImage.color = new Color(1, 1, 1, 1);
    }

    private void SetCutsorInValid()
    {
        cursorPositionOnValid= false;
        cursorImage.color = new Color(1, 0, 0, 0.4f); 
    }
#endregion
    //判断鼠标是否指向ui
    private bool InteractWithUI()
    {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }
}
