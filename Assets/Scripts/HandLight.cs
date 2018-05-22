using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class HandLight : MonoBehaviour {

    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshFilter shadowMeshFilter;
    [SerializeField]
    private LayerMask targetLayer;
    [SerializeField]
    private Material cloneShadoMat;

    [SerializeField]
    private Slider radiusSlider;
    [SerializeField]
    private Slider angleSlider;
    [SerializeField]
    private Slider deltaAngleSlider;
    [SerializeField]
    private Toggle makeColliderToggle;

    private List<Vector3> vertextList;
    private List<Vector3> shadowVertextList;

    private float radius = 10;
    private float angle = 45;
    private float deltaAngle = 0.2f;
    private float _deltaRad;
    private float _startRad;
    private bool isMakeCollider = false;
    void Start()
    {
        Initialize();

        //雑なパラメータ変更処理　動いているのが不思議
        radiusSlider.value = radius;
        radiusSlider.onValueChanged.AddListener(value =>
        {
            radius = value;
            Initialize();
        });
        angleSlider.value = angle;
        angleSlider.onValueChanged.AddListener(value =>
        {
            angle = value;
            Initialize();
        });
        deltaAngleSlider.value = deltaAngle;
        deltaAngleSlider.onValueChanged.AddListener(value =>
        {
            deltaAngle = value;
            Initialize();
        });
        makeColliderToggle.isOn = isMakeCollider;
        makeColliderToggle.onValueChanged.AddListener(flg =>
        {
            isMakeCollider = flg;
        });
    }

    private void Initialize()
    {
        vertextList = CreateFanVertices();
        shadowMeshFilter.sharedMesh = CreateFanMesh(vertextList);
        shadowVertextList = CreateFanVertices();
        meshFilter.sharedMesh = CreateFanMesh(shadowVertextList);
    }

    private List<Vector3> CreateFanVertices()
    {
        var vlist = new List<Vector3>();
        _deltaRad = deltaAngle * Mathf.Deg2Rad;
        _startRad = -angle / 2 * Mathf.Deg2Rad;

        //頂点作成
        for (var rad = _startRad; rad <= -_startRad; rad += _deltaRad)
        {
            vlist.Add(new Vector3(0, 0, 0));//0番頂点,2番頂点、4番頂点・・・
            vlist.Add(new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0)); //1番頂点、3番頂点、5番頂点・・・
        }
        return vlist;
    }

    private Mesh CreateFanMesh(List<Vector3> vlist)
    {
        var mesh = new Mesh();
        var indexList = new List<int>();
        //インデックス作成
        for (var i = 0; i < vlist.Count-2; i+=2)
        {
            indexList.AddRange(new[] { i + 0, i + 2, i + 1, i + 1, i + 2, i + 3 });//0-2-1の頂点で1三角形。 1-2-3の頂点で1三角形。
            indexList.AddRange(new[] { i + 0, i + 1, i + 2, i + 1, i + 3, i + 2 });//両面にしないと生成したMeshColliderがちょっとおかしくなる
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
            MakeInstance();
        }

        //リセット
        if(Input.GetKeyDown(KeyCode.R))SceneManager.LoadScene(0);
    }

    private void UpdateShadowVitices()
    {
        //ライトが回転している場合もあるので、回転と、逆回転のQuaternionを先に取得しておく
        var rot = transform.rotation;
        var invert = Quaternion.Inverse(rot);

        for (var i = 0; i < vertextList.Count - 1; i+=2)
        {
            var startVec = transform.position + rot * vertextList[i];
            var endVec = transform.position + rot * vertextList[i + 1];

            //基本的には縮退させて消す
            shadowVertextList[i] = Vector3.zero;
            shadowVertextList[i + 1] = Vector3.zero;

            //終端から逆にRaycastAllして、影部分を弾き出す
            var hits = Physics.RaycastAll(new Ray(endVec, startVec - endVec), radius, targetLayer);
            if (hits != null)
            {
                var farRaycastHit = hits.OrderByDescending(hit => hit.distance).FirstOrDefault();
                if (farRaycastHit.distance < 0.001f) continue; //終端が埋まっているぽいので回避

                shadowVertextList[i] = invert * (farRaycastHit.point - transform.position);
                shadowVertextList[i + 1] = invert * (endVec - transform.position);
            }
        }
        shadowMeshFilter.sharedMesh.SetVertices(shadowVertextList);
    }

    private void MakeInstance()
    {
        var makeShadowVertices = new List<Vector3>();
        foreach (var vpos in shadowVertextList)
        {
            if (vpos == Vector3.zero)
            {
                if (makeShadowVertices.Any())
                {
                    MakeShadowInstance(makeShadowVertices);
                    makeShadowVertices.Clear();
                }
            }
            else
            {
                makeShadowVertices.Add(vpos);
            }
        }
        if (makeShadowVertices.Any())
        {
            MakeShadowInstance(makeShadowVertices);
        }
    }

    private void MakeShadowInstance(List<Vector3> vlist)
    {
        var go = new GameObject("ShadowInstance");  //実体化した影
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
        var pos = go.transform.position;
        pos.z = -0.02f;//雑なZorder
        go.transform.position = pos;
        var renderer = go.AddComponent<MeshRenderer>();
        renderer.sharedMaterial = cloneShadoMat;
        var filter = go.AddComponent<MeshFilter>();
        filter.mesh = CreateFanMesh(vlist);
        var rigibody = go.gameObject.AddComponent<Rigidbody>();
        if (isMakeCollider)
        {
            rigibody.isKinematic = true;
            var collder = go.AddComponent<MeshCollider>();
        }
        Destroy(go.gameObject, 2.0f);
    }
}
