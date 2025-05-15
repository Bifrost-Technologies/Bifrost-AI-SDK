using System.Text;

namespace Bifrost_AI_SDK.Types.Genetics
{
    // Define a gender enum.
    public enum Gender
    {
        Male = 0,
        Female = 1,
        None = 2
    }

    // Persona holds gender, profession, and personality descriptors.
    public class Persona
    {
        public Gender Gender { get; set; }
        public string Profession { get; set; }
        public List<string> PersonalityDescriptors { get; set; }

        public Persona()
        {
            Profession = "";
            PersonalityDescriptors = new List<string>();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Gender);
            writer.Write(Profession ?? "");
            writer.Write(PersonalityDescriptors.Count);
            foreach (var desc in PersonalityDescriptors)
            {
                writer.Write(desc ?? "");
            }
        }

        public static Persona Deserialize(BinaryReader reader)
        {
            Persona p = new Persona
            {
                Gender = (Gender)reader.ReadByte(),
                Profession = reader.ReadString()
            };
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                p.PersonalityDescriptors.Add(reader.ReadString());
            }
            return p;
        }
    }

    // Identity combines the AI's name and persona.
    public class Identity
    {
        public string Name { get; set; }
        public Persona Persona { get; set; }

        public Identity()
        {
            Name = "";
            Persona = new Persona();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Name ?? "");
            Persona.Serialize(writer);
        }

        public static Identity Deserialize(BinaryReader reader)
        {
            Identity result = new Identity
            {
                Name = reader.ReadString(),
                Persona = Persona.Deserialize(reader)
            };
            return result;
        }
    }

    // CognitiveProcess holds the "chain-of-thought" reasoning.
    public class CognitiveProcess
    {
        public string ChainOfThought { get; set; }

        public CognitiveProcess()
        {
            ChainOfThought = "";
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(ChainOfThought ?? "");
        }

        public static CognitiveProcess Deserialize(BinaryReader reader)
        {
            CognitiveProcess result = new CognitiveProcess
            {
                ChainOfThought = reader.ReadString()
            };
            return result;
        }
    }

    // Capabilities holds the AI's skills.
    public class Capabilities
    {
        public List<string> Skills { get; set; }

        public Capabilities()
        {
            Skills = new List<string>();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Skills.Count);
            foreach (var skill in Skills)
            {
                writer.Write(skill ?? "");
            }
        }

        public static Capabilities Deserialize(BinaryReader reader)
        {
            Capabilities result = new Capabilities();
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                result.Skills.Add(reader.ReadString());
            }
            return result;
        }
    }

    // ChromosomeData now holds Identity, CognitiveProcess, and Capabilities.
    public class ChromosomeData
    {
        public Identity Identity { get; set; }
        public CognitiveProcess CognitiveProcess { get; set; }
        public Capabilities Capabilities { get; set; }

        public ChromosomeData()
        {
            Identity = new Identity();
            CognitiveProcess = new CognitiveProcess();
            Capabilities = new Capabilities();
        }

        public byte[] Serialize()
        {
            using MemoryStream ms = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true))
            {
                Identity.Serialize(writer);
                CognitiveProcess.Serialize(writer);
                Capabilities.Serialize(writer);
            }
            return ms.ToArray();
        }

        public static ChromosomeData Deserialize(byte[] data)
        {
            ChromosomeData result = new ChromosomeData();
            using MemoryStream ms = new MemoryStream(data);
            using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8, leaveOpen: true))
            {
                result.Identity = Identity.Deserialize(reader);
                result.CognitiveProcess = CognitiveProcess.Deserialize(reader);
                result.Capabilities = Capabilities.Deserialize(reader);
            }
            return result;
        }
    }

    // Demonstration:
    class Program
    {
        static void Main(string[] args)
        {
            var data = new ChromosomeData();
            data.Identity.Name = "Eva";
            data.Identity.Persona.Gender = Gender.Female;
            data.Identity.Persona.Profession = "Doctor";
            data.Identity.Persona.PersonalityDescriptors.AddRange(new[] { "compassionate", "meticulous", "empathetic" });
            data.CognitiveProcess.ChainOfThought = "Step 1: Analyze symptoms; Step 2: Consider diagnosis; Step 3: Recommend treatment.";
            data.Capabilities.Skills.AddRange(new[] { "Diagnosis", "Surgery", "Patient Care" });

            byte[] serialized = data.Serialize();
            ChromosomeData loadedData = ChromosomeData.Deserialize(serialized);

            Console.WriteLine("Name: " + loadedData.Identity.Name);
            Console.WriteLine("Gender: " + loadedData.Identity.Persona.Gender);
            Console.WriteLine("Profession: " + loadedData.Identity.Persona.Profession);
            Console.WriteLine("Personality Descriptors: " + string.Join(", ", loadedData.Identity.Persona.PersonalityDescriptors));
            Console.WriteLine("Chain-of-Thought: " + loadedData.CognitiveProcess.ChainOfThought);
            Console.WriteLine("Skills: " + string.Join(", ", loadedData.Capabilities.Skills));
        }
    }
}