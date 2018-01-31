using namudev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMgr : MonoBehaviour {

  public GameObject m_emptyRoom;
  public Camera m_camera;
  public Canvas m_canves;
  public float m_pixelH;

  public Font m_tagFont;

  public GameObject m_wall;
  public GameObject m_floorImg;

  public TextAsset JTxt;

  public GameObject m_desk;

  public PropertyGrid m_pGrid;

  string m_floorJson = null;


	// Use this for initialization
	void Start ()
  {
    string url = "file:///C:/dev/u3d/PlanData/ITC_6LV.json"; 
    GetTextFromWWW(url);
  }

  bool m_initialized = false;
  // Update is called once per frame
  void Update()
  {
    if (!m_initialized)
    {
      ReadJSon();
      m_initialized = true;
    }
  }

  public Bounds FloorBDs { get; private set; }
  public float CameraPercent { get; private set; }
  public float PixelPerUnit { get; private set; }
  Room m_rm;
  public Room HighlightRoom
  {
    get { return m_rm; }
    set
    {
      m_rm = value;
    }
  }

  void ReadJSon()
  {
    try
    {
      JFloor jFloor = JsonUtility.FromJson<JFloor>(m_floorJson);

      /// create original outline from JSon
      List<Bounds> rmBds = new List<Bounds>();
      foreach (JRoom jrm in jFloor.Rooms)
      {
          rmBds.Add(jrm.boundary.BD);
      }

      // calculate floor Bounds and PixelPerUnit
      Vector3 cropMin = jFloor.CropRegion.CropMin.GetVct2d();
      Vector3 cropMax = jFloor.CropRegion.CropMax.GetVct2d();
      List<Vector3> crops = new List<Vector3>();
      crops.Add(cropMin);crops.Add(cropMax);
      FloorBDs = Utils.GetBounds(crops);

      CameraPercent = 0.8f;
      Vector2 floorCenter = FloorBDs.center;
      Vector3 floorCenter3 = floorCenter;

      float pixW = m_camera.pixelWidth * CameraPercent;
      m_pixelH = m_camera.pixelHeight * CameraPercent;
      PixelPerUnit = Utils.GetPixelPerUnit(m_camera, pixW, m_pixelH, FloorBDs);

      Vector2 floorCorner0 = (FloorBDs.min - floorCenter3) / PixelPerUnit;
      Vector2 floorCorner1 = (FloorBDs.max - floorCenter3) / PixelPerUnit;
      Debug.DrawLine(floorCorner0, floorCorner1, Color.yellow, 10000f);

      {
        SpriteRenderer sr = m_floorImg.GetComponent<SpriteRenderer>();
        Sprite sp = sr.sprite;
        Vector3 rawSize = sp.bounds.size;

        float scale = (float)(cropMax.x - cropMin.x) / PixelPerUnit / rawSize.x;
        m_floorImg.transform.localScale = new Vector3(scale, scale, 1);
      }

      foreach (JRoom jrm in jFloor.Rooms)
      {
        /// create GameObject and set position
        GameObject gObj = Instantiate(m_emptyRoom);

        /// create Room
        Room rm = gObj.GetComponent<Room>();
        rm.m_floor = this;
        rm.m_jRoom = jrm; 
      }

      foreach(JWall jw in jFloor.Walls)
      {
        GameObject gObj = Instantiate(m_wall);

        Wall wa = gObj.GetComponent<Wall>();
        wa.m_floor = this;
        wa.m_wall = jw;
      }
    }
    catch(Exception ex)
    {
      Debug.LogError(ex);
    }
  }

  void GetTextFromWWW(string url)
  {
    m_floorJson = JTxt.text;
    return;

    WWW www = new WWW(url);

    while (!www.isDone){ };

    if (www.error != null)
    {
      Debug.Log(www.error);
    }

    if (www.isDone)
    {
      m_floorJson = www.text;
    }
  }
}
