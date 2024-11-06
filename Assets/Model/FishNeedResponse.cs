namespace Model
{
    [System.Serializable]
    public class FishNeedResponse
    {
        public int id;
        public int needLevel;

        public FishNeedResponse(int id, int needLevel)
        {
            this.id = id;
            this.needLevel = needLevel;
        }
    }
}