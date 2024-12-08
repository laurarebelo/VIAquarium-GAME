using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraveyardSorting : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    private DeadFishManager deadFishManager;

    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        deadFishManager = GameObject.Find("DeadFishManager").GetComponent<DeadFishManager>();
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
    }

    void OnDropdownValueChanged(int index)
    {
        switch (index)
        {
            case 0:
                _ = deadFishManager.SortByLastDied();
                break;
            case 1:
                _ = deadFishManager.SortByMostRespect();
                break;
            case 2:
                _ = deadFishManager.SortByMostDaysLived();
                break;
        }
    }
}
