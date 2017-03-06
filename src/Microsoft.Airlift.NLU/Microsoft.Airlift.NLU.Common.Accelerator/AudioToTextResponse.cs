using Microsoft.Airlift.NLU.Common.Accelerator.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Airlift.NLU.Common.Accelerator
{
    public sealed class AudioToTextResponse
    {
        public AudioToTextResponse(string httpResponse)
        {
            this.RawResponse = httpResponse;
            this.RawResponseParsed = Newtonsoft.Json.JsonConvert.DeserializeObject<BingSpeechAudioToTextResponse>(httpResponse);
            this.IsError = RawResponseParsed.header.status != "success";
            if (!this.IsError && this.RawResponseParsed != null && this.RawResponseParsed.results != null && this.RawResponseParsed.results.Count > 0)
            {
                var result = this.RawResponseParsed.results.FirstOrDefault();
                this.SpokenText = ReplaceNumbers(result.lexical);
            }
        }

        public bool IsError { get; private set; }
        internal BingSpeechAudioToTextResponse RawResponseParsed { get; private set; }
        public string RawResponse { get; private set; }
        public string SpokenText { get; private set; }

        private string ReplaceNumbers(string responseString)
        {
            return responseString.Replace(" eighteen ", " 18 ").Replace("thirty one", " 31 ");
        }
    }
}
