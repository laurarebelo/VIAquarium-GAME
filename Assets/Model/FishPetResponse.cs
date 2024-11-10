namespace Model
{
    [System.Serializable]
    public class FishPetResponse
    {
        public int id;
        public int socialLevel;

        public FishPetResponse(int id, int socialLevel)
        {
            this.id = id;
            this.socialLevel = socialLevel;
        }
    }
}