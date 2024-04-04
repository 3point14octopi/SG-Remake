using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSwap : MonoBehaviour
{
    public GameObject DDOL;
    public Image spriteRenderer;
    public Sprite WinScreen;
    public Sprite LoseScreen;
    
    // Start is called before the first frame update
    void Start()
    {
        DDOL = GameObject.FindGameObjectWithTag("DDOL");
        if(DDOL.GetComponent<DontDestroy>().win == true){
            spriteRenderer.sprite = WinScreen;
        }
        else if(DDOL.GetComponent<DontDestroy>().win == false){
            spriteRenderer.sprite = LoseScreen;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
