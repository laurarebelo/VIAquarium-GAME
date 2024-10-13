namespace Model
{
    [System.Serializable]
    public class FishGetObject
    {
        public int id;
        public string name;

        public FishGetObject(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}