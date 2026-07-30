using System.Collections.Generic;
using Game.ObjectInteractable;
using UnityEngine;
using System;

namespace Game.Unit.Component
{
    [Serializable]
    public struct HandItem
    {
        public TypeRes res;
        public Transform point;
    }
    
    public class LHandInventory : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private  HandItem[] handItems;

        private Dictionary<TypeRes, int> _handItemDict;
        private Dictionary<TypeRes, Transform> _handTransformDict;

        private void Awake()
        {
            _handItemDict = new(handItems.Length);
            _handTransformDict = new(handItems.Length);
            foreach (var handItem in handItems)
                _handItemDict[handItem.res] = default;
            foreach (var handItem in handItems)
                _handTransformDict[handItem.res] = handItem.point;
        }

        public bool HasItem(TypeRes res, int amount) => GetAmount(res) >= amount;
        public int GetAmount(TypeRes res) => _handItemDict[res];

        public void Add(ItemRes instance, int amount)
        {
            if (_handItemDict[instance.ResType] > 0)
            {
                _handItemDict[instance.ResType] += amount;
                player.ItemsEditInHand();
                return;
            }
            
            _handItemDict[instance.ResType] += amount;
            var res = Instantiate(instance,_handTransformDict[instance.ResType]);
            res.Collider.isTrigger  = true;
            res.gameObject.layer = 0;
            res.Body.isKinematic = true;
            res.Body.useGravity = false;
            res.transform.localScale = transform.localScale / 3;
            res.transform.localRotation = Quaternion.Euler(Vector3.zero);
            res.transform.localPosition = Vector3.zero;
            player.ItemsEditInHand();
        }

        public void Remove(TypeRes res, int amount)
        {
            _handItemDict[res] -= amount;
            _handItemDict[res] = Math.Clamp(_handItemDict[res], 0, int.MaxValue);

            ItemRes ress = null;
            if (_handItemDict[res] <= 0 && (ress = _handTransformDict[res].GetComponentInChildren<ItemRes>()) != null)
                Destroy(ress.gameObject);
            
            player.ItemsEditInHand();
        }
    }
}