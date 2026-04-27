using NUnit.Framework;
using UnityEngine;

public class GridFireplaceManager : MonoBehaviour
{
    public static GridFireplaceManager Instance;
    private string _output = "";

    [SerializeField] private FireplaceButton[] fireplaceButtons;
    private bool[,] isSelected = new bool[3, 3];
    private void Update()
    {
        for (int i = 0; i< isSelected.GetLength(0); i++)
        {
            for (int j = 0; j < isSelected.GetLength(1); j++)
            {
                _output += isSelected[i, j] ? "1" : "0";
            }
            _output += "\n";
        }
        Debug.Log(_output );
    }

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else
            GameObject.Destroy(this);
    }
}
