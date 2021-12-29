using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject OptionsMenuCanvas;
}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

public void option_btn_clicked()
{
    mainMenu.SetActive(false);
    optionMenu.SetActive(true);
}
public void option_back_clicked()
{
    optionMenu.SetActive(false);
    mainMenu.SetActive(true);
}
}
