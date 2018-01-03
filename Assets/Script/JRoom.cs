using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class JFurniture
{
  public JXYZ location;
  public JXYZ dir;

  public double desk_Width;

  public string FamilyName;
  public string TypeName;
}

[Serializable]
public class JRoom
{
  public string name;
  public string number;
  public string uuid;
  public string program_type;
  public double area;
  public string level;
  public JXYZ location;

  public int desk_count;
  public object table_count;
  public object internal_room_count;
  public object extra_furniture_count;
  public int soft_seating_count;
  public bool has_window;
  public List<JFurniture> furniture;

  public JMesh boundary;
}

[Serializable]
public class JFloor
{
  public List<JRoom> Rooms;// { get; set; }
  public List<JWall> Walls;
  public JFloor()
  {
  }

  public JCropRegion CropRegion;
}

[Serializable]
public class JCropRegion
{
  public JTransform Trf;
  public JXYZ CropMin;
  public JXYZ CropMax;
}

[Serializable]
public class JTransform
{
  public JXYZ BasisX;
  public JXYZ BasisY;
  public JXYZ BasisZ;
  public JXYZ Origin;
}

[Serializable]
public class JWall
{
  public WallTypes WallType;
  public float Thickness;
  public JXYZ Start;
  public JXYZ End;
}

public enum WallTypes
{
  FullHeight,
  PartialHeight,
  GYP,
  Other,
}

[Serializable]
public class JMesh
{
  public List<JXYZ> XYZList;// { get; set; }
  public List<int> Triangles;// { get; set; }

  public JMesh()
  {
    XYZList = new List<JXYZ>();
    Triangles = new List<int>();
  }

  List<Vector3> m_vct2ds = null;
  public List<Vector3> Vct2ds
  {
    get
    {
      if(null == m_vct2ds)
      {
        m_vct2ds = new List<Vector3>();
        foreach(JXYZ xyz in XYZList)
        {
          m_vct2ds.Add(xyz.GetVct2d());
        }
      }
      return m_vct2ds;
    }
  }

  bool m_hasBD = false;
  Bounds m_bd;
  public Bounds BD
  {
    get
    {
      if (!m_hasBD)
      {
        m_bd = Utils.GetBounds(Vct2ds);
      }
      return m_bd;
    }
  }
}

[Serializable]
public class JXYZ
{
  public double X;// { get; set; }
  public double Y;// { get; set; }
  public double Z;// { get; set; }

  public JXYZ(double x, double y, double z)
  {
    X = x;
    Y = y;
    Z = z;
  }

  public JXYZ() { }

  public Vector3 GetVct2d()
  {
    return new Vector3((float)X, (float)Y, 0);
  }
}
