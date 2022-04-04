using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Aim : MonoBehaviour
{
    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camPos;
    [SerializeField] Transform AimReticle;
    [SerializeField] float aimSmoothSpeed = 20;
    [SerializeField] LayerMask aimMask;
    // Start is called before the first frame update
    void Start()
    {
        // Ubah ke arah yang tepat pada awalnya
        xAxis.Value = -150;
    }

    // Update is called once per frame
    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
        Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCentre);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
        {
            AimReticle.position = Vector3.Lerp(AimReticle.position, hit.point, aimSmoothSpeed * Time.deltaTime);
        }
    }


    private void LateUpdate()
    {
        camPos.localEulerAngles = new Vector3(yAxis.Value, camPos.localEulerAngles.y, camPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }

    public Transform getReticle()
    {
        return AimReticle;
    }
}