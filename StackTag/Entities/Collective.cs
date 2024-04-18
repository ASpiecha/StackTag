namespace StackTag.Entities
{
    public class Collective
    {
        public int Id { get; set; }
        public List<string> Tags { get; set; }
        public List<ExternalLink> ExternalLinks { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Slug { get; set; }
    }
}
