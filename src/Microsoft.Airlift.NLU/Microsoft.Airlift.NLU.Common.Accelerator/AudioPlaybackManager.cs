using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Airlift.NLU.Common.Accelerator
{
    public sealed class AudioPlaybackManager
    {
        private MediaElement mediaElement = null;
        private bool isPlaying = false;

        public AudioPlaybackManager(MediaElement mediaElement)
        {
            this.mediaElement = mediaElement;
            this.mediaElement.AutoPlay = true;
            this.mediaElement.MediaEnded += MediaElement_MediaEnded;
        }

        private void MediaElement_MediaEnded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            isPlaying = false;
        }

        public async Task PlayBeep()
        {
            var fileStream = await (await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/beep.wav"))).OpenStreamForReadAsync();
            mediaElement.SetSource(fileStream.AsRandomAccessStream(), "WAV");
            mediaElement.Play();
        }

        public async Task PlayAudio(Stream audio)
        {
            if(isPlaying)
                await Task.Delay((int)(mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds - mediaElement.Position.TotalMilliseconds));

            isPlaying = true;
            mediaElement.SetSource(audio.AsRandomAccessStream(), "WAV");
            mediaElement.Play();
            await Task.Delay(1000);

        }

        public async Task PlayMusic001()
        {
            var fileStream = await(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/bornInTheUSA.mp3"))).OpenStreamForReadAsync();
            mediaElement.SetSource(fileStream.AsRandomAccessStream(), "MP3");
            mediaElement.Play();
        }

        public async Task PlayMusic002()
        {
            var fileStream = await (await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/eyeOfTheTiger.mp3"))).OpenStreamForReadAsync();
            mediaElement.SetSource(fileStream.AsRandomAccessStream(), "MP3");
            mediaElement.Play();
        }
    }
}
