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

        private TypeRes lvlStoreCurrent = TypeRes.Free;


        private void Awake()
        {
            addFireWood.onClick.AddListener(Add);
            updateStove.onClick.AddListener(UpdateStove);
            spawnerStove.OnInitEv += () => spawnerStove.Curent.OnExecuteEv += () =>
            {
                Cursor.lockState = CursorLockMode.None;
                panel.gameObject.SetActive(true);
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && panel.gameObject.activeSelf)
            {
                panel.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        private void Add()
        {
            if (player.LHand.HasItem(TypeRes.Wood, 1) && spawnerStove.Curent.AddSomeFirewood())
                player.LHand.Remove(TypeRes.Wood, 1);
            panel.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void UpdateStove()
        {
            if (spawnerStove.Data.HasUpdate(lvlStoreCurrent, out TypeRes? lvlStore))
            {

                if (spawnerStove.Data.TryUpdate(lvlStore.Value, player.LHand.GetAmount(lvlStore.Value), out var newStove))
                {
                    lvlStoreCurrent = lvlStore.Value;
                    var count = spawnerStove.Curent.AmountFireWood;
                    Destroy(spawnerStove.Curent.gameObject);
                    spawnerStove.SetStove(Instantiate(newStove, spawnerStove.transform));
                    spawnerStove.Curent.OnExecuteEv += () =>
                    {
                        Cursor.lockState = CursorLockMode.None;
                        panel.gameObject.SetActive(true);
                    };

                    for (var i = 0; i < count; i++)
                        spawnerStove.Curent.AddSomeFirewood();
                    
                    player.LHand.Remove(lvlStore.Value,spawnerStove.Data.GetAmount(lvlStore.Value));
                }
            }
            panel.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }
}