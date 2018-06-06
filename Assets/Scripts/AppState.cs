using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AppState : MonoBehaviour, IInputClickHandler
{
    public GameObject PlayerPrefab;
    public int TargetCount;
    public Material OcclusionMaterial;

    private GameObject _targetPrefab;
    private bool _targetsPlaced = false;
    private bool _playerPlaced = false;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Vector3 focusPoint;

        if (!_targetsPlaced)
        {
            if (Utils.IsFloorPointed(out focusPoint))
            {
                Instantiate(_targetPrefab, focusPoint, Quaternion.identity);

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

    IEnumerator Start()
    {
        InputManager.Instance.AddGlobalListener(gameObject);

        string uri = "https://holoballroller.blob.core.windows.net/assetscontainer/targetbundle.hd";

        var request = UnityWebRequest.GetAssetBundle(uri, 0);

        yield return request.Send();

        var bundle = DownloadHandlerAssetBundle.GetContent(request);

        _targetPrefab = bundle.LoadAsset<GameObject>("Target");
    }
}
