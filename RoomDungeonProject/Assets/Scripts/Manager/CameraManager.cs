using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    
    private CinemachineBasicMultiChannelPerlin mPerlin;
    


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {

            Destroy(gameObject);

        }
    }

    private void Start()
    {
        mPerlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
        mPerlin.AmplitudeGain = 0.0f;
        mPerlin.FrequencyGain = 0.0f;
    }



    public IEnumerator Shake()
    {
        mPerlin.AmplitudeGain = 0.7f;
        mPerlin.FrequencyGain = 0.7f;

        yield return new WaitForSeconds(0.5f);

        mPerlin.AmplitudeGain = 0.0f;
        mPerlin.FrequencyGain = 0.0f;
    }


}
