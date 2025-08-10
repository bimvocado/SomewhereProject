using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Contact")]
public class ContactData : ScriptableObject
{
    public List<string> requiredFlags;

    public string contactName;
    public string contactMessage;
    public Sprite contactImage;
    public string characterName;
}