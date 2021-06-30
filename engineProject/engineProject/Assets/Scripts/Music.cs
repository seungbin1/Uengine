using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public Slider mainbackVolume;
    public AudioSource mainaudio;
    private float mainbackVol = 1f;

    public Slider gamebackVolume;
    public AudioSource gameaudio;
    private float gamebackVol = 1f;

    public Slider bossbackVolume;
    public AudioSource bossaudio;
    private float bossbackVol = 1f;

    private float boss;
    // Start is called before the first frame update
    void Start()
    {
        boss = 1f;
        mainbackVol = PlayerPrefs.GetFloat("mainbackvol", 1f);
        mainbackVolume.value = mainbackVol;
        mainaudio.volume = mainbackVolume.value;

        gamebackVol = PlayerPrefs.GetFloat("gamebackvol", 1f);
        gamebackVolume.value = gamebackVol;
        gameaudio.volume = gamebackVolume.value;

        bossbackVol = PlayerPrefs.GetFloat("bossbackvol", 1f);
        bossbackVolume.value = bossbackVol;
        bossaudio.volume = bossbackVolume.value;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.parent.transform.position.x>450)
        {
            bossaudio.mute = false;
            boss = 0.6f;
        }
        else
        {
            boss = 1f;
            bossaudio.mute = true;
        }
        SoundSlider();
    }
    public void SoundSlider()
    {
        gameaudio.volume = gamebackVolume.value* boss;
        gamebackVol = gamebackVolume.value;
        PlayerPrefs.SetFloat("gamebackvol", gamebackVol);

        mainaudio.volume = mainbackVolume.value;
        mainbackVol = mainbackVolume.value;
        PlayerPrefs.SetFloat("mainbackvol", mainbackVol);

        bossaudio.volume = bossbackVolume.value;
        bossbackVol = bossbackVolume.value;
        PlayerPrefs.SetFloat("bossbackvol", bossbackVol);
    }
}
