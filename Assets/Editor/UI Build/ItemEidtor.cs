using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System;
using System.Linq;


public class ItemEidtor : EditorWindow
{
    private ItemDataList_So dataBase;
    private List<ItemDetails> itemLists = new List<ItemDetails>();

    //模板listview
    private VisualTreeAsset itemRowTemplate;
    private ListView itemListView;

    private ScrollView itemDetailsSection;
    private ItemDetails activeItem;

    //icon图标更新
    private VisualElement iconPreview;

    private Sprite defaultIcon;

    //枚举类型
    EnumField eField;
    ItemType itemType;

    [MenuItem("popingzhe/ItemEidtor")]
    public static void ShowExample()
    {
        ItemEidtor wnd = GetWindow<ItemEidtor>();
        wnd.titleContent = new GUIContent("ItemEidtor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
/*        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);
*/
        // Import UXML


        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Build/ItemEidtor.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        //加载list模板
        itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Build/ItemRow Template.uxml");
        defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");

        //变量赋值
        itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
        itemDetailsSection = root.Q<ScrollView>("itemDetails");
        iconPreview = itemDetailsSection.Q<VisualElement>("icon");
        eField = itemDetailsSection.Q<EnumField>("ItemType");

        //注册enumfield
        itemType = ItemType.defalut;
        eField.Init(itemType,true);

        //加载数据
        LoadDataBase();
        //生成list
        GenerateListView();

    }

    private void LoadDataBase()
    {
        string  path = "Assets/GameData/Invertory/ItemDataList_So.asset";
        dataBase = AssetDatabase.LoadAssetAtPath(path, typeof(ItemDataList_So)) as ItemDataList_So;

        itemLists = dataBase.itemDataList;
        //标记，不标记无法保存数据
        EditorUtility.SetDirty(dataBase);
        //Debug.Log(itemLists[0].itemID);
    }

    private void GenerateListView()
    {
        Func<VisualElement> makeItem =()=>itemRowTemplate.CloneTree();

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            if(i<itemLists.Count)
            {
                if (itemLists[i] != null)
                    e.Q<VisualElement>("icon").style.backgroundImage = itemLists[i].itemIcon.texture;
                e.Q<Label>("Name").text = itemLists[i] == null ? "NO ITEM" : itemLists[i].itemName;
            }
        };
        itemListView.fixedItemHeight = 60;
        itemListView.itemsSource = itemLists;
        itemListView.makeItem = makeItem;
        itemListView.bindItem = bindItem;

        //添加切换事件
         itemListView.onSelectionChange += OnListSelectionChange;
        //初始默认为不显示
        itemDetailsSection.visible = false;
        
        
    }

    private void OnListSelectionChange(IEnumerable<object> selectItem)
    {
        activeItem = (ItemDetails)selectItem.First();
        GetItemDetails();
        itemDetailsSection.visible = true;
    }

    private void GetItemDetails()
    {
        //刷新数据
        itemDetailsSection.MarkDirtyRepaint();

        itemDetailsSection.Q<IntegerField>("ItemId").value = activeItem.itemID;
        itemDetailsSection.Q<IntegerField>("ItemId").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemID = evt.newValue;
        });

        itemDetailsSection.Q<TextField>("ItemName").value = activeItem.itemName;
        itemDetailsSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemName = evt.newValue;
            itemListView.Rebuild();
        });

        //更新预览图片
        iconPreview.style.backgroundImage = activeItem.itemIcon.texture == null? defaultIcon.texture : activeItem.itemIcon.texture;
        itemDetailsSection.Q<ObjectField>("ItemIcon").value = activeItem.itemIcon;
        itemDetailsSection.Q<ObjectField>("ItemIcon").RegisterValueChangedCallback(evt =>
        {
            Sprite newIcon = evt.newValue as Sprite;
            activeItem.itemIcon = newIcon;
            iconPreview.style.backgroundImage = newIcon.texture == null? defaultIcon.texture:newIcon.texture;
            itemListView.Rebuild();
        });

        itemDetailsSection.Q<EnumField>("ItemType").value = activeItem.itemType;
        itemDetailsSection.Q<EnumField>("ItemType").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemType = (ItemType)evt.newValue;
        });

        itemDetailsSection.Q<TextField>("itemDescription").value = activeItem.itemDescription;
        itemDetailsSection.Q<TextField>("itemDescription").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemDescription = evt.newValue;
        });


        itemDetailsSection.Q<IntegerField>("itemUseRadius").value = activeItem.itemUseRadius;
        itemDetailsSection.Q<IntegerField>("itemUseRadius").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemUseRadius = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("canPickUp").value = activeItem.canPickUp;
        itemDetailsSection.Q<Toggle>("canPickUp").RegisterValueChangedCallback(evt =>
        {
            activeItem.canPickUp = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("canDropped").value = activeItem.canDropped;
        itemDetailsSection.Q<Toggle>("canDropped").RegisterValueChangedCallback(evt =>
        {
            activeItem.canDropped = evt.newValue;
        });

        itemDetailsSection.Q<Toggle>("canCarried").value = activeItem.canCarried;
        itemDetailsSection.Q<Toggle>("canCarried").RegisterValueChangedCallback(evt =>
        {
            activeItem.canCarried = evt.newValue;
        });

        itemDetailsSection.Q<IntegerField>("itemPrice").value = activeItem.itemPrice;
        itemDetailsSection.Q<IntegerField>("itemPrice").RegisterValueChangedCallback(evt =>
        {
            activeItem.itemPrice = evt.newValue;
        });

        itemDetailsSection.Q<Slider>("SellPercentage").value = activeItem.SellPercentage;
        itemDetailsSection.Q<Slider>("SellPercentage").RegisterValueChangedCallback(evt =>
        {
            activeItem.SellPercentage = evt.newValue;
        });
    }

}