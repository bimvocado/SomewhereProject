using UnityEngine;

[CreateAssetMenu(fileName = "EpisodeData", menuName = "Episode Data")]
public class EpisodeData : ScriptableObject
{
    public string EpisodeName;

    [TextArea] public string EpisodeDescription;
}
