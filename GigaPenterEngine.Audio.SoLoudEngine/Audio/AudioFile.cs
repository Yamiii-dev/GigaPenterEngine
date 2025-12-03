using SoLoud;

namespace GigaPenterEngine.Audio.SoLoudEngine.Audio;

public class AudioFile
{
    private Wav file = new Wav();
    private Soloud engine = new Soloud();

    public float Volume = 1.0f;
    public float Pan = 0.0f;

    public AudioFile(string filePath)
    {
        file.load(filePath);
        engine.init();
    }

    public void Play(bool looping = false)
    {
        file.setLooping(looping ? 1 : 0);
        engine.play(file, Volume, Pan);
    }
}