using Bifrost_AI_SDK;
using Bifrost_AI_SDK.Types.Genetics;
using Bifrost_AI_SDK.Types.Persona;
using Lifespark.Genetics;
using System.Text;

namespace Bifrost_AI_SDK.Tests
{
    class Program
    {
        static async void Main(string[] args)
        {
            byte[] SerializeToBytes(Action<BinaryWriter> serializeAction)
            {
                using MemoryStream ms = new MemoryStream();
                using (BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true))
                {
                    serializeAction(writer);
                }
                return ms.ToArray();
            }

            var data = new ChromosomeData();
            data.Identity.Name = "Eva";
            data.Identity.Persona.Gender = Gender.Female;
            data.Identity.Persona.Profession = "Assistant";
            var descriptors = new PersonalityDescriptors();
            data.Identity.Persona.PersonalityDescriptors.AddRange(new[] { descriptors.ExpertDescriptors[1], descriptors.AnalyticalDescriptors[5], descriptors.CreativeDescriptors[3] });
            data.CognitiveProcess.ChainOfThought = "Step 1: get the information by invoking a function you want to use. Step 2: process the data. Step 3: Display the data to the user.";
            data.Capabilities.Skills.AddRange(new[] { "TemporalAwareness" });

            byte[] identityBytes = SerializeToBytes(writer => data.Identity.Serialize(writer));
            byte[] cognitiveBytes = SerializeToBytes(writer => data.CognitiveProcess.Serialize(writer));
            //byte[] capabilitiesBytes = SerializeToBytes(writer => data.Capabilities.Serialize(writer));

            GeneticEncoder lifespark = new GeneticEncoder();
            Chromosome identity = new Chromosome(lifespark.WriteDNA(identityBytes));
            Chromosome cognitiveprocess = new Chromosome(lifespark.WriteDNA(cognitiveBytes));
            //Chromosome capabilities = new Chromosome(lifespark.WriteDNA(capabilitiesBytes));

            string breakcodon = DNA.codons.Last().Item1;
            string triplebreak = string.Concat(Enumerable.Repeat(breakcodon, 3));

            string identityDNA = string.Concat(identity.Select(n => n.Item1)) + triplebreak;
            string cognitiveprocessDNA = string.Concat(cognitiveprocess.Select(n => n.Item1));
            //string capabilitiesDNA = string.Concat(capabilities.Select(n => n.Item1));

            string _genome = identityDNA + cognitiveprocessDNA;
            BifrostAI bifrostAI = new BifrostAI(_genome);
            await bifrostAI.StartAsync();

        }
    }
}