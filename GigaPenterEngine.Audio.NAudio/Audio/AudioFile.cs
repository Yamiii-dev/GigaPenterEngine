using NAudio.Wave;

namespace GigaPenterEngine.Audio.NAudio;

public class AudioFile
{
    public float[] AudioData { get; private set; }
    public WaveFormat WaveFormat { get; private set; }
    public AudioFile(string filePath)
    {
        using (var audioFileReader = new AudioFileReader(filePath))
        {
            WaveFormat = audioFileReader.WaveFormat;
            var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
            var readBuffer = new float[audioFileReader.WaveFormat.SampleRate *  audioFileReader.WaveFormat.Channels];
            int samplesRead;
            while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
            {
                wholeFile.AddRange(readBuffer.Take(samplesRead));
            }
            AudioData = wholeFile.ToArray();
        }
    }
}

internal class CachedSoundSampleProvider : ISampleProvider
{
    private readonly AudioFile file;
    private long position;

    public CachedSoundSampleProvider(AudioFile file)
    {
        this.file = file;
    }

    public int Read(float[] buffer, int offset, int count)
    {
        var availableSamples = file.AudioData.Length - position;
        var samplesToCopy = Math.Min(availableSamples, count);
        Array.Copy(file.AudioData, position, buffer, offset, samplesToCopy);
        position += samplesToCopy;
        return (int)samplesToCopy;
    }
    
    public WaveFormat WaveFormat => file.WaveFormat;
}