using Vortex.Core.AudioSystem.Model;

namespace Vortex.Unity.AudioSystem.Model
{
    public class Sound : SoundSample
    {
        public SoundClip GetData() => Sample as SoundClip;
    }
}