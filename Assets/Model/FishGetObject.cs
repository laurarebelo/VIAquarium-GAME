[System.Serializable]
public class FishGetObject
{
    public int id;
    public string name;
    public int hungerLevel;
    public string lastUpdatedHunger;
    public int socialLevel;
    public string lastUpdatedSocial;
    public string dateOfBirth;

    public FishGetObject(int id, string name, int hungerLevel, string lastUpdatedHunger, int socialLevel, string lastUpdatedSocial, string dateOfBirth)
    {
        this.id = id;
        this.name = name;
        this.hungerLevel = hungerLevel;
        this.lastUpdatedHunger = lastUpdatedHunger;
        this.socialLevel = socialLevel;
        this.lastUpdatedSocial = lastUpdatedSocial;
        this.dateOfBirth = dateOfBirth;
    }
}