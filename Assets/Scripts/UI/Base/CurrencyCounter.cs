﻿using UnityEngine;


public class CurrencyCounter : CounterBase
{
    [SerializeField] private CurrencyType currencyType;

    private CurrencyManager _cachedCurrencyManager;
    
    
    public CurrencyType CurrencyType => currencyType;
    
    
    private void Awake()
    {
        _cachedCurrencyManager = CurrencyManager.Instance;
        
        _cachedCurrencyManager.OnCurrencyAmountChanged += CurrencyManager_OnChange;
        
        UpdateCurrency();
    }


    private void OnDestroy()
    {
        _cachedCurrencyManager.OnCurrencyAmountChanged -= CurrencyManager_OnChange;
    }


    private void UpdateCurrency()
    {
        var newValue = _cachedCurrencyManager.GetCurrencyAmount(currencyType);
        
        UpdateValue(newValue);
    }


    private void CurrencyManager_OnChange(CurrencyType type)
    {
        if (type != currencyType) return;

        UpdateCurrency();
    }
}
