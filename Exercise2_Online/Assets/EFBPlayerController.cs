using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFBPlayerController : MonoBehaviour
{
    private EFBActions controls;

    private void Awake()
    {
        controls = new EFBActions();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
