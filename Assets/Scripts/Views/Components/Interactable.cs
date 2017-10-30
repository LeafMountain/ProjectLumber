using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	public void Interact(){
        Extractable extractable = GetComponent<Extractable>();

        if(extractable){
            
        }
        Debug.Log("Interacting");
    }
}
