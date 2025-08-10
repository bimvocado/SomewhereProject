using System.Collections.Generic;
using UnityEngine;

public class ContactManager : MonoBehaviour
{
    public static ContactManager Instance { get; private set; }

    [SerializeField] private List<ContactUI> contacts;
}