using UnityEngine;

[System.Serializable]
public struct AffectionChangeData
{
    public string targetCharacter;
    public int changeValue;
}

[CreateAssetMenu(fileName = "New ItemData", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    [Header("아이템 고유 ID")]
    public string itemID;

    [Header("기본 정보")]
    public string itemName;
    [TextArea(3, 5)]
    public string itemDescription;
    public Sprite itemIcon;

    [Header("상점 정보")]
    public int price;

    [Header("사용 효과")]
    public AffectionChangeData affectionChange;
}