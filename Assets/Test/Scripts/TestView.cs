using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestView : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //_Collider.enabled = false;
            Ray ray = CameraController.Instance._MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Turret")))
            {
                var turret = hit.transform.GetComponent<Turret>();
                if (turret != null)
                {
                    turret.OnClick();
                }
                Debugger.Log("hit.name=" + hit.transform.name);
            }
            //_Collider.enabled = true;
        }
    }
}
