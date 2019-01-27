using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    static SFXController instance;
    public AudioClip placeBlock;
    public AudioClip playerDie;
    public AudioClip enemyDie;
    public AudioClip pickUp;
    public AudioClip walk;
    public AudioClip win;

    public enum SoundType
    {
        PlaceBlock,
        PlayerDie,
        EnemyDie,
        PickUp,
        Walk,
        Win
    }

    static public void Play(SoundType sfxType, float volume = 1)
    {
		if(instance != null)
			instance.PlaySound(sfxType, volume);
		else
		{
			instance = GameObject.Find("SFX").GetComponent<SFXController>();
			instance.PlaySound(sfxType, volume);
		}
    }

	void PlaySound(SoundType sfxType, float volume)
	{
		AudioClip playClip = null;

		switch(sfxType)
		{
			case SoundType.PlaceBlock:
				playClip = placeBlock;
				break;
				
			case SoundType.PlayerDie:
				playClip = playerDie;
				break;

			case SoundType.EnemyDie:
				playClip = enemyDie;
				break;

			case SoundType.PickUp:
				playClip = pickUp;
				break;

			case SoundType.Walk:
				playClip = walk;
				break;

			case SoundType.Win:
				playClip = win;
				break;
		}

		if(playClip != null)
			audioSource.PlayOneShot(playClip, volume);
	}

    AudioSource audioSource;

    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
}
