using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private string soundToPlay;
    [SerializeField] private bool playOnStart;
    [SerializeField] private bool playOnTrigger;
    [SerializeField] private string[] targetTags;

    private void Start()
    {
        if (playOnStart)
        {
            AudioManager.Instance.PlaySound(soundToPlay);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playOnTrigger) return;

        if (targetTags.Length == 0 || System.Array.Exists(targetTags, tag => other.CompareTag(tag)))
        {
            AudioManager.Instance.PlaySound(soundToPlay);
        }
    }

    public void PlaySound()
    {
        AudioManager.Instance.PlaySound(soundToPlay);
    }
} 