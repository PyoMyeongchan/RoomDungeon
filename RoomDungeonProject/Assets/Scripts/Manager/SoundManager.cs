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

    // 게임 시작 시 자동으로 실행되는 초기화 함수
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    public static void InitSoundManager()
    {
        GameObject obj = new GameObject("SoundManager");
        instance = obj.AddComponent<SoundManager>();
        DontDestroyOnLoad(obj);

        // BGM 설정
        GameObject bgmObj = new GameObject("BGM");
        SoundManager.instance.bgmSource = bgmObj.AddComponent<AudioSource>();
        bgmObj.transform.SetParent(obj.transform);
        SoundManager.instance.bgmSource.loop = true;
        SoundManager.instance.bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1.0f); // 1.0f는 디폴트 값

        // SFX 설정
        GameObject sfxObj = new GameObject("SFX");
        SoundManager.instance.sfxSource = sfxObj.AddComponent<AudioSource>();
        SoundManager.instance.sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        sfxObj.transform.SetParent(obj.transform);

        // Resources 파일에서 불러오기
        AudioClip[] bgmClips = Resources.LoadAll<AudioClip>("Sound/BGM");

        // 예외처리 (매니저 만들때는 예외처리를 다해야한다!)
        foreach (var clip in bgmClips)
        {
            try
            {
                BGMType type = (BGMType)Enum.Parse(typeof(BGMType), clip.name);
                SoundManager.instance.bgmDic.Add(type, clip);
            }
            catch
            {
                Debug.LogWarning("BGM enum 필요" + clip.name);
            }

        }

        AudioClip[] sfxClips = Resources.LoadAll<AudioClip>("Sound/SFX");

        // 예외처리
        foreach (var clip in sfxClips)
        {
            try
            {
                SFXType type = (SFXType)Enum.Parse(typeof(SFXType), clip.name);
                SoundManager.instance.sfxDic.Add(type, clip);
            }
            catch
            {
                Debug.LogWarning("SFX enum 필요" + clip.name);
            }
        }
        // 씬 로드시마다 OnSceneLoadCompleted 호출
        // += 이벤트 연결
        SceneManager.sceneLoaded += SoundManager.instance.OnSceneLoadCompleted;

    }

    // 씬 전환 완료 시 자동 호출되는 함수
    public void OnSceneLoadCompleted(Scene scene, LoadSceneMode mode)
    {
        // 씬말고 맵내에서 바꾸는 것을 활용해서 만들어보자
        // 이건 씬 바꾸면 재생되는 코드
        if (scene.name == "Main")
        {
            PlayBGM(BGMType.MainBGM, 1f);
        }
        else if(scene.name == "Map1") 
        {
            PlayBGM(BGMType.Tutorial_Forest, 1f);
        }
    }

    // 효과음 재생
    public void PlaySFX(SFXType type)
    {
        sfxSource.PlayOneShot(sfxDic[type]);
    }

    // 배경음 재생 함수(페이드효과 포함)
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

    //BGM 볼륨을 천천히 줄이는 코루틴
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
        //페이드 아웃 후 콜백 실행
        // onComplete가 null일 수 있다.
        onComplete?.Invoke();
    }

    // BGM 볼륨을 천천히 올리는 코루틴
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

    // BGM 볼륨 설정
    public void SetBGMVolume(float volume)
    { 
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    // SFX 볼륨 설정
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
