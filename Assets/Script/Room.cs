using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
  public FloorMgr m_floor;
  public JRoom m_jRoom;

	// Use this for initialization
	void Start ()
  {
    try
    {
      Setup();
      //m_floor.m_canves.referencePixelsPerUnit = m_floor.PixelPerUnit;
    }
    catch(Exception ex)
    {
      Debug.LogError(ex);
    }
  }

  public Material WhiteMaker;
  public Material PostOutline;

  void Setup()
  {
    /// set position
    GameObject gObj = gameObject;
    JMesh jMsh = m_jRoom.boundary;
    // Vector2 pos = Utils.Rvt2CamCoord(jMsh.BD.min, m_floor.FloorBDs, true, m_floor.PixelPerUnit);
    gObj.transform.position = new Vector3(0, 0, 5);

    /// calculate mesh

    /// reshape
    List<Vector3> v2ds = Utils.Rvt2CamCoord(jMsh.Vct2ds, m_floor.FloorBDs, true, m_floor.PixelPerUnit);
    List<Vector2> uv = new List<Vector2>();
    for(int i = 0; i<v2ds.Count;i++)
    {
      uv.Add(new Vector2((float)i / (float)v2ds.Count * jMsh.BD.size.x, (float)i / (float)v2ds.Count * jMsh.BD.size.y));
    }
    MeshFilter mFilter = gObj.GetComponent<MeshFilter>();
    Mesh oldMesh = mFilter.mesh;

    Mesh msh = new Mesh();
    mFilter.mesh = msh;

    msh.vertices = v2ds.ToArray();
    jMsh.Triangles.Reverse();
    msh.triangles = jMsh.Triangles.ToArray();
    msh.uv = uv.ToArray();

    MeshCollider msCll = gObj.GetComponent<MeshCollider>();
    msCll.sharedMesh = msh;

    MeshRenderer mshRender = GetComponent<MeshRenderer>();
    mshRender.material.color = RoomUtils.GetColor(m_jRoom.program_type);

    foreach(JFurniture fn in m_jRoom.furniture)
    {
      GameObject dskObj = GameObject.Instantiate(m_floor.m_desk);
      Desk dsk = dskObj.GetComponent<Desk>();
      dsk.m_furniture = fn;
      dsk.m_rm = this;
    }
  }

  void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    Camera cam = Camera.main;
    RenderTexture TempRT = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 24);
    RenderTexture whiteRT = new RenderTexture(TempRT.width, TempRT.height, TempRT.depth);

    Graphics.Blit(source, whiteRT, WhiteMaker);
    Graphics.Blit(whiteRT, destination, PostOutline);
  }

  GameObject m_txtObj = null;
  bool hasTag = true;
  void DrawLable()
  {
    if (null == m_txtObj)
    {
      string tag = RoomUtils.GetRoomTag(m_jRoom);
      if(null == tag)
      {
        hasTag = false;
        return;
      }

      m_txtObj = new GameObject("Rm_Lable+" + m_jRoom.number, typeof(RectTransform));

      Text tx = m_txtObj.AddComponent<Text>();
      tx.text = tag;
      tx.font = m_floor.m_tagFont;
      tx.fontSize = 8;
      tx.rectTransform.position = Vector2.zero;
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
    myTx.transform.position = Utils.Rvt2PixCoord(m_jRoom.location.GetVct2d(), m_floor.FloorBDs, true, m_floor.PixelPerUnit, m_floor.m_camera, m_floor.m_canves);
  }

  // Update is called once per frame
  void Update ()
  {
    DrawLable();

    RaycastHit hitInfo;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    bool hit = Physics.Raycast(ray, out hitInfo);
    if(!hit)
    {
      return;
    }

    Console.WriteLine("Hit");

    Collider myCd = GetComponent<MeshCollider>();
    if(myCd == null)
    {
      return;
    }
    
    MeshRenderer mshRender = GetComponent<MeshRenderer>();
    if (myCd == hitInfo.collider)
    {
      if (m_floor.HighlightRoom != null && 
        m_floor.HighlightRoom.GetInstanceID() == GetInstanceID())
      {
        return;
      }
      m_floor.HighlightRoom = this;     

      Color cl = mshRender.material.color;
      cl.a = 0.6f;
      mshRender.material.color = cl;
    }
    else
    {
      Color cl = mshRender.material.color;
      cl.a = 0.8f;
      mshRender.material.color = cl;
    }
  }
}
