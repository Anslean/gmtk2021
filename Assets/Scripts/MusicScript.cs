using UnityEngine;

// Ultra-simple music script!
public class MusicScript : MonoBehaviour
{
    public AudioClip songToPlay;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource music = gameObject.AddComponent<AudioSource>();
        music.volume = 0.7f;
        music.loop = true;
        music.spatialBlend = 0.0f;
        music.clip = songToPlay;
        music.Play();
    }
}
