using mattatz.Triangulation2DSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
  public FloorMgr m_floor;
  public RoomOutline m_outline;
  public JRoom JRm
  {
    get { return m_outline.m_jRoom; }
  }

	// Use this for initialization
	void Start ()
  {
    try
    {
      Setup();
      // DebugOutline();

      m_floor.m_canves.referencePixelsPerUnit = m_floor.PixelPerUnit;
    }
    catch(Exception ex)
    {
      Debug.LogError(ex);
    }
  }

  void TryGetShape(Vector2[] rawV2ds, ref Vector2[] v2ds, ref int[] indices)
  {
    List<Vector2> vs = new List<Vector2>(rawV2ds);
    for (int j = 0; j < rawV2ds.Length; j++)
    {
      Triangulator tr = new Triangulator(vs.ToArray());
      indices = tr.Triangulate();
      int i = 0;
      bool needChange = false;
      while (true)
      {
        if (i + 2 >= vs.Count)
        {
          break;
        }
        double testAngle = Vector2.SignedAngle((vs[indices[i + 2]] - vs[indices[i + 1]]), vs[indices[i + 1]] - vs[indices[i]]);
        testAngle = Math.Abs(testAngle);
        if (testAngle < 1 || testAngle > 159)
        {
          needChange = true;
          break;
        }
        i = i + 3;
      }
      if (needChange)
      {
        Vector2 v = vs[0];
        vs.RemoveAt(0);
        vs.Add(v);
      }
      else
      {
        v2ds = vs.ToArray();
        break;
      }
    }
  }

  void Setup()
  {
    /// set position
    GameObject gObj = gameObject;
    Vector2 pos = Utils.Rvt2CamCoord(m_outline.BD.min, m_floor.FloorBDs, true, m_floor.PixelPerUnit);
    gObj.transform.position = pos;

    /// calculate mesh
    Vector2[] rawV2ds = m_outline.OriginalPnts.ToArray();
    Vector2[] v2ds = null;
    int[] indices = null;
    TryGetShape(rawV2ds, ref v2ds, ref indices);
    DebugMesh(v2ds, indices);

    /// create Sprite
    float w = m_outline.BD.max.x - m_outline.BD.min.x;
    float h = m_outline.BD.max.y - m_outline.BD.min.y;
    Texture2D tx = new Texture2D((int)Math.Ceiling(w) * 10, (int)Math.Ceiling(h) * 10);
    Rect rct = new Rect(0, 0, w, h);
    Sprite newSp = Sprite.Create(tx, rct, new Vector2(0, 0), m_floor.PixelPerUnit);

    /// reshape
    v2ds = Utils.Rvt2RevCoord(v2ds, m_outline.BD, false).ToArray();
    newSp.OverrideGeometry(v2ds, Utils.ToUShort(indices));

    /// SpriteRender
    SpriteRenderer newSpr = gObj.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
    newSpr.sprite = newSp;
    newSpr.color = RoomUtils.GetColor(JRm.program_type);

    /// collider
    PolygonCollider2D polygCll = gObj.AddComponent<PolygonCollider2D>();
    List<Vector2> polyVs = new List<Vector2>();
    foreach (Vector2 v in v2ds)
    {
      Vector2 vb = Utils.Rev2CamCoord(v, m_floor.PixelPerUnit);
      polyVs.Add(vb);
    }
    m_outline.PntsOnScreen = polyVs.ToArray();
    polygCll.points = polyVs.ToArray();
  }

  void DebugMesh(Vector2[] v2ds, int[] indices)
  {
    int count = (indices.Length+1)/3;
    int index = 0;
    
    while(true)
    {
      if(index >= count * 3)
      {
        break;
      }

      Vector2 pos = gameObject.transform.position;

      Vector2 va = v2ds[indices[index]];
      Vector2 vb = v2ds[indices[index + 1]];
      Vector2 vc = v2ds[indices[index + 2]];

      Vector2 v0 = Utils.Rvt2CamCoord(va, m_outline.BD, false, m_floor.PixelPerUnit) + pos;
      Vector2 v1 = Utils.Rvt2CamCoord(vb, m_outline.BD, false, m_floor.PixelPerUnit) + pos;
      Vector2 v2 = Utils.Rvt2CamCoord(vc, m_outline.BD, false, m_floor.PixelPerUnit) + pos;

      Debug.DrawLine(v0, v1, Color.red, 10000f, true);
      Debug.DrawLine(v1, v2, Color.red, 10000f, true);
      Debug.DrawLine(v2, v0, Color.red, 10000f,
        true);

      index = index + 3;
    }

    {
      List<Vector3> v3List = new List<Vector3>();
      List<Vector2> uv = new List<Vector2>();
      foreach(Vector2 v2 in v2ds)
      {
        float x = v2.x - transform.position.x;
        float y = v2.y - transform.position.y;
        v3List.Add(new Vector3(x, y, 0));
        uv.Add(new Vector2(x / m_outline.BD.size.x, y / m_outline.BD.size.y));
      }
      GameObject gObj = GameObject.Instantiate(m_floor.m_planeMesh);
      gObj.transform.rotation = Quaternion.identity;
      gObj.transform.position = transform.position;
      MeshFilter mFilter = gObj.GetComponent<MeshFilter>();
      Mesh oldMesh = mFilter.mesh;

      Mesh msh = new Mesh();
      mFilter.mesh = msh;

      msh.vertices = v3List.ToArray();
      msh.triangles = indices;
      msh.uv = uv.ToArray(); 
    }
  }

  void DebugOutline()
  {
    /// Draw Debug Lines
    Vector2 position2 = transform.position;
    Vector2[] v2ds = m_outline.PntsOnScreen;
    
    for (int i = 0; i < v2ds.Length; i++)
    {
      Vector2 v1 = v2ds[i];
      int j = i + 1;
      if (i == v2ds.Length - 1)
      {
        j = 0;
      }
      Vector2 v2 = v2ds[j];

      Debug.DrawLine(v1 + position2, v2 + position2, Color.green, 10000f, true);
    }
  }

  GameObject m_txtObj = null;
  bool hasTag = true;
  void DrawLable()
  {
    if (null == m_txtObj)
    {
      string tag = RoomUtils.GetRoomTag(JRm);
      if(null == tag)
      {
        hasTag = false;
        return;
      }

      m_txtObj = new GameObject("Rm_Lable+" + JRm.number, typeof(RectTransform));

      Text tx = m_txtObj.AddComponent<Text>();
      tx.text = tag;
      Font arialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
      tx.font = arialFont;
      tx.fontSize = 8;
      tx.transform.position = Vector2.zero;
      tx.color = Color.black;
      tx.horizontalOverflow = HorizontalWrapMode.Overflow;
      tx.verticalOverflow = VerticalWrapMode.Overflow;
      tx.alignment = TextAnchor.MiddleCenter;

      m_txtObj.transform.SetParent(m_floor.m_canves.transform);
      RectTransform txTrf = m_txtObj.GetComponent<RectTransform>();
      txTrf.localScale = Vector3.one;
      txTrf.sizeDelta = new Vector2(0, 0);
    }
    else
    {
      if(!hasTag)
      {
        return;
      }
    }

    Text myTx = m_txtObj.GetComponent<Text>();
    myTx.transform.position = Utils.Rvt2PixCoord(m_outline.BD.center, m_floor.FloorBDs, true, m_floor.PixelPerUnit, m_floor.m_camera, m_floor.m_canves);
  }

  // Update is called once per frame
	void Update ()
  {
    DrawLable();

    Collider2D cd =  Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    if(null == cd)
    {
      return;
    }
    
    Collider2D myCd = GetComponent(typeof(Collider2D)) as Collider2D;
    if(myCd == null)
    {
      return;
    }

    SpriteRenderer spRd = GetComponent<SpriteRenderer>();
    if (myCd == cd)
    {
      if (m_floor.HighlightRoom != null && 
        m_floor.HighlightRoom.GetInstanceID() == GetInstanceID())
      {
        return;
      }
      Debug.Log(JRm.program_type + "-" + JRm.name + "-" + JRm.number);
      m_floor.HighlightRoom = this;

      Color cl = spRd.color;
      cl.a = 0.8f;
      spRd.color = cl;
    }
    else
    {
      Color cl = spRd.color;
      cl.a = 1.0f;
      spRd.color = cl;
    }
  }
}
