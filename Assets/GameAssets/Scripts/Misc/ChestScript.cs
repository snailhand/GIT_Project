using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{ 
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneHandler.instance.NextLevel();
        }
    }
}
