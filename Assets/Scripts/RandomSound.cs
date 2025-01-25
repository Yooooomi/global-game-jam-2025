using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;

    private AudioClip GetClip() {
        return clips[Random.Range(0, clips.Length)];
    }

    public void PlayRandom() {
        AudioSource.PlayClipAtPoint(GetClip(), transform.position);
    }
}
