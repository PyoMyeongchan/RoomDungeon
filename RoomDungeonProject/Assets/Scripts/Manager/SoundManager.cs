using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Collections;

public enum BGMType
{ 
    MainBGM,
    Tutorial_Forest
}

public enum SFXType
{ 
    Attack1Sound,
    Damaged,
    EquipSound,
    JumpSound,
    RollSound,
    StepSound

}


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public Dictionary<BGMType, AudioClip> bgmDic = new Dictionary<BGMType, AudioClip>();
    public Dictionary<SFXType, AudioClip> sfxDic = new Dictionary<SFXType, AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // ���� ���� �� �ڵ����� ����Ǵ� �ʱ�ȭ �Լ�
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    public static void InitSoundManager()
    {
        GameObject obj = new GameObject("SoundManager");
        instance = obj.AddComponent<SoundManager>();
        DontDestroyOnLoad(obj);

        // BGM ����
        GameObject bgmObj = new GameObject("BGM");
        SoundManager.instance.bgmSource = bgmObj.AddComponent<AudioSource>();
        bgmObj.transform.SetParent(obj.transform);
        SoundManager.instance.bgmSource.loop = true;
        SoundManager.instance.bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1.0f); // 1.0f�� ����Ʈ ��

        // SFX ����
        GameObject sfxObj = new GameObject("SFX");
        SoundManager.instance.sfxSource = sfxObj.AddComponent<AudioSource>();
        SoundManager.instance.sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        sfxObj.transform.SetParent(obj.transform);

        // Resources ���Ͽ��� �ҷ�����
        AudioClip[] bgmClips = Resources.LoadAll<AudioClip>("Sound/BGM");

        // ����ó�� (�Ŵ��� ���鶧�� ����ó���� ���ؾ��Ѵ�!)
        foreach (var clip in bgmClips)
        {
            try
            {
                BGMType type = (BGMType)Enum.Parse(typeof(BGMType), clip.name);
                SoundManager.instance.bgmDic.Add(type, clip);
            }
            catch
            {
                Debug.LogWarning("BGM enum �ʿ�" + clip.name);
            }

        }

        AudioClip[] sfxClips = Resources.LoadAll<AudioClip>("Sound/SFX");

        // ����ó��
        foreach (var clip in sfxClips)
        {
            try
            {
                SFXType type = (SFXType)Enum.Parse(typeof(SFXType), clip.name);
                SoundManager.instance.sfxDic.Add(type, clip);
            }
            catch
            {
                Debug.LogWarning("SFX enum �ʿ�" + clip.name);
            }
        }
        // �� �ε�ø��� OnSceneLoadCompleted ȣ��
        // += �̺�Ʈ ����
        SceneManager.sceneLoaded += SoundManager.instance.OnSceneLoadCompleted;

    }

    // �� ��ȯ �Ϸ� �� �ڵ� ȣ��Ǵ� �Լ�
    public void OnSceneLoadCompleted(Scene scene, LoadSceneMode mode)
    {
        // ������ �ʳ����� �ٲٴ� ���� Ȱ���ؼ� ������
        // �̰� �� �ٲٸ� ����Ǵ� �ڵ�
        if (scene.name == "Main")
        {
            PlayBGM(BGMType.MainBGM, 1f);
        }
        else if(scene.name == "Map1") 
        {
            PlayBGM(BGMType.Tutorial_Forest, 1f);
        }
    }

    // ȿ���� ���
    public void PlaySFX(SFXType type)
    {
        sfxSource.PlayOneShot(sfxDic[type]);
    }

    // ����� ��� �Լ�(���̵�ȿ�� ����)
    public void PlayBGM(BGMType type, float fadeTime = 0)
    {
        if (bgmSource.clip != null)
        {
            if (bgmSource.clip.name == type.ToString())
            {
                return;
            }

            if (fadeTime == 0)
            {
                bgmSource.clip = bgmDic[type];
                bgmSource.Play();
            }
            else
            {
                StartCoroutine(FadeOutBGM(() =>
                {
                    bgmSource.clip = bgmDic[type];
                    bgmSource.Play();
                    StartCoroutine(FadeInBGM(fadeTime));
                },fadeTime));
            }

        }
        else
        {
            if (fadeTime == 0)
            {
                bgmSource.clip = bgmDic[type];
                bgmSource.Play();
            }
            else
            {
                bgmSource.volume = 0;
                bgmSource.clip = bgmDic[type];
                bgmSource.Play();
                StartCoroutine(FadeInBGM(fadeTime));
            }

        }
        
    }

    //BGM ������ õõ�� ���̴� �ڷ�ƾ
    private IEnumerator FadeOutBGM(Action onComplete, float duration)
    { 
        float startVolume = bgmSource.volume;
        float time = 0;

        while (time < duration)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0F, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        bgmSource.volume = 0f;
        //���̵� �ƿ� �� �ݹ� ����
        // onComplete�� null�� �� �ִ�.
        onComplete?.Invoke();
    }

    // BGM ������ õõ�� �ø��� �ڷ�ƾ
    private IEnumerator FadeInBGM(float duration = 1.0f)
    {
        float targetVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        float time = 0f;

        while (time < duration)
        {
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }

    // BGM ���� ����
    public void SetBGMVolume(float volume)
    { 
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    // SFX ���� ����
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
