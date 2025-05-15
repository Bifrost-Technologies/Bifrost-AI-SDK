namespace Bifrost_AI_SDK.Types.Persona
{
    public class PersonalityDescriptors
    {
        // A list to hold the names of professions/roles
        public List<string> Professions { get; private set; }

        // Properties to hold descriptors for each category
        public List<string> ExpertDescriptors { get; private set; }
        public List<string> HistoricalDescriptors { get; private set; }
        public List<string> AnalyticalDescriptors { get; private set; }
        public List<string> EmpatheticDescriptors { get; private set; }
        public List<string> CreativeDescriptors { get; private set; }

        // Constructor initializes all the descriptor lists
        public PersonalityDescriptors()
        {
            Professions = new List<string>
            {
                // Administration & Office Roles
                "Personal Assistant",
                "Executive Assistant",
                "Office Administrator",
                "Office Manager",
                "Project Manager",
                "Customer Service Representative",
                
                // Healthcare & Wellness
                "Doctor",
                "Physician",
                "Nurse",
                "Therapist",
                "Counselor",
                "Psychologist",
                "Medical Consultant",
                
                // Science & Research
                "Physicist",
                "Chemist",
                "Biologist",
                "Data Scientist",
                "Research Scientist",
                "Academic Researcher",
                
                // Technology & Engineering
                "Software Engineer",
                "Developer",
                "General Engineer",
                "Mechanical Engineer",
                "Electrical Engineer",
                "Security Analyst",
                "Systems Architect",
                "IT Specialist",
                "VP of Engineering",
                "Chief Technology Officer (CTO)",
                
                // Creative & Media
                "Artist",
                "Visual Artist",
                "Digital Artist",
                "Graphic Designer",
                "Industrial Designer",
                "Writer",
                "Author",
                "Musician",
                "Composer",
                "Film Director",
                "Film Producer",
                
                // Business, Finance & Law
                "Entrepreneur",
                "Business Owner",
                "Accountant",
                "Financial Analyst",
                "Economist",
                "Consultant",
                "Lawyer",
                "Legal Advisor",
                "Attorney",
                "Chief Marketing Officer (CMO)",
                
                // Education & Communication
                "Teacher",
                "Educator",
                "Professor",
                "Lecturer",
                "Journalist",
                "Reporter",
                "Content Curator",
                "Editor",
                "Public Relations Specialist",
                
                // Other Professional & Specialized Roles
                "Marketer",
                "Brand Strategist",
                "Historian",
                "Social Worker",
                "HR Manager",
                "Talent Developer",
                "Operations Manager",
                "Supply Chain Manager",
                "Business Analyst",
                "Event Planner"
            };

            // Expert or Specialist descriptors
            ExpertDescriptors = new List<string>
            {
                "knowledgeable",
                "expert",
                "specialist",
                "authoritative",
                "experienced",
                "skilled",
                "competent",
                "savvy",
                "masterful",
                "proficient",
                "insightful" // Bridges with broader personality
            };

            // Historical or Character-based descriptors
            HistoricalDescriptors = new List<string>
            {
                "Renaissance polymath",
                "visionary inventor",
                "classical scholar",
                "vintage",
                "old-world",
                "traditional",
                "legendary",
                "mythical",
                "historic",
                "renaissance man"
            };

            // Critic or Analytical descriptors
            AnalyticalDescriptors = new List<string>
            {
                "discerning",
                "analytical",
                "critical",
                "evaluative",
                "reasoned",
                "methodical",
                "objective",
                "systematic",
                "observant",
                "precise",
                "inquisitive"
            };

            // Empathetic and Supportive descriptors
            EmpatheticDescriptors = new List<string>
            {
                "empathetic",
                "caring",
                "supportive",
                "compassionate",
                "understanding",
                "nurturing",
                "considerate",
                "attentive",
                "warm",
                "kind",
                "patient",
                "reassuring",
                "gentle",
                "personable"
            };

            // Creative or Entertaining descriptors
            CreativeDescriptors = new List<string>
            {
                "witty",
                "imaginative",
                "playful",
                "innovative",
                "quirky",
                "artistic",
                "creative",
                "lively",
                "entertaining",
                "spirited",
                "charming",
                "vibrant"
            };
        }

        /// <summary>
        /// Retrieves all personality descriptors organized by type.
        /// </summary>
        /// <returns>A dictionary mapping descriptor type to its list of adjectives.</returns>
        public Dictionary<string, List<string>> GetAllDescriptors()
        {
            return new Dictionary<string, List<string>>
            {
                { "Expert", new List<string>(ExpertDescriptors) },
                { "Historical", new List<string>(HistoricalDescriptors) },
                { "Analytical", new List<string>(AnalyticalDescriptors) },
                { "Empathetic", new List<string>(EmpatheticDescriptors) },
                { "Creative", new List<string>(CreativeDescriptors) }
            };
        }

        /// <summary>
        /// Retrieves descriptors for a given category name (case-insensitive).
        /// Recognized category names include:
        ///  - "expert" or "specialist"
        ///  - "historical" or "character"
        ///  - "analytical" or "critic"
        ///  - "empathetic" or "supportive"
        ///  - "creative" or "entertaining"
        /// </summary>
        /// <param name="category">The descriptor category desired.</param>
        /// <returns>A list of descriptors belonging to that category.</returns>
        /// <exception cref="ArgumentException">Thrown if the category is not recognized.</exception>
        public List<string> GetDescriptors(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                throw new ArgumentException("Category must not be empty.");

            switch (category.Trim().ToLower())
            {
                case "expert":
                case "specialist":
                    return new List<string>(ExpertDescriptors);
                case "historical":
                case "character":
                    return new List<string>(HistoricalDescriptors);
                case "analytical":
                case "critic":
                    return new List<string>(AnalyticalDescriptors);
                case "empathetic":
                case "supportive":
                    return new List<string>(EmpatheticDescriptors);
                case "creative":
                case "entertaining":
                    return new List<string>(CreativeDescriptors);
                default:
                    throw new ArgumentException($"Descriptor category '{category}' not recognized.");
            }
        }

        /// <summary>
        /// Prints the list of professions to the console.
        /// </summary>
        public void PrintProfessions()
        {
            Console.WriteLine("Available AI Professions:");
            foreach (var profession in Professions)
            {
                Console.WriteLine($"- {profession}");
            }
        }
    }
}

