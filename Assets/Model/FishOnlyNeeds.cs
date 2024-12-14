namespace Model
{
    public class FishOnlyNeeds
    {
        public int id;
        public int hungerLevel;
        public int socialLevel;
        public FishOnlyNeeds(){}
        public FishOnlyNeeds(int id, int hungerLevel, int socialLevel)
        {
            this.id = id;
            this.hungerLevel = hungerLevel;
            this.socialLevel = socialLevel;
        }

    }
}