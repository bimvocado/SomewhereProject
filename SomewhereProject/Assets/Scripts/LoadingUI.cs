using TMPro;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;

    public void SetEpisodeInfo(EpisodeData episodeData)
    {
        if (episodeData == null) return;
        descriptionText.text = episodeData.EpisodeDescription;
    }
}

