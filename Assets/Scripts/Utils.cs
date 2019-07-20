using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class Utils
{
    public static void ReloadLvl()
    {
        int indx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(indx);
    }
    public static void LoadAnyLvl(int lvl)
    {
        SceneManager.LoadScene(lvl);
    }
}

