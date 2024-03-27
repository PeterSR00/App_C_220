using System.Collections.Generic;
using UnityEngine;


public class StateObject : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private int defaultState = 0;
    [SerializeField] private List<GameObject> states;
    

    public RectTransform RectTransform => rectTransform;


    private void Awake()
    {
        SetState(defaultState);
    }


    public void SetState(int index)
    {
        if (index >= states.Count)
        {
            Debug.LogError("StateObject state index overflow", this);
            return;
        }
        
        ChangeState(index);
    }


    private void ChangeState(int index)
    {
        for (int i = 0; i < states.Count; i++)
        {
            states[i].SetActive(i == index);
        }
    }
}
