using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactManager : MonoBehaviour
{
    [SerializeField] private List<ContactUI> contacts;

    private void OnEnable()
    {
        StartCoroutine(UpdateContactsAfterFrame());
    }

    private IEnumerator UpdateContactsAfterFrame()
    {
        yield return new WaitForEndOfFrame();

        UpdateAllContactsStatus();
    }

    public void UpdateAllContactsStatus()
    {
        if (contacts == null || contacts.Count == 0)
        {
            return;
        }

        foreach (var contact in contacts)
        {
            if (contact != null)
            {
                contact.UpdateContactState();
            }
        }
    }
}