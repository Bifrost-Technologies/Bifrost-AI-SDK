using Bifrost_AI_SDK.Plugins;
using Bifrost_AI_SDK.Types.Genetics;
using Lifespark.Genetics;
using LLama;
using LLama.Common;
using LLama.Native;
using LLama.Sampling;
using LLamaSharp.SemanticKernel;
using LLamaSharp.SemanticKernel.ChatCompletion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using AuthorRole = Microsoft.SemanticKernel.ChatCompletion.AuthorRole;
using ChatHistory = Microsoft.SemanticKernel.ChatCompletion.ChatHistory;
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

namespace Bifrost_AI_SDK
{
    public class BifrostAI
    {
        private string modelPath = Directory.GetCurrentDirectory() + @"\llama\models\Bitnet\bitnet2.gguf";
        private string llamalibrary = Directory.GetCurrentDirectory() + @"\llama\llama.dll";

        private Identity? Identity { get; set; }
        private CognitiveProcess? CognitiveProcess { get; set; }
        private Capabilities? Capabilities { get; set; }
        private ModelParams ModelConfig { get; set; }
        private InferenceParams InferenceConfig { get; set; }

        private bool living = true;
        public BifrostAI(string _genome)
        {
            NativeLibraryConfig.LLama.WithLibrary(llamalibrary);
            ModelConfig = new ModelParams(modelPath)
            {
                ContextSize = 4056, // The longest length of chat as memory
            };
            InferenceConfig = new InferenceParams()
            {
                // MaxTokens = 256, // No more than 256 tokens should appear in answer. Remove it if antiprompt is enough for control.
                AntiPrompts = new List<string> { "User:" }, // Stop generation once antiprompts appear.
                SamplingPipeline = new DefaultSamplingPipeline(),
            };

            Genome genome = new Genome(_genome);
            if (genome.Count > 0)
            {
                GeneticEncoder lifespark = new GeneticEncoder();
                int i = 0;
                foreach (var molecule in genome)
                {
                    if (i == 0)
                    {
                        string gene_code = string.Empty;
                        molecule.ForEach(nucleotide_pair => gene_code += nucleotide_pair.Item1);
                        byte[] encoded_genes = lifespark.ReadDNA(gene_code);
                        this.Identity = Identity.Deserialize(new BinaryReader(new MemoryStream(encoded_genes)));
                        Console.WriteLine("Identity: " + Identity.Name);
                    }
                    if (i == 1)
                    {
                        string gene_code = string.Empty;
                        molecule.ForEach(nucleotide_pair => gene_code += nucleotide_pair.Item1);
                        byte[] encoded_genes = lifespark.ReadDNA(gene_code);
                        this.CognitiveProcess = CognitiveProcess.Deserialize(new BinaryReader(new MemoryStream(encoded_genes)));
                        Console.WriteLine("Cognitive: " + CognitiveProcess.ChainOfThought);
                    }
                    if (i == 2)
                    {
                        string gene_code = string.Empty;
                        molecule.ForEach(nucleotide_pair => gene_code += nucleotide_pair.Item1);
                        byte[] encoded_genes = lifespark.ReadDNA(gene_code);
                        this.Capabilities = Capabilities.Deserialize(new BinaryReader(new MemoryStream(encoded_genes)));
                    }
                    i++;
                }
            }
            else
            {
                throw new ArgumentException("Genome cannot be null or empty.");
            }
        }

        public async Task StartAsync()
        {
            await Task.Run(() => Living());
        }
        private InteractiveExecutor GenerateNeuralNetwork()
        {
            using var model = LLamaWeights.LoadFromFile(ModelConfig);
            using var context = model.CreateContext(ModelConfig);
            return new InteractiveExecutor(context);
        }
        private StatelessExecutor GenerateStatelessNeuralNetwork()
        {
            using var model = LLamaWeights.LoadFromFile(ModelConfig);
            return new StatelessExecutor(model, ModelConfig);
        }
        private ChatHistory SyncInstinct(bool useNarrativePrompt = true)
        {
            string systemPrompt;
            var chatHistory = new ChatHistory();
            if (Identity == null || CognitiveProcess == null)
            {
                throw new ArgumentNullException("Identity, CognitiveProcess cannot be null.");
            }
            if (useNarrativePrompt)
            {
                string pronoun;
                switch (Identity.Persona.Gender)
                {
                    case Gender.Male:
                        pronoun = "he";
                        break;
                    case Gender.Female:
                        pronoun = "she";
                        break;
                    default:
                        pronoun = "it";
                        break;
                }
                string pronounCap = char.ToUpper(pronoun[0]) + pronoun.Substring(1);

                systemPrompt =
                    $"This is a dialog transcript where the User interacts with an assistant named {Identity.Name}. " +
                    $"{Identity.Name} works as a {Identity.Persona.Profession} and possesses qualities such as " +
                    $"{string.Join(", ", Identity.Persona.PersonalityDescriptors)}. " +
                    //$"{pronounCap} uses a clear chain-of-thought to analyze input: " +
                   // $"{CognitiveProcess.ChainOfThought} " +//and {pronoun} is skilled in the following areas: " +
                                                           //  $"{string.Join(", ", Capabilities.Skills)}. " +
                    $"{pronounCap} always responds immediately and precisely." + 
                    "If the user wants to know the time, then answer with the following: '[GET TIME]' after if you have replied with that command. use the tools data in the chat history to display it back to the user. Do not repeat yourself";
            }
            else
            {
                systemPrompt =
                    "Transcript of a dialog for in-context training:\n" +
                    "Identity:\n" +
                    $"  Name: {Identity.Name}\n" +
                    $"  Gender: {Identity.Persona.Gender}\n" +
                    $"  Profession: {Identity.Persona.Profession}\n" +
                    $"  Personality Descriptors: {string.Join(", ", Identity.Persona.PersonalityDescriptors)}\n\n" +
                    "Cognitive Process:\n" +
                    $"{CognitiveProcess.ChainOfThought}" +
                    // "Capabilities:\n";
                    //  $"{string.Join(", ", Capabilities.Skills)}\n\n" +
                    "Rules: Provide precise and immediate responses. "
                    + "If the user wants to know the time, then answer with the following: '[GET TIME]' after if you have replied with that command. use the tools data in the chat history to display it back to the user. Do not repeat yourself";
            }
            chatHistory.AddMessage(AuthorRole.System, systemPrompt);

            return chatHistory;
        }

        private async Task Living()
        {
            using var model = LLamaWeights.LoadFromFile(ModelConfig);
            using var context = model.CreateContext(ModelConfig);
            var executor = new StatelessExecutor(model, ModelConfig);
            var instinct = SyncInstinct();
            var execution_settings = new LLamaSharpPromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
                Temperature = 0,
            };
            var builder = Kernel.CreateBuilder();
            // builder.Services.AddKeyedSingleton<ISemanticTextMemory>("bifrost-memory", memory);   
            builder.Services.AddSingleton<IChatCompletionService>(new LLamaSharpChatCompletion(executor, execution_settings));
            builder.Plugins.AddFromType<TemporalAwareness>("TemporalAwareness");
            var kernel = builder.Build();

            var chatHistory = instinct;
            IChatCompletionService chat_service = kernel.GetRequiredService<IChatCompletionService>();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("The chat session has started.\nUser: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string userInput = Console.ReadLine() ?? "";
            chatHistory.AddMessage(AuthorRole.User, userInput);
            while (living)
            {
                var response = string.Empty;
                await foreach (var reply in chat_service.GetStreamingChatMessageContentsAsync(chatHistory, execution_settings, kernel: kernel))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(reply.Content!);
                    response += reply.Content!;
                }
                chatHistory.AddMessage(AuthorRole.Assistant, response);
                FunctionResult? fres = null;
                if (response.Contains("[GET TIME]"))
                {
                    fres = await kernel.InvokeAsync("TemporalAwareness", "GetCurrentTime");
                    chatHistory.AddMessage(AuthorRole.Tool, "Current Time: "+ fres.GetValue<string>()!);
                    await foreach (var reply in chat_service.GetStreamingChatMessageContentsAsync(chatHistory, execution_settings, kernel: kernel))
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(reply.Content!);
                        response += reply.Content!;
                    }
                    chatHistory = SyncInstinct();
                    Console.ForegroundColor = ConsoleColor.Green;
                    userInput = Console.ReadLine()!;
                    chatHistory.AddMessage(AuthorRole.User, userInput);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    userInput = Console.ReadLine()!;
                    chatHistory.AddMessage(AuthorRole.User, userInput);
                    Console.ForegroundColor = ConsoleColor.White;
                }
           
            }
        }

    }
}
