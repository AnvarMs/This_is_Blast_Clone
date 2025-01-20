
using System.Collections.Generic;
using UnityEngine;

public class FightSlotManager : MonoBehaviour
{
    [SerializeField]
    public List<Transform> slots = new List<Transform>();

    [SerializeField]
    public bool[] slotFilld;
    private static FightSlotManager instance;
    public static FightSlotManager Instance {  get { return instance; } }

    private void Awake()
    {
        instance = this;
        slotFilld = new bool[slots.Count];
    }

    




}
