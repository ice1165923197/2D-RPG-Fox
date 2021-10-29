using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    public GameObject enterDialog;
    //public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //anim = enterDialog.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            enterDialog.SetActive(true);
            //anim.SetBool("exit", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //anim.SetBool("exit", true);
            enterDialog.SetActive(false);
        }
    }
    

}
