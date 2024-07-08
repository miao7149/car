using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleTrigger : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        //Debug.Log("tt");


        transform.parent.GetComponent<People>().TriggerEnter(other);
    }
}
