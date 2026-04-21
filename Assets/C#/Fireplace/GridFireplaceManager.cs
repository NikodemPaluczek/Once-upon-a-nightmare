using UnityEngine;

public class GridFireplaceManager : MonoBehaviour
{
    public static GridFireplaceManager Instance;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else
            GameObject.Destroy(this);
    }
    private FireplaceButton _currentFireplaceButton;
    public void ManageRaycast(FireplaceButton fireplaceButton)
    {
         if (fireplaceButton == null && _currentFireplaceButton != null)
         {
            _currentFireplaceButton.ResetColor();
            _currentFireplaceButton = null;
            return;
         }

        if (fireplaceButton != _currentFireplaceButton)
        {
            if (_currentFireplaceButton != null)
                _currentFireplaceButton.ResetColor();

            _currentFireplaceButton = fireplaceButton;
            _currentFireplaceButton.HighlightColorChange();

        }
    }
}
