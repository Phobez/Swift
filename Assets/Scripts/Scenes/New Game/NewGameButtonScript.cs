﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameButtonScript : MonoBehaviour{

    public void ToggleMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
