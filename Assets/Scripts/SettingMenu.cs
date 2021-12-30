using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public GameObject SettingUI;

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSetting()
    {
        SettingUI.SetActive(true);
    }

    public void CloseSetting()
    {
        SettingUI.SetActive(false);
    }
}
