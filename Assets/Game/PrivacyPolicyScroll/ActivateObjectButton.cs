using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActivateObjectButton : MonoBehaviour
{
    [SerializeField] private string _objectName = null;
    
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GameObject[] gameObjects = FindObjectsOfType<GameObject>(true);

            foreach (var gameObject in gameObjects)
            {
                if (gameObject.name == _objectName)
                {
                    gameObject.SetActive(true);
                    
                    break;
                }
            }
        });
    }
}
