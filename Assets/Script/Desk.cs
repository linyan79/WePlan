using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour
{
  public JFurniture m_furniture;
  public Room m_rm;

	// Use this for initialization
	void Start ()
  {
    transform.position = Utils.Rvt2CamCoord(m_furniture.location.GetVct2d(), m_rm.m_floor.FloorBDs, true, m_rm.m_floor.PixelPerUnit);
    SpriteRenderer sr = GetComponent<SpriteRenderer>();
    Sprite sp = sr.sprite;
    Vector3 rawSize = sp.bounds.size;

    float scale = (float)m_furniture.desk_Width / m_rm.m_floor.PixelPerUnit / rawSize.x;
    transform.localScale = new Vector3(scale, scale, 1);

    float angle = Vector3.SignedAngle(m_furniture.dir.GetVct2d(), Vector3.right, Vector3.back);
    transform.Rotate(0, 0, angle);
  }
	
	// Update is called once per frame
	void Update ()
  {

  }
}
