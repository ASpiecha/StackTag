namespace StackTag.Entities
{
    public class RootResponse
    {
        public int? Id { get; set; }
        public List<Tag>? Items { get; set; }
        public bool? HasMore { get; set; }
        public int? QuotaMax { get; set; }
        public int? QuotaRemaining { get; set; }
    }
}
