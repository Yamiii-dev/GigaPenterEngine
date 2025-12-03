using GigaPenterEngine.Core;
using MiniAudioEx.Core.StandardAPI;

namespace GigaPenterEngine.Audio.MiniAudio.Audio;

public class AudioPlayer : GameSystem
{
    private uint SAMPLE_RATE = 44100;
    private uint CHANNELS = 2;

    public AudioPlayer()
    {
        AudioContext.Initialize(SAMPLE_RATE, CHANNELS);
    }

    public override void Update()
    {
        base.Update();
        AudioContext.Update();
    }

    public void PlayClip(AudioSource audioSource, AudioFile clip, bool loop = false)
    {
        audioSource.source.PlayOneShot(clip.clip);
        audioSource.source.Loop = loop;
    }
    
}