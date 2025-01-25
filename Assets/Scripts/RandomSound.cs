using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;

    private AudioClip GetClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    public void PlayRandom(float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(GetClip(), transform.position, volume);
    }
}
