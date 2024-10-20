namespace Model
{
    [System.Serializable]
    public class HungerPostObject
    {
        public int hungerPoints;

        public HungerPostObject(int hungerPoints)
        {
            this.hungerPoints = hungerPoints;
        }
    }
}