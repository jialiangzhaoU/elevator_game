using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start_menu : MonoBehaviour
{
    public void changeScenne(string name) 
    {
        SceneManager.LoadScene(name);
    }
}
