using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Client()
    {
        SceneManager.LoadScene("Join Game");
    }

    public void Server()
    {
        SceneManager.LoadScene("Create Game");
    }


}
