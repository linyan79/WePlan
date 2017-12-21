using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMgr : MonoBehaviour {

  public GameObject m_emptyRoom;
  public Camera m_camera;
  public Canvas m_canves;
  public float m_pixelH;

  public GameObject m_planeMesh;

  public TextAsset JTxt;

  public string floorJson = null;

  bool m_initDone = false;

	// Use this for initialization
	void Start ()
  {
    string url = "file:///C:/inetpub/wwwroot/weihailv2.txt";
    GetTextFromWWW(url);
	}

  // Update is called once per frame
  void Update()
  {
    if (!string.IsNullOrEmpty(floorJson) && ! m_initDone)
    {
      ReadJSon();
      m_initDone = true;
    }
  }

  public Bounds FloorBDs { get; private set; }
  public float CameraPercent { get; private set; }
  public float PixelPerUnit { get; private set; }
  public Room HighlightRoom { get; set; }

  void ReadJSon()
  {
    try
    {
      string[] lines = floorJson.Split(new[] { '\r', '\n' });

      List<RoomOutline> outlines = new List<RoomOutline>();

      /// create original outline from JSon
      List<Bounds> rmBds = new List<Bounds>();
      int lineIndex = 0;
      foreach (string ln in lines)
      {
        string text = null;
        try
        {
          if (string.IsNullOrEmpty(ln))
          {
            continue;
          }
          lineIndex++;

          text = ln.Substring(1, ln.Length - 2);
          text = text.Replace("\"\"", "\"");
          int outlnStart = text.IndexOf("[[");
          int outlnEnd = text.IndexOf("]]");

          JRoom rm = JsonUtility.FromJson<JRoom>(text);
          double area = rm.area;

          /// temp filter
          if (rm.program_type == "CIRCULATE")
          {
            //continue;
          }

          string outLnStr = text.Substring(outlnStart, outlnEnd - outlnStart + 2);
          RoomOutline outline = RoomOutline.ParseOutline(outLnStr);
          outline.m_jRoom = rm;
          outlines.Add(outline);
          rmBds.Add(outline.BD);
        }
        catch
        {
          Debug.Log("JSon error on line " + lineIndex);
        }
      }

      // calculate floor Bounds and PixelPerUnit
      FloorBDs = Utils.GetBounds(rmBds);

      CameraPercent = 0.8f;
      Vector2 floorCenter = FloorBDs.center;
      Vector3 floorCenter3 = floorCenter;

      float pixW = m_camera.pixelWidth * CameraPercent;
      m_pixelH = m_camera.pixelHeight * CameraPercent;
      PixelPerUnit = Utils.GetPixelPerUnit(m_camera, pixW, m_pixelH, FloorBDs);

      Vector2 floorCorner0 = (FloorBDs.min - floorCenter3) / PixelPerUnit;
      Vector2 floorCorner1 = (FloorBDs.max - floorCenter3) / PixelPerUnit;
      Debug.DrawLine(floorCorner0, floorCorner1, Color.yellow, 10000f);

      foreach (RoomOutline outln in outlines)
      {
        /// create GameObject and set position
        GameObject gObj = new GameObject();

        /// create Room
        Room rm = gObj.AddComponent<Room>();
        rm.m_floor = this;
        rm.m_outline = outln;
      }
    }
    catch(Exception ex)
    {
      Debug.LogError(ex);
    }
  }

  void GetTextFromWWW(string url)
  {
    floorJson = JTxt.text;
    return;

    WWW www = new WWW(url);

    while (!www.isDone){ };

    if (www.error != null)
    {
      Debug.Log(www.error);
    }

    if (www.isDone)
    {
      floorJson = www.text;
    }
  }
}
