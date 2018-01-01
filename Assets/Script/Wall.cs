using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
  public JWall m_wall;
  public FloorMgr m_floor;

  static Color color_full = new Color(204f / 255f, 228f / 255f, 247f / 255f);
  static Color color_partial = new Color(0, 128f / 255f, 255f / 255f); 
  static Color color_gyp = new Color(211f / 255f, 89f / 255f, 108f / 255f);
  static Color color_other = new Color(133f / 255f, 133f / 255f, 133f / 255f);

  // Use this for initialization
  void Start()
  {
    Vector3 start = m_wall.Start.GetVct2d();
    Vector3 end = m_wall.End.GetVct2d();
    Vector3 dir = start - end;
    float length = dir.magnitude;
    float angle = Vector3.SignedAngle(dir, Vector3.right, Vector3.back);

    transform.position = Utils.Rvt2CamCoord((start + end) * 0.5f, m_floor.FloorBDs, true, m_floor.PixelPerUnit);
    SpriteRenderer sr = GetComponent<SpriteRenderer>();
    Sprite sp = sr.sprite;
    Vector3 rawSize = sp.bounds.size;

    float scaleX = length / m_floor.PixelPerUnit / rawSize.x;
    float scaleY = m_wall.Thickness / m_floor.PixelPerUnit / rawSize.x;
    transform.localScale = new Vector3(scaleX, scaleY, 1);

    switch(m_wall.WallType)
    {
      case WallTypes.FullHeight:
        sr.color = color_full;
        break;
      case WallTypes.PartialHeight:
        sr.color = color_partial;
        break;
      case WallTypes.GYP:
        sr.color = color_gyp;
        break;
      case WallTypes.Other:
      default:
        sr.color = color_other;
        break;
    }

    transform.Rotate(0, 0, angle);
  }
	
	// Update is called once per frame
	void Update ()
  {

  }
}
