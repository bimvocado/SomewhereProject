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
    [Header("������ ���� ID")]
    public string itemID;

    [Header("�⺻ ����")]
    public string itemName;
    [TextArea(3, 5)]
    public string itemDescription;
    public Sprite itemIcon;

    [Header("���� ����")]
    public int price;

    [Header("��� ȿ��")]
    public AffectionChangeData affectionChange;
}