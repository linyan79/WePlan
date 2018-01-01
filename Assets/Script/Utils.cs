using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Utils
{
  public static ushort[] ToUShort(int[] trs)
  {
    List<ushort> rslt = new List<ushort>();
    foreach (int tr in trs)
    {
      rslt.Add((ushort)tr);
    }
    return rslt.ToArray();
  }

  /// RVT Coordinate        - RvtCoord
  /// RVT Center Coordinate - RevCoord
  /// Sprite Coordinate     - SprCoord
  /// Camera Coordinate     - CamCoord
  /// Pixel Coordinate      - PixCoord
  
  public static Vector2 Rvt2RevCoord(Vector2 v, Bounds bd, bool floorOrSprite)
  {
    Vector2 anch = floorOrSprite ? bd.center : bd.min;
    return v - anch;
  }

  public static Vector3 Rvt2RevCoord(Vector3 v, Bounds bd, bool floorOrSprite)
  {
    Vector3 anch = floorOrSprite ? bd.center : bd.min;
    return v - anch;
  }

  public static List<Vector2> Rvt2RevCoord(ICollection<Vector2> vs, Bounds bd, bool floorOrSprite)
  {
    List<Vector2> rslt = new List<Vector2>();
    foreach(Vector2 v in vs)
    {
      rslt.Add(Rvt2RevCoord(v, bd, floorOrSprite));
    }
    return rslt;
  }

  public static List<Vector3> Rvt2RevCoord(ICollection<Vector3> vs, Bounds bd, bool floorOrSprite)
  {
    List<Vector3> rslt = new List<Vector3>();
    foreach (Vector3 v in vs)
    {
      rslt.Add(Rvt2RevCoord(v, bd, floorOrSprite));
    }
    return rslt;
  }

  public static Vector2 Cam2PixCoord(Vector2 v, Camera cam, Canvas canv)
  {
    Vector3 scale = canv.transform.localScale;
    Vector3 pos = new Vector3(v.x * scale.x, v.y * scale.y, 0);
    return pos * cam.pixelHeight / (cam.orthographicSize * 2f);
  }

  public static Vector3 Cam2PixCoord(Vector3 v, Camera cam, Canvas canv)
  {
    Vector3 scale = canv.transform.localScale;
    Vector3 pos = new Vector3(v.x * scale.x, v.y * scale.y, 0);
    return pos * cam.pixelHeight / (cam.orthographicSize * 2f);
  }

  public static Vector2 Rev2CamCoord(Vector2 v, float pixelPerUnit)
  {
    return v / pixelPerUnit;
  }

  public static Vector3 Rev2CamCoord(Vector3 v, float pixelPerUnit)
  {
    return v / pixelPerUnit;
  }

  public static Vector2 Rvt2CamCoord(Vector2 v, Bounds bd, bool floorOrSprite, float pixelPerUnit)
  {
    return Rev2CamCoord(Rvt2RevCoord(v, bd, floorOrSprite), pixelPerUnit);
  }

  public static Vector3 Rvt2CamCoord(Vector3 v, Bounds bd, bool floorOrSprite, float pixelPerUnit)
  {
    return Rev2CamCoord(Rvt2RevCoord(v, bd, floorOrSprite), pixelPerUnit);
  }

  public static List<Vector3> Rvt2CamCoord(List<Vector3> vs, Bounds bd, bool floorOrSprite, float pixelPerUnit)
  {
    List<Vector3> rslt = new List<Vector3>();
    foreach(Vector3 v in vs)
    {
      rslt.Add(Rvt2CamCoord(v, bd, floorOrSprite, pixelPerUnit));
    }
    return rslt;
  }

  public static Vector2 Rvt2PixCoord(Vector2 v, Bounds bd, bool floorOrSprite, float pixelPerUnit, Camera cam, Canvas canv)
  {
    return Cam2PixCoord(Rvt2CamCoord(v, bd, floorOrSprite, pixelPerUnit), cam, canv);
  }

  public static Vector3 Rvt2PixCoord(Vector3 v, Bounds bd, bool floorOrSprite, float pixelPerUnit, Camera cam, Canvas canv)
  {
    return Cam2PixCoord(Rvt2CamCoord(v, bd, floorOrSprite, pixelPerUnit), cam, canv);
  }

  public static float GetPixelPerUnit(Camera cam, float pixW, float pixH, Bounds rvtBd)
  {
    float floorH = rvtBd.size.y;
    float floorW = rvtBd.size.x;
    float percent = pixH / cam.pixelHeight;
    float cameraSize = cam.orthographicSize * percent * 2f;
    if (floorW / floorH > (float)cam.pixelWidth / (float)cam.pixelHeight)
    {
      cameraSize = (cameraSize * (float)cam.pixelWidth / (float)cam.pixelHeight) * floorH / floorW;
    }
    return floorH / cameraSize;
  }

  public static Bounds GetBounds(List<Vector2> vs)
  {
    Bounds tmpBd = new Bounds(vs[0], Vector2.zero);
    foreach (Vector2 v in vs)
    {
      tmpBd.Encapsulate(v);
    }
    return tmpBd;
  }

  public static Bounds GetBounds(List<Vector3> vs)
  {
    Bounds tmpBd = new Bounds(vs[0], Vector2.zero);
    foreach (Vector2 v in vs)
    {
      tmpBd.Encapsulate(v);
    }
    return tmpBd;
  }

  public static Bounds GetBounds(List<Bounds> bds)
  {
    Bounds tmpBd = bds[0];
    foreach(Bounds bd in bds)
    {
      tmpBd.Encapsulate(bd);
    }
    return tmpBd;
  }
}

