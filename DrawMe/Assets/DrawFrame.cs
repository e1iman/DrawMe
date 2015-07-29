using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawFrame : MonoBehaviour {

    GameObject img;

    float minX;
    float minY;
    float maxX;
    float maxY;

    Vector3 center;
    Vector3 size;

    List<Vector3> list;
    [Range(0, 0.25f)]
    public float radius = 0.08f;

    [SerializeField]
    LayerMask layerMaskImage; // image layer
    [SerializeField]
    LayerMask layerMaskCheckPoint; // checkpoints layer

    void Awake()
    {
        img = GM.master.GetRandomImage();
    }

	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            minX = int.MaxValue;
            minY = int.MaxValue;
            maxX = int.MinValue;
            maxY = int.MinValue;

            img.SetActive(false);

            StartCoroutine("MouseCoordRecord");
        }
        if (Input.GetMouseButton(0))
        {
            minX = minX > Camera.main.ScreenToWorldPoint(Input.mousePosition).x ? Camera.main.ScreenToWorldPoint(Input.mousePosition).x : minX;
            minY = minY > Camera.main.ScreenToWorldPoint(Input.mousePosition).y ? Camera.main.ScreenToWorldPoint(Input.mousePosition).y : minY;
            maxX = maxX < Camera.main.ScreenToWorldPoint(Input.mousePosition).x ? Camera.main.ScreenToWorldPoint(Input.mousePosition).x : maxX;
            maxY = maxY < Camera.main.ScreenToWorldPoint(Input.mousePosition).y ? Camera.main.ScreenToWorldPoint(Input.mousePosition).y : maxY;
        }

        if (Input.GetMouseButtonUp(0))
        {
            center = new Vector3(((maxX + minX) / 2), ((maxY + minY) / 2));
            size = new Vector3((maxX - minX), (maxY - minY));

            img.transform.position = center;
            img.GetComponent<InscribedBounds>().SetScale((maxX - minX), (maxY - minY));
            img.SetActive(true);

            StopCoroutine("MouseCoordRecord");
            CheckHits();
        }
	}

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(center, size);
        if (list != null)
        {
            foreach (var el in list)
            {
                Gizmos.DrawWireSphere(el, radius);
            }
        }
    }

    IEnumerator MouseCoordRecord()
    {
        list = new List<Vector3>(100);
        while (true)
        {
            if (list.Count + 10 > list.Capacity)
            {
                list.Capacity += 50;
            }
            list.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            yield return new WaitForSeconds(0.015f);
        }
    }

    void CheckHits()
    {
        int total = list.Count;
        int missed = 0;
        HashSet<Collider2D> checkPoints = new HashSet<Collider2D>();
        for (int i = 0; i < total; i++)
        {
            if (!Physics2D.OverlapCircle(list[i], radius, layerMaskImage))
            {
                missed++;
            }

            Collider2D col = Physics2D.OverlapCircle(list[i], radius, layerMaskCheckPoint);

            if (col != null)
            {
                checkPoints.Add(col);
            }
        }


        Debug.Log("Missed " + missed + " out of total " + total + " - " + (((float)missed / total) * 100).ToString("F1") + "%");
        GM.master.SetAccText(100 - ((float)missed / total) * 100);

        if (missed == 0 && checkPoints.Count == img.transform.childCount)
        {
            img.SetActive(false);
            GM.master.AddScore(1);
            img = GM.master.GetRandomImage(img);
        }
    }

    public void OnEnable()
    {
        img = GM.master.GetRandomImage();
    }
}
