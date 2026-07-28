using System.Collections.Generic;
using System.Collections;
using Game.Abstraction;
using UnityEngine;
using System;

namespace Game.ObjectInteractable
{
    public class StoveWithAudio : InteractableObject
    {
        public event Action OnExecuteEv;

        [SerializeField] private Light light;
        [SerializeField] private ParticleSystem fire;
        [SerializeField] private Transform[] woodPoints;

        [Header("Audio")]
        [SerializeField] private StoveFireAudio fireAudio;

        private List<Transform> _woodPointsList;

        private FireWoodTime _fireWoodTime;

        public bool IsFull() => _woodPointsList.Count == woodPoints.Length;

        public bool IsFire() => fire != null ? fire.gameObject.activeSelf : false;

        public int AmountFireWood => _woodPointsList.Count;

        private void Awake()
        {
            if (fireAudio == null)
            {
                fireAudio = GetComponent<StoveFireAudio>();
            }
        }

        public void Init(FireWoodTime fireWoodTime)
        {
            _fireWoodTime = fireWoodTime;
            _woodPointsList = new(woodPoints.Length);
        }

        public override void Execute()
        {
            OnExecuteEv?.Invoke();
        }

        public bool AddSomeFirewood()
        {
            if (IsFull()) return false;

            for (var i = woodPoints.Length - 1; i >= 0; i--)
            {
                if (!woodPoints[i].gameObject.activeSelf)
                {
                    StartCoroutine(FireWood(woodPoints[i]));
                    return true;
                }
            }

            return false;
        }

        private IEnumerator FireWood(Transform woodPoint)
        {
            _woodPointsList.Add(woodPoint);
            woodPoint.gameObject.SetActive(true);
            fire.gameObject.SetActive(_woodPointsList.Count > 0);

            fireAudio?.SetFireLevel(_woodPointsList.Count);

            var time = 0f;
            var bonus = _fireWoodTime.heat / _fireWoodTime.time;
            while (time < _fireWoodTime.time)
            {
                time += Time.deltaTime;
                Player?.Stats.SetHeat(bonus * Time.deltaTime);
                yield return null;
            }

            _woodPointsList.Remove(woodPoint);
            woodPoint.gameObject.SetActive(false);
            fire.gameObject.SetActive(_woodPointsList.Count > 0);

            fireAudio?.SetFireLevel(_woodPointsList.Count);

            Debug.Log($" hide  {_woodPointsList.Count == 0}");
        }
    }
}