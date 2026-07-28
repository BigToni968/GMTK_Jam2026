using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GhostMusic : MonoBehaviour
    {
        [Tooltip("Area of the sound to be in")]
        public Collider Area;
        [Tooltip("Character to track")]
        public GameObject Player;
        public AudioSource AudioSource;

        void Update()
        {
            
            // Set position to closest point to the player
            transform.position = Player.transform.position + Player.transform.forward * 2;
        }
    }
}
