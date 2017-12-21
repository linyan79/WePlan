using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RoomOutline
{
  public JRoom m_jRoom;

  const float LENGTH_EPS = 0.1f;
  const float ANGLE_EPS = 1f;

  public List<Vector2> OriginalPnts = new List<Vector2>();
  public Bounds BD;

  public Vector2[] PntsOnScreen;

  public static RoomOutline ParseOutline(string outlnStr)
  {
    RoomOutline rslt = new RoomOutline();

    string[] parts = outlnStr.Split('(');
    foreach (string part in parts)
    {
      if (!part.Contains(")"))
      {
        continue;
      }

      string vStr = part.Split(')')[0];
      Vector2 vct = new Vector2();
      string[] vStrs = vStr.Split(',');
      for (int i = 0; i < 2; i++)
      {
        string str = vStrs[i];
        float p = float.Parse(str);
        vct[i] = p;
      }
      rslt.OriginalPnts.Add(vct);
    }

    rslt.BD = Utils.GetBounds(rslt.OriginalPnts);

    /// clean Points
    rslt.CleanPnts();

    return rslt;
  }

  public void CleanPnts()
  {
    // OriginalPnts.Add(OriginalPnts[0]);
    int pntCount = OriginalPnts.Count;
    while (true)
    {
      CleanPntsA();
      CleanPntsB();
      if(pntCount == OriginalPnts.Count)
      {
        break;
      }
      pntCount = OriginalPnts.Count;
    }
  }

  void CleanPntsA()
  {
    List<int> toRemove = new List<int>();
    for (int i = 0; i < OriginalPnts.Count; i++)
    {
      int j = i + 1;
      int k = i + 2;
      if (j > OriginalPnts.Count - 1)
      {
        j = j - OriginalPnts.Count;
      }
      if (k > OriginalPnts.Count - 1)
      {
        k = k - OriginalPnts.Count;
      }

      Vector2 p0 = OriginalPnts[i];
      Vector2 p1 = OriginalPnts[j];
      Vector2 p2 = OriginalPnts[k];

      float dist = (p1 - p0).magnitude;
      if (dist < LENGTH_EPS)
      {
        toRemove.Add(j);
      }
    }

    List<Vector2> newPnts = new List<Vector2>();
    for (int i = 0; i < OriginalPnts.Count; i++)
    {
      if (toRemove.Contains(i))
      {
        continue;
      }
      newPnts.Add(OriginalPnts[i]);
    }

    OriginalPnts = newPnts;
  }

  void CleanPntsB()
  {
    List<int> toRemove = new List<int>();
    for (int i = 0; i < OriginalPnts.Count; i++)
    {
      int j = i + 1;
      int k = i + 2;
      if (j > OriginalPnts.Count - 1)
      {
        j = j - OriginalPnts.Count;
      }
      if (k > OriginalPnts.Count - 1)
      {
        k = k - OriginalPnts.Count;
      }

      Vector2 p0 = OriginalPnts[i];
      Vector2 p1 = OriginalPnts[j];
      Vector2 p2 = OriginalPnts[k];

      float angle = Vector2.SignedAngle(p2 - p1, p1 - p0);
      if (Math.Abs(angle) < ANGLE_EPS)
      {
        toRemove.Add(j);
      }
    }

    List<Vector2> newPnts = new List<Vector2>();
    for (int i = 0; i < OriginalPnts.Count; i++)
    {
      if (toRemove.Contains(i))
      {
        continue;
      }
      newPnts.Add(OriginalPnts[i]);
    }

    OriginalPnts = newPnts;
  }
}
