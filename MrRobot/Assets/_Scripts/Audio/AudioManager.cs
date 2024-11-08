using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } 

    [SerializeField] private AudioSource[] sfx; // Tablica SFX
    [SerializeField] private AudioSource[] bgm; // Tablica muzyki

    public bool playBgm; 
    private int bgmIndex;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        playBgm = true;
        PlayRandomBGM();
        PlaySFX(0); // Odtwarzaj SFX z indeksu 0 lub odpowiedni
    }

    private void Update()
    {
        if (!playBgm)
        {
            StopAllBGM();
        }
        else
        {
            if (bgmIndex < bgm.Length && !bgm[bgmIndex].isPlaying)
            {
                PlayNextBGM(); 
            }
        }
    }

    public void PlaySFX(int _sfxIndex)
    {
        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].Play();
        }
    }

  

    public void StopSFX(int _index) 
    {
        if (_index < sfx.Length)
        {
            sfx[_index].Stop();
        }
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;
        StopAllBGM();
        bgm[bgmIndex].Play();
    }

    public void PlayNextBGM()
    {
        bgmIndex++; 
        if (bgmIndex >= bgm.Length)
        {
            bgmIndex = 0;
        }
        PlayBGM(bgmIndex); 
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}