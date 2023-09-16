using TMPro;
using UniRx;
using UnityEngine;

namespace Units
{
    public class UnitPackView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _packCount;

        public void Initialize(UnitPack pack)
        {
            pack.UnitCurrentCount.Subscribe(value => _packCount.text = value.ToString()).AddTo(this);
        }
    }
}