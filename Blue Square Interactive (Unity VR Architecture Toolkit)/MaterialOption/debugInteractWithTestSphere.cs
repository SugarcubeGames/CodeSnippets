using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugInteractWithTestSphere : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.gameObject.name);
    }

}
