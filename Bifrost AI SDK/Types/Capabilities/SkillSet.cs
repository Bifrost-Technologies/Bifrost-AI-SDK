namespace Bifrost_AI_SDK.Types
{
    public class SkillSet
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Skills { get; set; }
        public SkillSet(string name, string description, List<string> skills)
        {
            Name = name;
            Description = description;
            Skills = skills;
        }
        public override string ToString()
        {
            return $"{Name}: {Description} - Skills: {string.Join(", ", Skills)}";
        }
    }
}
