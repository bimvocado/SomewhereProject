using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class ProfileImageState
{
    public string requiredFlag;
    public Sprite sprite;
}


[CreateAssetMenu(fileName = "Contact")]
public class ContactData : ScriptableObject
{
    public List<string> requiredFlags;

    public string contactName;
    public string contactMessage;
    public Sprite contactImage;
    public string characterName;

        public List<ProfileImageState> profileImageStates;
}