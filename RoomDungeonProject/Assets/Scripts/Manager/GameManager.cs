using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int keyCount = 0;

    public Text keyText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddKey(int count)
    {
        keyCount += count;
        keyText.text = keyCount.ToString();
        SoundManager.instance.PlaySFX(SFXType.EquipSound);
        PlayerPrefs.SetInt("Key", keyCount);
    }

    public int GetInKey()
    { 
        return keyCount;
    }

    public void ResetKey()
    { 
        keyCount = 0;  
        PlayerPrefs.SetInt("Key", keyCount);   
    }


}
