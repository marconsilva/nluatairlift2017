using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Microsoft.Airlift.NLU.Common.Accelerator
{
    public sealed class BingSpeechManager
    {
        private StorageFile inputFile;
        private readonly string key;
        private readonly BingSpeechManagerLanguage language;
        private string accessToken = null;

        private string textToSpeachPTBodyFormat = "<speak version='1.0' xml:lang='pt-PT'><voice xml:lang='pt-PT' xml:gender='Female' name='Microsoft Server Speech Text to Speech Voice (pt-PT, HeliaRUS)'>{0}</voice></speak>";
        private string textToSpeachENBodyFormat = "<speak version='1.0' xml:lang='en-US'><voice xml:lang='en-US' xml:gender='Female' name='Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)'>{0}</voice></speak>";
        private string speechRecognitionPTUrl = "https://speech.platform.bing.com/recognize?scenarios=ulm&appid=D4D52672-91D7-4C74-8AD8-42B1D98141A5&locale=pt-PT&device.os=W10IoT&version=3.0&format=json&requestid=";
        private string speechRecognitionENUrl = "https://speech.platform.bing.com/recognize?scenarios=ulm&appid=D4D52672-91D7-4C74-8AD8-42B1D98141A5&locale=en-US&device.os=W10IoT&version=3.0&format=json&requestid=";
        AudioRecorder _audioRecorder;

        public BingSpeechManager(string key, BingSpeechManagerLanguage language)
        {
            this.key = key;
            this.language = language;
        }

        public async Task StartCapture()
        {
            _audioRecorder = new AudioRecorder();
            inputFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync($"{DateTime.Now.ToString("yyyyMMddHHmmss")}.wav");

            await _audioRecorder.StartRecordToFileAsync(inputFile);
        }

        public async Task<AudioToTextResponse> EndCapture()
        {
            await _audioRecorder.StopRecordAsync();

            if (accessToken == null)
                accessToken = await GetAccessToken();

            var inputFileStream = (await inputFile.OpenReadAsync());
            if (inputFileStream.Size == 0)
                return null;

            var fileStream = inputFileStream.GetInputStreamAt(0).AsStreamForRead();
            return await ProcessAudioToText(fileStream, accessToken);
        }

        public async Task<Stream> GetAudio(string inputText)
        {
            Stream audioStream = null;

            if (accessToken == null)
                accessToken = await GetAccessToken();

            audioStream = await ProcessTextToAudio(inputText, accessToken);

            return audioStream;
        }

        private async Task<Stream> ProcessTextToAudio(string inputText, string accessToken)
        {
            HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = false };

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://speech.platform.bing.com");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Add("X-Microsoft-OutputFormat", "riff-8khz-8bit-mono-mulaw");
                client.DefaultRequestHeaders.Add("User-Agent", "AirliftDemo");
                try
                {
                    var bodyContent = new StringContent(string.Format(GetTextToSpeachBodyFormat(), inputText));
                    bodyContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
                    bodyContent.Headers.ContentType.Parameters.Add(new System.Net.Http.Headers.NameValueHeaderValue("charset", "utf-8"));
                    var response = await client.PostAsync("https://speech.platform.bing.com/synthesize",
                        new StringContent(string.Format(GetTextToSpeachBodyFormat(), inputText)));

                    var audioPayload = await response.Content.ReadAsStreamAsync();
                    return audioPayload;

                }
                catch (Exception ex)
                {
                    accessToken = null;
                    return null;
                }
            }
        }

        private string GetTextToSpeachBodyFormat()
        {
            switch (language)
            {
                case BingSpeechManagerLanguage.pt_PT:
                    return textToSpeachPTBodyFormat;
                case BingSpeechManagerLanguage.en_US:
                default:
                    return textToSpeachENBodyFormat;
            }
        }

        private async Task<AudioToTextResponse> ProcessAudioToText(Stream fileStream, string accessToken)
        {
            HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = false };

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://speech.platform.bing.com");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                try
                {
                    var bodyContent = new StreamContent(fileStream);
                    bodyContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/wav");
                    bodyContent.Headers.ContentType.Parameters.Add(new System.Net.Http.Headers.NameValueHeaderValue("samplerate", "16000"));

                    var urlString = language == BingSpeechManagerLanguage.en_US ? speechRecognitionENUrl : speechRecognitionPTUrl;
                    var response = await client.PostAsync(urlString + Guid.NewGuid().ToString() + "&instanceid=1d4b6030-9099-11e0-91e4-0800200c9a66",
                        bodyContent);

                    return new AudioToTextResponse(await response.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    accessToken = null;
                    return null;
                }
            }
        }

        private async Task<string> GetAccessToken()
        {
            HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = false };
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://api.cognitive.microsoft.com");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "d70ff94024944732a8c68f45c1f3c2f9");
                try
                {
                    var response = await client.PostAsync("https://api.cognitive.microsoft.com/sts/v1.0/issueToken", null);


                    var responseString = await response.Content.ReadAsStringAsync();
                    if (responseString.StartsWith("{ \"statusCode\": 4"))
                        return null;
                    return responseString;
                }
                catch (Exception ex)
                {
                    accessToken = null;
                    return null;
                }
            }


        }
    }
}
