using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HPSliderTracker : MonoBehaviour
{
    [SerializeField]
    HPManager _trackedEntity;
    Slider _slider;
    [SerializeField]
    [Tooltip("If true it will fill the slider as the entity loses health.")]
    bool _fillSlider;
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = _trackedEntity.GetMaxHp();
        
        
    }
    private void Start()
    {
        if (_fillSlider)
        {
            _slider.value = _trackedEntity.GetMaxHp() - _trackedEntity.GetHp();
        }
        else
        {
            _slider.value = _trackedEntity.GetHp();
        }
        
    }
    private void OnEnable()
    {
        _trackedEntity.onHPChange += UpdateSlider;
    }
    private void OnDisable()
    {
        _trackedEntity.onHPChange -= UpdateSlider;
    }
    private void UpdateSlider(int hpValue)
    {
        if (_fillSlider)
        {
            _slider.value = _trackedEntity.GetMaxHp() - _trackedEntity.GetHp();

        }
        else
        {
            _slider.value = _trackedEntity.GetHp();
        }
        
    }
}
