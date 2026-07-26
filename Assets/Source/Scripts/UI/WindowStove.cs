using Game.ObjectInteractable;
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
            spawnerStove.OnInitEv += () => spawnerStove.Curent.OnExecuteEv += () => panel.gameObject.SetActive(true);
        }

        private void Add()
        {
            var list = player.LHand.GetComponentsInChildren<ItemRes>();
            for (var i = list.Length - 1; i >= 0; i--)
            {
                if (list[i].ResType == TypeRes.Wood)
                {
                    var wood = list[i];
                    if (spawnerStove.Curent.AddSomeFirewood())
                    {
                        panel.gameObject.SetActive(false);
                        Destroy(wood.gameObject);
                        player.ItemsEditInHand();
                        return;
                    }
                }
            }

            panel.gameObject.SetActive(false);
        }

        private void UpdateStove()
        {
            if (spawnerStove.Data.HasUpdate(lvlStoreCurrent, out TypeRes? lvlStore))
            {
                var amount = 0;
                var items = player.LHand.GetComponentsInChildren<ItemRes>();
                foreach (var resHand in items)
                {
                    if (resHand.ResType == lvlStore)
                        amount++;
                }

                if (spawnerStove.Data.TryUpdate(lvlStore.Value, amount, out var newStove))
                {
                    lvlStoreCurrent = lvlStore.Value;
                    var count = spawnerStove.Curent.AmountFireWood;
                    Destroy(spawnerStove.Curent.gameObject);
                    spawnerStove.SetStove(Instantiate(newStove, spawnerStove.transform));
                    spawnerStove.Curent.OnExecuteEv += () => panel.gameObject.SetActive(true);

                    for (var i = 0; i < count; i++)
                        spawnerStove.Curent.AddSomeFirewood();

                    var step = 0;
                    for (var i = items.Length - 1; i >= 0; i--)
                    {
                        if (step == amount) break;

                        if (items[i].ResType == lvlStore.Value)
                            Destroy(items[i].gameObject);
                    }

                    player.ItemsEditInHand();
                }
            }

            panel.gameObject.SetActive(false);
        }
    }
}