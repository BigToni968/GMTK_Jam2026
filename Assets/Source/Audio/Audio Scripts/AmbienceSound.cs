using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Unit;
using Game;

public class AmbienceSound : MonoBehaviour
{

    public AudioClip AudioClip;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name); 
        if (other.TryGetComponent(out GhostMusic ghost))
        {
            SoundManager.PlaySound(SoundType.STORM);
        }
    }
}
