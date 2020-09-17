using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace CognitiveSpeech
{
    class SpeechTest
    {
        public async Task continuosRecognition()
        {
            var speechConfig = SpeechConfig.FromSubscription("myKey", "northeurope");
            using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            speechConfig.SpeechRecognitionLanguage = "ro-RO";
            speechConfig.EnableDictation();
            using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
            var stopRecognition = new TaskCompletionSource<int>();
            Console.OutputEncoding = Encoding.UTF8;
            recognizer.Recognizing += (s, e) =>
            {
                Console.WriteLine("Text=" + e.Result.Text);
            };
            recognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    Console.WriteLine("Final Text=" + e.Result.Text);
                }
                else
                {
                    Console.WriteLine("Speech not found!");
                }
            };
            recognizer.Canceled += (s, e) =>
            {
                Console.WriteLine("Reason=" + e.Reason);
            };
            recognizer.SessionStopped += (s, e) =>
            {
                Console.WriteLine("\n Session Stopped!");
                stopRecognition.TrySetResult(0);
            };
            await recognizer.StartContinuousRecognitionAsync();
            Task.WaitAny(new[] { stopRecognition.Task });
            await recognizer.StopContinuousRecognitionAsync();
        }
    }
}
