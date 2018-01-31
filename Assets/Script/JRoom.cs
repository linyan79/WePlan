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

public class PropertyData
{ 
    public string PropertyName { get; set; }
    public string PropertyValueString { get; set; }
    public int PropertyValueDouble { get; set; }
    public Type PropertyType { get; set; }

    public object GeneralValue
    {
        get
        {
            if(PropertyType == typeof(int))
            {
                return PropertyValueDouble;
            }
            else if(PropertyType == typeof(string))
            {
                return PropertyValueString;
            }

            return PropertyValueString;
        }
        set
        {
            if (PropertyType == typeof(int))
            {
                PropertyValueDouble = (int)value;
            }
            else if (PropertyType == typeof(string))
            {
                PropertyValueString = (string)value;
            }
            else
            {
                PropertyValueString = (string)value;
            }
        }
    }

    public PropertyData(string pName, string pValue)
    {
        PropertyName = pName;
        PropertyValueString = pValue;
        PropertyType = pValue.GetType();
    }

    public PropertyData(string pName, int pValue)
    {
        PropertyName = pName;
        PropertyValueDouble = pValue;
        PropertyType = pValue.GetType();
    }
}

[Serializable]
public class JRoom
{
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    public string Number
    {
        get
        {
            return number;
        }
        set
        {
            number = value;
        }
    }

    public int Desk_Count { get { return desk_count; } set { desk_count = value; } }

    List<PropertyData> m_propertyList = null;
    public List<PropertyData> PropertyList
    {
        get
        {
            if (null == m_propertyList)
            {
                m_propertyList = new List<PropertyData>();
                m_propertyList.Add(new PropertyData("Name", name));
                m_propertyList.Add(new PropertyData("Number", number));
                m_propertyList.Add(new PropertyData("Desk_Count", desk_count));
            }
            return m_propertyList;
        }
    }

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
            if (null == m_vct2ds)
            {
                m_vct2ds = new List<Vector3>();
                foreach (JXYZ xyz in XYZList)
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
