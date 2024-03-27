using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class ShopItem : MonoBehaviour
{
    public enum ShopItemState
    {
        None                = 0,
        
        Closed              = 1,
        Opened              = 2,
        Selected            = 3,
    }


    [SerializeField] private bool isCoinPrice = true;
    [SerializeField] private int id;
    [SerializeField] private float price;
    [SerializeField] private List<Text> priceLabels;
    [SerializeField] private ButtonBase buyButton;
    [SerializeField] private ButtonBase selectButton;
    [Space]
    [SerializeField] private GameObject openedObject;
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private GameObject closedObject;

    private ShopItemState _currentState;
    private Sequence _flipSequence;


    public int Id => id;


    private void Start()
    {
        Initialize();
    }


    private void OnDestroy()
    {
        _flipSequence?.Kill();

        CurrencyManager.Instance.OnCurrencyAmountChanged -= CurrencyAmount_OnChange;
    }


    private void Initialize()
    {
        buyButton.OnClick.AddListener(BuyButton_OnClick);
        selectButton.OnClick.AddListener(SelectButton_OnClick);

        CurrencyManager.Instance.OnCurrencyAmountChanged += CurrencyAmount_OnChange;
        SkinManager.Instance.OnSelect.AddListener(Skin_OnSelect);
            
        var initialState = !SkinManager.Instance.IsPurchased(id)
            ? ShopItemState.Closed
            : SkinManager.Instance.CurrentIndex == id
                ? ShopItemState.Selected
                : ShopItemState.Opened;
            
        ChangeState(initialState);
        
        SetPrice(price);
    }


    private void SetPrice(float amount)
    {
        price = amount;

        var value = price.ToString();

        foreach (var priceLabel in priceLabels)
        {
            priceLabel.text = value;
        }
    }


    private void Open()
    {
        var currencyType = isCoinPrice
            ? CurrencyType.Coins
            : CurrencyType.Rockets;
        
        if (CurrencyManager.Instance.TryRemoveCurrency(currencyType, price))
        {
            ChangeState(ShopItemState.Opened);

            SkinManager.Instance.BuyPack(id);

            AudioManager.Instance.PlaySound(AudioClipType.Purchase);
        }
    }


    private void Select()
    {
        SkinManager.Instance.SelectPack(id);

        ChangeState(ShopItemState.Selected);
    }


    private void SetInteractable(bool isInteractable)
    {
        buyButton.SetInteractable(isInteractable);
    } 
    

    private void ChangeState(ShopItemState state)
    {
        _currentState = state;
        
        openedObject.SetActive(state == ShopItemState.Opened);
        selectedObject.SetActive(state == ShopItemState.Selected);
        closedObject.SetActive(state == ShopItemState.Closed);

        SetInteractable(state != ShopItemState.Selected);
        CheckInteractable();
    }


    private void CheckInteractable()
    {
        if (_currentState == ShopItemState.Closed)
        {
            var coinsAmount = CurrencyManager.Instance.GetCurrencyAmount(CurrencyType.Coins);
            var rocketsAmount = CurrencyManager.Instance.GetCurrencyAmount(CurrencyType.Rockets);
            var currencyAmount = isCoinPrice
                ? coinsAmount
                : rocketsAmount;
            
            SetInteractable(currencyAmount >= price);
        }
    }


    private void BuyButton_OnClick()
    {
        if (_currentState == ShopItemState.Closed)
        {
            Open();
        }
    }
    
    
    private void SelectButton_OnClick()
    {
        if (_currentState == ShopItemState.Opened)
        {
            Select();
        }
    }


    private void CurrencyAmount_OnChange(CurrencyType type)
    {
        if ((type == CurrencyType.Coins && isCoinPrice) ||
            type == CurrencyType.Rockets && !isCoinPrice)
        {
            CheckInteractable();
        }
    }


    private void Skin_OnSelect()
    {
        if (_currentState == ShopItemState.Selected)
        {
            ChangeState(ShopItemState.Opened);
        }
    }
}