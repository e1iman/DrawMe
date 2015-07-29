using UnityEngine;
using System.Collections;

public class InscribedBounds : MonoBehaviour {

    [Range(0, 15)]
    [SerializeField]
    float height = 1;
    [Range(0, 15)]
    [SerializeField]
    float width = 1;

	void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width * transform.localScale.x, height * transform.localScale.y));
    }

    public void SetScale(float _width, float _height){

        float _scale = ((_width / width) + (_height / height)) / 2;

        transform.localScale = new Vector3(_scale, _scale, 1);

    }
}
