using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } 

    [SerializeField] private AudioSource[] sfx; // Tablica SFX
    [SerializeField] private AudioSource[] bgm; // Tablica muzyki

    public bool playBgm; // Czy graæ BGM?
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
        playBgm = true; // Ustaw playBgm na true, aby w³¹czyæ muzykê
        PlayRandomBGM(); // Losowo odtwarzaj muzykê
        PlaySFX(0); // Odtwarzaj SFX z indeksu 0 lub odpowiedni, w zale¿noœci od dostêpnych dŸwiêków
    }

    private void Update()
    {
        if (!playBgm)
        {
            StopAllBGM();
        }
        else
        {
            // Upewnij siê, ¿e bgmIndex nie przekracza zakresu
            if (bgmIndex < bgm.Length && !bgm[bgmIndex].isPlaying)
            {
                PlayNextBGM(); // Odtwarzaj nastêpny utwór
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
        bgmIndex++; // PrzechodŸ do nastêpnego utworu
        if (bgmIndex >= bgm.Length) // SprawdŸ, czy nie przekracza d³ugoœci
        {
            bgmIndex = 0; // Zresetuj do 0, aby odtwarzaæ od pocz¹tku
        }
        PlayBGM(bgmIndex); // Odtwarzaj nastêpny utwór
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}
