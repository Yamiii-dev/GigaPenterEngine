using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace GigaPenter.Audio.NAudio;

public class AudioPlayer
{
    private readonly IWavePlayer outputDevice;
    private readonly MixingSampleProvider mixer;

    public AudioPlayer()
    {
        outputDevice = new WaveOutEvent();
        mixer = new  MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(48000, 4));
        mixer.ReadFully = true;
        outputDevice.Init(mixer);
        outputDevice.Play();
    }

    public void PlaySound(AudioFile audio)
    {
        AddMixerInput(new CachedSoundSampleProvider(audio));
    }
    
    private ISampleProvider ConvertToRightChannelCount(ISampleProvider input)
    {
        if (input.WaveFormat.Channels == mixer.WaveFormat.Channels)
        {
            return input;
        }
        if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2)
        {
            return new MonoToStereoSampleProvider(input);
        }
        throw new NotImplementedException("Not yet implemented this channel count conversion");
    }
    private void AddMixerInput(ISampleProvider input)
    {
        mixer.AddMixerInput(ConvertToRightChannelCount(input));
    }
}