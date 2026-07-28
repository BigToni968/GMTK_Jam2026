using UnityEngine;

namespace Game
{
    public class GhostMusic : MonoBehaviour
    {
        [Tooltip("Character to track")]
        public GameObject Player;

        void Update()
        {
            // Set position to closest point to the player
            transform.position = Player.transform.position + Player.transform.forward * 2;
        }
    }
}