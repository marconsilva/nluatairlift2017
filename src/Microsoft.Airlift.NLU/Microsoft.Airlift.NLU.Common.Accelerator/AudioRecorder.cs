using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Audio;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Media.Render;
using Windows.Storage;

namespace Microsoft.Airlift.NLU.Common.Accelerator
{
    class AudioRecorder
    {
        public async Task StartRecordToFileAsync(StorageFile file)
        {
            var result = await AudioGraph.CreateAsync(
              new AudioGraphSettings(AudioRenderCategory.Media));

            if (result.Status == AudioGraphCreationStatus.Success)
            {
                this.graph = result.Graph;

                var microphone = await DeviceInformation.CreateFromIdAsync(
                  MediaDevice.GetDefaultAudioCaptureId(AudioDeviceRole.Default));

                // Low gives us 1 channel, 16-bits per sample, 16K sample rate.
                var outProfile = MediaEncodingProfile.CreateWav(AudioEncodingQuality.Low);
                outProfile.Audio = AudioEncodingProperties.CreatePcm(16000, 1, 16);
                //outProfile.Audio = AudioEncodingProperties.CreatePcm(44100, 2, 32);

#if ARM_HACK
                var inProfile = MediaEncodingProfile.CreateWav(AudioEncodingQuality.Low);
                inProfile.Audio = AudioEncodingProperties.CreatePcm(44100, 2, 32);
#else
                var inProfile = MediaEncodingProfile.CreateWav(AudioEncodingQuality.High);

#endif
                //inProfile.Audio = AudioEncodingProperties.CreatePcm(16000, 1, 16);

                var outputResult = await this.graph.CreateFileOutputNodeAsync(file,
                  outProfile);

                if (outputResult.Status == AudioFileNodeCreationStatus.Success)
                {
                    this.outputNode = outputResult.FileOutputNode;

                    var inputResult = await this.graph.CreateDeviceInputNodeAsync(
                      MediaCategory.Media,
                      inProfile.Audio,
                      microphone);

                    if (inputResult.Status == AudioDeviceNodeCreationStatus.Success)
                    {
                        try
                        {
                            inputResult.DeviceInputNode.AddOutgoingConnection(
                              this.outputNode);

                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                        this.graph.Start();
                    }
                }
            }
            else
            {
                throw new Exception("Could not create AudioGraph");
            }
        }
        public async Task StopRecordAsync()
        {
            if (this.graph != null)
            {
                this.graph?.Stop();

                await this.outputNode.FinalizeAsync();

                // assuming that disposing the graph gets rid of the input/output nodes?
                this.graph?.Dispose();

                this.graph = null;
            }
        }
        AudioGraph graph;
        AudioFileOutputNode outputNode;
    }
}
