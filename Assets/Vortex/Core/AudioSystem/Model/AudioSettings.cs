namespace Vortex.Core.AudioSystem.Model
{
    public class AudioSettings
    {
        public float SoundVolume { get; internal set; } = 1;
        public float MusicVolume { get; internal set; } = 1;
        public bool SoundOn { get; internal set; } = true;
        public bool MusicOn { get; internal set; } = true;
    }
}