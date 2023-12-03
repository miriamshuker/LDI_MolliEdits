
[System.Serializable]
public class MusicNote
{
    public MusicKey noteKey;
    public float duration = 1; //in quarter notes
    public int octave = 4; //C4

    public void PitchUp()
    {
        noteKey = GetHalfToneUp(noteKey);
    }
    public void PitchDown()
    {
        noteKey = GetHalfToneDown(noteKey);
    }
    public MusicKey GetHalfToneUp(MusicKey key)
    {
        switch (key)
        {
            case (MusicKey.C):
            case (MusicKey.D):
            case (MusicKey.F):
            case (MusicKey.G):
            case (MusicKey.A):
                return key + 1;
            default:
                return key;
        }
    }
    public MusicKey GetHalfToneDown(MusicKey key)
    {
        switch (key)
        {
            case (MusicKey.D):
            case (MusicKey.E):
            case (MusicKey.G):
            case (MusicKey.A):
            case (MusicKey.B):
                return key - 1;
            default:
                return key;
        }
    }
    public bool IsBlackKey()
    {
        return IsBlackKey(noteKey);
    }
    public static bool IsBlackKey(MusicKey key)
    {
        switch (key)
        {
            case (MusicKey.CD):
            case (MusicKey.DE):
            case (MusicKey.FG):
            case (MusicKey.GA):
            case (MusicKey.AB):
                return true;
            default:
                return false;
        }
    }
}
