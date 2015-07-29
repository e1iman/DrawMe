using UnityEngine;
using System.Collections;

public class MouseFollow : MonoBehaviour {

    ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        ps.enableEmission = Input.GetMouseButton(0);

        Vector3 _pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _pos.z = -1;
        transform.position = _pos;
    }
}
