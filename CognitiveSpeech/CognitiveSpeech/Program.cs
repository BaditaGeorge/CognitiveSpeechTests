using System;
using System.Threading.Tasks;

namespace CognitiveSpeech
{
    class Program
    {
        static async Task Main(string[] args)
        {
            SpeechTest st = new SpeechTest();
            await st.continuosRecognition();
        }
    }
}
