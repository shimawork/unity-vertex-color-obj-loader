using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;


public class VertexColorObjLoader : MonoBehaviour {

    //　頂点配列
    public Vector3[] vertices;
    //　UV配列
    public Vector2[] uvs;
    //　三角形の順番配列
    public int[] triangles;

    //頂点カラー
    public Color[] colors;
    //　メッシュ
    private Mesh mesh;
    //　メッシュ表示コンポーネント
    private MeshRenderer meshRenderer;
    //　メッシュに設定するマテリアル
    public Material material;

	// Use this for initialization
	void Start () {
		//
		loadObj(Application.dataPath + "/vertexColordobjLoader/sample.obj");

        gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();

        mesh = GetComponent<MeshFilter>().mesh;
        material = new Material(Shader.Find("Custom/VertexColorShader"));
        meshRenderer.material = material;

        //　頂点の設定
        mesh.vertices = vertices;
        //　三角形メッシュの設定
        mesh.triangles = triangles;
        //頂点RGB
        mesh.colors = colors;

        mesh.RecalculateNormals ();
        var filter = GetComponent<MeshFilter> ();
        filter.sharedMesh = mesh;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //objファイルから、vertices、color、trianglesをloadする。
    private void loadObj(String fileName)
    {
        //StringBuilder sb = new StringBuilder();
        string[] lines = File.ReadAllLines(fileName);
        //int start = 0;
        StringBuilder sbFloat = new StringBuilder();
        List<Vector3> verticesList = new List<Vector3>();
        List<Color> colorsList = new List<Color>();
        List<int> trianglesList = new List<int>();

        float scale = 1;
        foreach (string line in lines)
        {
            //半スペでsplit
            string[] tokens = line.Split(null);
            if (tokens[0] == "v")
            {
                try
                {
                    float x = float.Parse(tokens[1]) /scale;
                    float y = float.Parse(tokens[2]) /scale;
                    float z = float.Parse(tokens[3]) /scale;
                    //float z = 0;
                    verticesList.Add(new Vector3(x, y, z));
                    float r = float.Parse(tokens[4]);
                    float g = float.Parse(tokens[5]);
                    float b = float.Parse(tokens[6]);
                    colorsList.Add(new Color(r,g,b,1.0f));

                }
                catch (Exception e)
                {
                    //数値に変換できないとかゴミを握りつぶす。
                    Debug.Log(e.StackTrace);
                }
            }
            else if (tokens[0] == "f")
            {
                for (int k = 1; k < tokens.Length; k++)
                {
                    try
                    {
                        int tr = int.Parse(tokens[k]);
                        tr = tr -1;
                        trianglesList.Add(tr);
                    }
                    catch (Exception e)
                    {
                        //数値に変換できないとかゴミを握りつぶす。
                        Debug.Log(e.StackTrace);
                    }
                }
            }
        }
        this.vertices = verticesList.ToArray();
        this.triangles = trianglesList.ToArray();
        this.colors = colorsList.ToArray();
    }

}
