using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterColorData
{
    public string characterName;
    public Color backgroundColor = Color.white;
    public Color textColor = Color.black;
}

public class CharacterColorManager : MonoBehaviour
{
    public static CharacterColorManager Instance;

    [SerializeField] private CharacterColorData[] characterColors;
    [SerializeField] private CharacterColorData defaultColor;

    private Dictionary<string, CharacterColorData> colorMap;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializeColorMap();
    }

    private void InitializeColorMap()
    {
        colorMap = new Dictionary<string, CharacterColorData>();
        foreach (var colorData in characterColors)
        {
            colorMap[colorData.characterName] = colorData;
        }
    }

    public CharacterColorData GetColorData(string characterName)
    {
        if (string.IsNullOrEmpty(characterName)) return defaultColor;
        return colorMap.ContainsKey(characterName) ? colorMap[characterName] : defaultColor;
    }
}