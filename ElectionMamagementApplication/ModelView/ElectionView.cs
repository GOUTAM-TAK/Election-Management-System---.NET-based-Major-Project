namespace ElectionMamagementApplication.ModelView
{
    public class ElectionView
    {
        public string ElectionName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public string ElectionStatus { get; set; } = null!;
    }
   
}
