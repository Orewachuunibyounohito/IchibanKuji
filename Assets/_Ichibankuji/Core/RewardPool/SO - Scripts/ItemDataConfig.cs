using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Ichibankuji.SO
{

    [CreateAssetMenu(menuName = "Ichibankuji/Config/Item", fileName = "New ItemData Config")]
    public class ItemDataConfig : ScriptableObject
    {
        public List<ItemDataConfigContent> Items;

        private void OnEnable(){
            #if UNITY_EDITOR
            foreach(var itemContent in Items){
                if(string.IsNullOrEmpty(
                    AssetDatabase.AssetPathToGUID($"{ResourcesPath.ITEM_DATA_DIRECTORY}")))
                {
                    itemContent.NoSaved = true;
                }
            }
            #endif
        }

        [Serializable]
        public class ItemDataConfigContent
        {
            private const string TAB_GROUP = "Tab" ;
            private const string BUTTON_GROUP = "Tab/Button" ;

            [InlineEditor]
            // [VerticalGroup("Vertical")]
            // [HorizontalGroup("Vertical/Horizontal")]
            public ItemData Item;

            public bool NoSaved{ get; set; }
            
            [TabGroup(TAB_GROUP, "New")]
            [ButtonGroup(BUTTON_GROUP)]
            [ShowIf("ItemIsEmptyOrNull")]
            [Button(SdfIconType.Plus, Name = "Empty", Style = ButtonStyle.FoldoutButton)]
            public void CreateNewItemData(){
                CreateNewItemDataWithArgument("New ItemData");
            }

            [TabGroup(TAB_GROUP, "New")]
            [ButtonGroup(BUTTON_GROUP)]
            [ShowIf("ItemIsEmptyOrNull")]
            [Button(SdfIconType.Plus, Name = "Argument", Style = ButtonStyle.FoldoutButton)]
            public void CreateNewItemDataWithArgument(string name, string description = ""){
                if(string.IsNullOrEmpty(name)){ return ; }
                NoSaved = true;
                ItemData data = CreateInstance<ItemData>();
                data.name = name;
                data.Name = name;
                data.Description = description;
                Item = data;
            }

            #if UNITY_EDITOR
            [ShowIf("@NoSaved")]
            [Button(Name = "Save Asset", Style = ButtonStyle.FoldoutButton)]
            public void CreateItemDataAsset(){
                if(ItemIsEmptyOrNull()){ return ; }
                NoSaved = false;
                AssetDatabase.CreateAsset(Item, $"{ResourcesPath.ITEM_DATA_DIRECTORY}/{Item.Name}.asset");
                Debug.Log($"Create {Item.Name} asset is done.");
            }
            #endif

            private bool ItemIsEmptyOrNull() => Item == null;
        }
    }
}