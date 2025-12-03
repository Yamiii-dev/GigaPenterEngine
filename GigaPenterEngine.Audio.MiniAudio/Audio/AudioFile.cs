using MiniAudioEx.Core.StandardAPI;

namespace GigaPenterEngine.Audio.MiniAudio.Audio;

public class AudioFile
{
    public AudioClip clip;

    public AudioFile(string filePath)
    {
        clip = new AudioClip(filePath);
    }
}