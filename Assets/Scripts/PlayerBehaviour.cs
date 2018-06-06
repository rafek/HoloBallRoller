using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour, IInputClickHandler
{
    public float ForceMultiplier = 10.0f;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Vector3 focusPoint;

        if (Utils.IsFloorPointed(out focusPoint))
        {
            var focusPointNorm = new Vector3(focusPoint.x, transform.position.y, focusPoint.z);
            var playerPoint = transform.position;

            gameObject.GetComponent<Rigidbody>().AddForce((focusPointNorm - playerPoint) * ForceMultiplier);
        }
    }

    private void Start()
    {
        InputManager.Instance.AddGlobalListener(gameObject);
    }
}
