using UnityEngine;

public static class Storage
{
    #region Keys
    private const string keyAudioSourceEnabled = "audioSourceEnabled";
    #endregion
    #region Getter Methods
    public static bool GetAudioSourceEnabled()
    {
        return PlayerPrefs.GetInt(keyAudioSourceEnabled, 1) == 1;
    }
    #endregion
    #region Setter Methods
    public static void SetAudioSourceEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(keyAudioSourceEnabled, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }
    #endregion
}