namespace Model
{
    [System.Serializable]
    public class FishNeedResponse
    {
        public int id;
        public int needLevel;
        //public string type;

        public FishNeedResponse(int id, string type, int needLevel)
        {
            this.id = id;
            //this.type = type;
            this.needLevel = needLevel;
        }
    }
}