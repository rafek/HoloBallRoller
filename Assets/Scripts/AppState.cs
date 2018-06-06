using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;

public class AppState : MonoBehaviour, IInputClickHandler
{
    public GameObject PlayerPrefab;
    public GameObject TargetPrefab;
    public int TargetCount;
    public Material OcclusionMaterial;

    private bool _targetsPlaced = false;
    private bool _playerPlaced = false;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Vector3 focusPoint;

        if (!_targetsPlaced)
        {
            if (Utils.IsFloorPointed(out focusPoint))
            {
                Instantiate(TargetPrefab, focusPoint, Quaternion.identity);

                TargetCount = TargetCount - 1;
            }

            _targetsPlaced = TargetCount == 0;

            if (_targetsPlaced)
            {
                SurfaceMeshesToPlanes.Instance.MakePlanes();
            }
        }
        else if (!_playerPlaced)
        {
            if (Utils.IsFloorPointed(out focusPoint))
            {
                Instantiate(PlayerPrefab, focusPoint + Vector3.up * .3f, Quaternion.identity);

                _playerPlaced = true;

                SpatialMappingManager.Instance.SetSurfaceMaterial(OcclusionMaterial);

                InputManager.Instance.RemoveGlobalListener(gameObject);
            }
        }

        eventData.Use();
    }

    private void Start()
    {
        InputManager.Instance.AddGlobalListener(gameObject);
    }
}
