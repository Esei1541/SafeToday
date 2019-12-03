using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DebugFeverBoost(float value)
    {
        FeverManager.FeverIncrease(value);
    }

    public void PlayerPrefsDelete()
    {
        PlayerPrefs.DeleteAll();
    }
}
