using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CalendarImageChanger : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image calendarImageDisplay;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    [Header("Calendar Images")]
    [SerializeField] private List<Sprite> calendarSprites;

    private int currentImageIndex = 0;

    void Awake()
    {
        prevButton.onClick.AddListener(OnPrevButtonClicked);
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    void Start()
    {
        UpdateCalendarImage();
    }

    private void OnPrevButtonClicked()
    {
        currentImageIndex--;
        if (currentImageIndex < 0)
        {
            currentImageIndex = calendarSprites.Count - 1;
        }
        UpdateCalendarImage();
    }

    private void OnNextButtonClicked()
    {
        currentImageIndex++;
        if (currentImageIndex >= calendarSprites.Count)
        {
            currentImageIndex = 0;
        }
        UpdateCalendarImage();
    }

    private void UpdateCalendarImage()
    {
        if (calendarSprites != null && calendarSprites.Count > 0 && calendarImageDisplay != null)
        {
            calendarImageDisplay.sprite = calendarSprites[currentImageIndex];
            prevButton.interactable = currentImageIndex > 0;
            nextButton.interactable = currentImageIndex < calendarSprites.Count - 1;
        }
        else
        {
            Debug.LogWarning("달력 이미지 없음");
        }
    }
}