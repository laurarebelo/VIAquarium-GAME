[System.Serializable]
public class DeadFishGetObject
{
    public int id;
    public string name;
    public string template;
    public string sprite;
    public string dateOfBirth;
    public string dateOfDeath;
    public int daysLived;
    public int respectCount;
    public string causeOfDeath;
    
    public DeadFishGetObject(int id, string name, string dateOfBirth, string dateOfDeath, int daysLived, int respectCount, string causeOfDeath,string template, string sprite)
    {
        this.id = id;
        this.name = name;
        this.dateOfBirth = dateOfBirth;
        this.dateOfDeath = dateOfDeath;
        this.daysLived = daysLived;
        this.respectCount = respectCount;
        this.causeOfDeath = causeOfDeath;
        this.template = template;
        this.sprite = sprite;
    }
}