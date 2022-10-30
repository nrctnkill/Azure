using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
namespace Speech_To_Text
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*await RecognizeSpeechToText();
            Console.WriteLine("Finished!");*/
            ContinuousSpeechRecignitionAsync().Wait();
            Console.ReadKey();
        }
        /// <summary>
        /// One line from mic
        /// </summary>
        /// <returns></returns>
        static async Task RecognizeSpeechToText()
        {
            var subscriptionKey = "d0f7d8a59e7442dab63838848630f8fc";//Azure Cognitive Service(Speech api key)
            var region = "westus";
            var configuration = SpeechConfig.FromSubscription(subscriptionKey, region);

            using (var recog=new SpeechRecognizer(configuration))
            {
                Console.WriteLine("Speak Something...");
                var result = await recog.RecognizeOnceAsync();
                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine(result.Text);
                }
            }
        }
        /// <summary>
        /// Continuously Recognize speech from Mic
        /// </summary>
        /// <returns></returns>
        private static async Task ContinuousSpeechRecignitionAsync()
        {
            var subscriptionKey = "d0f7d8a59e7442dab63838848630f8fc";//Generate from Azure portal
            var region = "westus";

            var configuration = SpeechConfig.FromSubscription(subscriptionKey, region);

            using (var recognizer=new SpeechRecognizer(configuration))
            {
                recognizer.Recognizing += (sender, eventArgs) => { Console.WriteLine($"recognizing:{eventArgs.Result.Text}"); };
                recognizer.Recognized += (sender, eventArgs) =>
                 {
                     var result = eventArgs.Result;
                     if (result.Reason == ResultReason.RecognizedSpeech)
                     {
                         Console.WriteLine($"Final Statement:{result.Text}");
                     }
                 };

                recognizer.SessionStarted += (sender, eventArgs) => { Console.WriteLine($"\n Session has Started.You can start speaking..."); };
                recognizer.SessionStopped += (sender, eventArgs) => { Console.WriteLine($"\n Session stopped..."); };

                await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

                do
                {
                    Console.WriteLine("Press Enter to Stop");
                } while (Console.ReadKey().Key !=ConsoleKey.Enter );

                await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            }
        }

        
    }
}
