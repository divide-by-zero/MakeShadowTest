using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using Slider = UnityEngine.UI.Slider;

public class HandLight : MonoBehaviour {

    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private float radius = 10;

    [SerializeField]
    private float angle = 25;

    [SerializeField]
    private float deltaAngle = 0.1f;

    [SerializeField]
    private LayerMask targetLayer;

    [SerializeField]
    private MeshFilter shadowMeshFilter;

    [SerializeField]
    private Material cloneShadoMat;

    [SerializeField]
    private Slider radiusSlider;

    [SerializeField]
    private Slider angleSlider;

    [SerializeField]
    private Slider deltaAngleSlider;

    private List<Vector3> vertextList = new List<Vector3>();
    private List<Vector3> shadowVertextList = new List<Vector3>();

    private float _deltaRad;
    private float _startRad;



    void Start()
    {
        Initialize();

        //雑なパラメータ変更処理　動いているのが不思議
        radiusSlider.onValueChanged.AddListener(value =>
        {
            radius = value;
            Initialize();
        });
        angleSlider.onValueChanged.AddListener(value =>
        {
            angle = value;
            Initialize();
        });
        deltaAngleSlider.onValueChanged.AddListener(value =>
        {
            deltaAngle = value;
            Initialize();
        });
    }

    private void Initialize()
    {
        shadowMeshFilter.sharedMesh = CreatePlaneMesh(vertextList);
        meshFilter.sharedMesh = CreatePlaneMesh(shadowVertextList);
    }

    private Mesh CreatePlaneMesh(List<Vector3> vlist)
    {
        vlist.Clear();
        var mesh = new Mesh();
        var indexList = new List<int>();
        _deltaRad = deltaAngle * Mathf.Deg2Rad;
        _startRad = -angle / 2 * Mathf.Deg2Rad;

        var index = 0;
        for (var rad = _startRad; rad < -_startRad; rad += _deltaRad)
        {
            vlist.Add(new Vector3(0, 0, 0));//0番頂点
            vlist.Add(new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0)); //1番頂点
            vlist.Add(new Vector3(0, 0, 0));//0番頂点
            vlist.Add(new Vector3(Mathf.Cos(rad +_deltaRad) * radius, Mathf.Sin(rad + _deltaRad) * radius, 0)); //1番頂点
            indexList.AddRange(new[] { index + 0, index + 2,index + 1,index + 1,index+ 2,index+ 3 });//0-2-1の頂点で1三角形。 1-2-3の頂点で1三角形。
            index += 4;
        }

        mesh.SetVertices(vlist);//meshに頂点群をセット
        mesh.SetIndices(indexList.ToArray(), MeshTopology.Triangles, 0);//メッシュにどの頂点の順番で面を作るかセット
        return mesh;
    }

    public void Update()
    {
        UpdateShadowVitices();

        if (Input.GetMouseButtonDown((int)MouseButton.RightMouse))
        {
            MakeClone();
        }

        //リセット
        if(Input.GetKeyDown(KeyCode.R))SceneManager.LoadScene(0);
    }

    private void UpdateShadowVitices()
    {
        for (var i = 0; i < vertextList.Count - 1; i+=2)
        {
            var startVec = transform.position + vertextList[i];
            var endVec = transform.position + vertextList[i + 1];

            //基本的には縮退させて消す
            shadowVertextList[i] = Vector3.zero;
            shadowVertextList[i + 1] = Vector3.zero;

            RaycastHit hitinfo;
            if (Physics.Linecast(startVec, endVec, out hitinfo, targetLayer))
            {
                //シンプル影
//                vertextList[i] = hitinfo.point - transform.position;
//                vertextList[i + 1] = endVec - transform.position;

                //終端から逆にRaycastAllして、影部分を弾き出す
                var hits = Physics.RaycastAll(new Ray(endVec, startVec - endVec), radius, targetLayer);
                if (hits != null)//ありえないはずだけれどまぁ、一応
                {
                    var farRaycastHit = hits.OrderByDescending(hit => hit.distance).FirstOrDefault();

                    if (farRaycastHit.distance < 0.001f)//終端が埋まっているぽいので回避
                    {
                        continue;
                    }

                    shadowVertextList[i] = farRaycastHit.point - transform.position;
                    shadowVertextList[i + 1] = endVec - transform.position;
                }
            }

        }
        shadowMeshFilter.sharedMesh.SetVertices(shadowVertextList);
    }

    private void MakeClone()
    {
        var go = new GameObject("shadowcopy");
        go.transform.position = transform.position;
        var pos = go.transform.position;
        pos.z = -0.02f;//雑なZorder
        go.transform.position = pos;
        var renderer = go.AddComponent<MeshRenderer>();
        renderer.material = cloneShadoMat;
        var filter = go.AddComponent<MeshFilter>();
        filter.mesh = Instantiate(shadowMeshFilter.sharedMesh);
        go.gameObject.AddComponent<Rigidbody>();
        //ここでちゃんとMeshColliderの設定とかすればまぁ、似た感じになると思うんだけどスルー
        Destroy(go.gameObject, 2.0f);
    }
}
