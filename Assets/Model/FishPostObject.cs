namespace Model
{
    [System.Serializable]
    public class FishPostObject
    {
        public string name;
        public string template;
        public string sprite;

        public FishPostObject(string name, string template, string image)
        {
            this.name = name;
            this.template = template;
            this.sprite = image;
        }
    }
}