using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SortDeadFish : MonoBehaviour
{
    public List<DeadFishGetObject> Search(List<DeadFishGetObject> deadFishList, string query)
    {
        return deadFishList
            .Where(fish => fish.name.ToLower().Contains(query.ToLower()))
            .ToList();
    }

    public List<DeadFishGetObject> Filter(List<DeadFishGetObject> deadFishList, DeadFishSortType searchType)
    {
        switch (searchType)
        {
            case DeadFishSortType.LastDied:
                return deadFishList
                    .OrderByDescending(fish => System.DateTime.Parse(fish.dateOfDeath))
                    .ToList();

            case DeadFishSortType.DaysLived:
                return deadFishList
                    .OrderByDescending(fish => fish.daysLived)
                    .ToList();

            case DeadFishSortType.Respect:
                return deadFishList
                    .OrderByDescending(fish => fish.respectCount)
                    .ToList();
        }
        return deadFishList;
    }
}

public enum DeadFishSortType
{
    LastDied,
    DaysLived,
    Respect
}