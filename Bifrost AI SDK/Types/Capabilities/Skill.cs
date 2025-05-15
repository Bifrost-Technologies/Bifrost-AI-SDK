namespace Bifrost_AI_SDK.Types
{
    public class Skill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
        public Skill(string name, string description, List<string> tags)
        {
            Name = name;
            Description = description;
            Tags = tags;
        }
        public override string ToString()
        {
            return $"{Name}: {Description} - Tags: {string.Join(", ", Tags)}";
        }
    }
}
