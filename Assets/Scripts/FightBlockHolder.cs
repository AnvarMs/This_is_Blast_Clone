
using System.Collections.Generic;
using UnityEngine;

public class FightBlockHolder :MonoBehaviour
{
    [SerializeField]
    public  List<Shooter> shooterList;

    static FightBlockHolder instance;

    public static FightBlockHolder Instance {  get { return instance; } }
    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else
        {
            Destroy(instance);

        }
    }



}
