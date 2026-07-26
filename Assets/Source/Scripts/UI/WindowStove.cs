using UnityEngine.UI;
using UnityEngine;
using Game.Unit;

namespace Game.UI
{
    public class WindowStove : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private SpawnerStove spawnerStove;
        [SerializeField] private Button addFireWood;
        [SerializeField] private Button updateStove;
        [SerializeField] private Image panel;

        private void Start()
        {
            addFireWood.onClick.AddListener(Add);
            updateStove.onClick.AddListener(UpdateStove);
            spawnerStove.Curent.OnExecuteEv += () => panel.gameObject.SetActive(true);
        }

        private void Add()
        {
            Debug.Log(spawnerStove.Curent.AddSomeFirewood());
            panel.gameObject.SetActive(false);
        }

        private void UpdateStove()
        {
            panel.gameObject.SetActive(false);
        }
    }
}