using UnityEngine;

[CreateAssetMenu(fileName = "EpisodeData", menuName = "Episode Data")]
public class EpisodeData : ScriptableObject
{
    [TextArea] public string EpisodeDescription;
}
