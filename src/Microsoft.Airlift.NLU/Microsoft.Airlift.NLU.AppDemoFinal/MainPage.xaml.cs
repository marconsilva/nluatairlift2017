using Microsoft.Airlift.NLU.Common.Accelerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Cognitive.LUIS;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Microsoft.Airlift.NLU.AppDemoFinal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private GpioManager gpioManager;
        private BingSpeechManager bingSpeechManager;
        private AudioPlaybackManager audioPlaybackManager;
        private OutputMessagesManager outputMessagesManager;
        private LuisManager luisManager;
        private WeatherServiceManager weatherServiceManager;

        private bool isCapturing = false;

        public MainPage()
        {
            this.InitializeComponent();
            audioPlaybackManager = new AudioPlaybackManager(AudioPlayer);

            ////PT
            //outputMessagesManager = new OutputMessagesManager(OutputMessagesManagerLanguage.pt_PT);
            //bingSpeechManager = new BingSpeechManager("d70ff94024944732a8c68f45c1f3c2f9", BingSpeechManagerLanguage.pt_PT);
            //luisManager = new LuisManager("1cb00497-bcb0-4ca0-9a88-44069a82a3e7", "550dc6979d514e09aa089e3996d204b4");

            //EN
            outputMessagesManager = new OutputMessagesManager(OutputMessagesManagerLanguage.en_US);
            bingSpeechManager = new BingSpeechManager("d70ff94024944732a8c68f45c1f3c2f9", BingSpeechManagerLanguage.en_US);
            luisManager = new LuisManager("c6f3d045-ce31-421b-adfa-b839d4491836", "550dc6979d514e09aa089e3996d204b4");

            weatherServiceManager = new WeatherServiceManager("3b0224d98c41c7db245366776dd206c3");

            gpioManager = new GpioManager();
            gpioManager.InitGpio();

            gpioManager.buttonPressed += GpioManager_buttonPressed;
            gpioManager.buttonReleased += GpioManager_buttonReleased;
        }
        

        private async Task StartCapture()
        {
            await bingSpeechManager.StartCapture();
            await audioPlaybackManager.PlayBeep();
        }

        private async Task StopCapture()
        {
            gpioManager.TurnLedOn();
            var textToSpeachResult = await bingSpeechManager.EndCapture();

            if (textToSpeachResult == null || textToSpeachResult.IsError)
            {
                await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetUnknownIntentOutput()));
            }
            else
            {
                await ProcessIntent(await luisManager.GetIntent(textToSpeachResult.SpokenText));
            }
           gpioManager.TurnLedOff();
        }

        private async Task ProcessIntent(LuisResult luisResult)
        {
            switch (luisResult.TopScoringIntent.Name.ToLowerInvariant())
            {
                case "lightsoff":
                    gpioManager.TurnLightOff();
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetLightsOutIntentOutput()));
                    break;
                case "lightson":
                    gpioManager.TurnLightOn();
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetLightsOnIntentOutput()));
                    break;
                case "playmusic":
                    if (!luisResult.Entities.ContainsKey("SongName"))
                        await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetMusicNameNotFoundMessageOutput()));
                    else
                    {
                        switch (luisResult.Entities["SongName"].FirstOrDefault().Value.ToLowerInvariant())
                        {
                            case "born in the u":
                            case "born in the usa":
                                await audioPlaybackManager.PlayMusic001();
                                break;
                            case "eye of the tiger":
                                await audioPlaybackManager.PlayMusic002();
                                break;
                            default:
                                await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetMusicNotFoundMessageOutput()));
                                break;
                        }
                    }
                    break;
                case "weatherforecast":
                    var forecast = await weatherServiceManager.GetCurrentWeather();
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetForecastTypeMessage(forecast.Forecast)));
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetForecastMinTemperatureMessage(forecast.MinTemperature)));
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetForecastMaxTemperatureMessage(forecast.MaxTemperature)));
                    break;
                case "myage":
                    if(!luisResult.Entities.ContainsKey("builtin.age"))
                        await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetAgeValueMissingMessage()));
                    else
                        await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetAgeMessage(luisResult.Entities["builtin.age"].FirstOrDefault().Value)));
                        break;
                case "whois":
                    foreach (var entityKeyMatch in luisResult.Entities)
                    {
                        foreach (var entity in entityKeyMatch.Value)
                        {
                            await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetWhoIsMessage(entityKeyMatch.Key, entity.Value)));
                        }
                    }
                    break;
                case "none":
                default:
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetUnknownIntentOutput()));
                    break;
            }
        }

        private async void GpioManager_buttonPressed(object sender, EventArgs e)
        {
            await StartCapture();
        }

        private async void GpioManager_buttonReleased(object sender, EventArgs e)
        {
            await StopCapture();
        }

        private async void capturebutton_Click(object sender, RoutedEventArgs e)
        {
            isCapturing = !isCapturing;
            if(isCapturing)
                await StartCapture();
            else
                await StopCapture();
        }
    }
}
