using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Airlift.NLU.Common.Accelerator.ServiceModels
{
    public sealed class BingSpeechAudioToTextResponse
    {
        public string version { get; set; }
        public BingSpeechAudioToTextResponseHeader header { get; set; }
        public List<BingSpeechAudioToTextResponseResult> results { get; set; }
    }


    public sealed class BingSpeechAudioToTextResponseResult
    {
        public string name { get; set; }
        public string lexical { get; set; }
        public double confidence { get; set; }
        public List<BingSpeechAudioToTextResponseResultToken> tokens { get; set; }

    }

    public sealed class BingSpeechAudioToTextResponseResultToken
    {
        public string token { get; set; }
        public string lexical { get; set; }
        public string pronunciation { get; set; }
    }

    public sealed class BingSpeechAudioToTextResponseHeader
    {
        public string status { get; set; }
        public string scenario { get; set; }
        public string name { get; set; }
        public string lexical { get; set; }
    }
}
