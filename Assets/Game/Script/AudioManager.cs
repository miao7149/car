using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //单例
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject obj = new()
                    {
                        name = "AudioManager"
                    };
                    instance = obj.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }
    //按钮点击音效
    public AudioClip buttonClickClip;
    //小汽车行驶音效
    public AudioClip carSmallMoveClip;
    //汽车撞击音效
    public AudioClip carCrashClip;
    //卡车行驶音效
    public AudioClip carBigMoveClip;
    //行人被撞击音效
    public AudioClip pedestrianHitClip;
    //行人眩晕音效
    public AudioClip pedestrianDizzyClip;
    //气球充气音效
    public AudioClip balloonInflateClip;
    //气球飞行音效
    public AudioClip balloonFlyClip;
    //获得金币音效
    public AudioClip getCoinClip;
    //游戏开始音效
    public AudioClip gameStartClip;
    //胜利音效
    public AudioClip victoryClip;
    //失败音效
    public AudioClip failClip;
    //金币结算音效
    public AudioClip coinSettleClip;
    //背景音乐
    public AudioClip backgroundMusicClip;

    private AudioSource soundEffectsSource;
    private AudioSource backgroundMusicSource;

    void Start()
    {
        // 创建两个 AudioSource 组件
        soundEffectsSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();

        // 设置背景音乐的 AudioSource
        backgroundMusicSource.clip = backgroundMusicClip;
        backgroundMusicSource.loop = true; // 设置循环播放
        backgroundMusicSource.playOnAwake = true; // 启动时播放
        backgroundMusicSource.volume = 0.5f; // 设置音量

        // 播放背景音乐
        if (GlobalManager.Instance.IsSound)
            backgroundMusicSource.Play();
    }

    // 播放按钮点击音效
    public void PlayButtonClick()
    {
        if (GlobalManager.Instance.IsSound)
            soundEffectsSource.PlayOneShot(buttonClickClip);
    }
    // 播放小汽车行驶音效
    public void PlayCarSmallMove()
    {
        if (GlobalManager.Instance.IsSound)
        {
            soundEffectsSource.volume = 0.2f;
            soundEffectsSource.PlayOneShot(carSmallMoveClip);
        }

    }
    // 播放汽车撞击音效
    public void PlayCarCrash()
    {
        if (GlobalManager.Instance.IsSound)
        {
            soundEffectsSource.volume = 0.5f;
            soundEffectsSource.PlayOneShot(carCrashClip);
        }
    }
    // 播放卡车行驶音效
    public void PlayCarBigMove()
    {
        if (GlobalManager.Instance.IsSound)
        {
            soundEffectsSource.volume = 0.5f;
            soundEffectsSource.PlayOneShot(carBigMoveClip);
        }
    }
    // 播放行人被撞击音效
    public void PlayPedestrianHit()
    {
        if (GlobalManager.Instance.IsSound)
        {
            soundEffectsSource.volume = 0.5f;
            soundEffectsSource.PlayOneShot(pedestrianHitClip);
        }
    }
    // 播放行人眩晕音效
    public void PlayPedestrianDizzy()
    {
        if (GlobalManager.Instance.IsSound)
        {
            soundEffectsSource.volume = 0.5f;
            soundEffectsSource.PlayOneShot(pedestrianDizzyClip);
        }
    }
    // 播放气球充气音效
    public void PlayBalloonInflate()
    {
        if (GlobalManager.Instance.IsSound)
        {
            soundEffectsSource.volume = 0.5f;
            soundEffectsSource.PlayOneShot(balloonInflateClip);
        }
    }
    // 播放气球飞行音效
    public void PlayBalloonFly()
    {
        if (GlobalManager.Instance.IsSound)
        {
            soundEffectsSource.volume = 0.5f;
            soundEffectsSource.PlayOneShot(balloonFlyClip);
        }
    }
    // 播放获得金币音效
    public void PlayGetCoin()
    {
        if (GlobalManager.Instance.IsSound)
        {
            soundEffectsSource.volume = 0.5f;
            soundEffectsSource.PlayOneShot(getCoinClip);
        }
    }
    // 播放游戏开始音效
    public void PlayGameStart()
    {
        if (GlobalManager.Instance.IsSound)
        {
            soundEffectsSource.volume = 0.5f;
            soundEffectsSource.PlayOneShot(gameStartClip);
        }
    }
    // 播放胜利音效
    public void PlayVictory()
    {
        if (GlobalManager.Instance.IsSound)
        {
            soundEffectsSource.volume = 0.5f;
            soundEffectsSource.PlayOneShot(victoryClip);
        }
    }
    // 播放失败音效
    public void PlayFail()
    {
        if (GlobalManager.Instance.IsSound)
        {
            soundEffectsSource.volume = 0.5f;
            soundEffectsSource.PlayOneShot(failClip);
        }

    }
    // 播放金币结算音效
    public void PlayCoinSettle()
    {
        if (GlobalManager.Instance.IsSound)
        {
            soundEffectsSource.volume = 0.5f;
            soundEffectsSource.PlayOneShot(coinSettleClip);
        }

    }
    // 暂停背景音乐
    public void PauseBackgroundMusic()
    {
        if (GlobalManager.Instance.IsSound)
            backgroundMusicSource.Pause();
    }
    // 恢复背景音乐
    public void ResumeBackgroundMusic()
    {
        if (GlobalManager.Instance.IsSound)
            backgroundMusicSource.Play();
    }
    // 停止背景音乐
    public void StopBackgroundMusic()
    {
        if (GlobalManager.Instance.IsSound)
            backgroundMusicSource.Stop();
    }
    // 设置背景音乐音量
    public void SetBackgroundMusicVolume(float volume)
    {
        backgroundMusicSource.volume = volume;
    }
    // 设置音效音量
    public void SetSoundEffectsVolume(float volume)
    {
        soundEffectsSource.volume = volume;
    }
    // 判断音效是否正在播放
    public bool IsSoundEffectPlaying()
    {
        return soundEffectsSource.isPlaying;
    }
}
