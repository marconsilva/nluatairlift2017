using Microsoft.Cognitive.LUIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Airlift.NLU.AppDemoFinal
{
    public sealed class LuisManager
    {
        LuisClient luisClient;

        public LuisManager(string apiId, string apiKey)
        {
            luisClient = new LuisClient(apiId, apiKey, true);
        }

        public async Task<LuisResult> GetIntent(string text)
        {
            return await luisClient.Predict(text);
        }
    }
}
