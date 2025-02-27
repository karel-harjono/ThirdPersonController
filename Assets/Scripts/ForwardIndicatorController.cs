using Unity.Cinemachine;
using UnityEngine;

public class ForwardIndicatorController : MonoBehaviour
{
    [SerializeField] Transform indicatorAnchor;
    [SerializeField] CinemachineCamera freeLookCamera;

    private void Update()
    {
        transform.forward = freeLookCamera.transform.forward;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
