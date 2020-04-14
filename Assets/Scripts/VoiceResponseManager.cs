using UnityEngine;

public class VoiceResponseManager : MonoBehaviour
{
    AudioSource audS;
    public AudioClip shutterSfx;
    public AudioClip successSfx;
    [Tooltip("make sure it is in decending order")]
    public AudioClip[] underpickSfx;
    [Tooltip("make sure it is in ascending order")]
    public AudioClip[] overpickSfx;

    private void Start()
    {
        audS = GetComponent<AudioSource>();
    }

    public void PlayCameraShutter()
    {
        audS.PlayOneShot(shutterSfx);
    }

    public void PlaySuccess()
    {
        audS.PlayOneShot(successSfx);
    }

    /// <summary>
    ///     Play the voice from the array depends on the number that the user speaks
    ///     (calculate from VoiceProcessor.cs - pickItem)
    /// </summary>
    /// <param name="i">number of the difference that the user have to pick/return</param>
    public void PlayIncorrectQuantityPick(int i)
    {
        if(i > 0)
        {
            audS.PlayOneShot(overpickSfx[i - 1]);
        }
        else
        {
            audS.PlayOneShot(underpickSfx[-i - 1]);
        }
    }
}
