namespace Model
{
    [System.Serializable]
    public class FishFedResponse
    {
        public int id;
        public int hungerLevel;

        public FishFedResponse(int id, int hungerLevel)
        {
            this.id = id;
            this.hungerLevel = hungerLevel;
        }
    }
}