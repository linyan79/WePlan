using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAdjust : MonoBehaviour {

  Camera m_cam;
  float m_scale_speed = 100;
  float m_move_speed = 100;

  // Use this for initialization
  void Start()
  {
    m_cam = GetComponent<Camera>();
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKey(KeyCode.PageUp)) // Change From Q to anyother key you want
    {
      m_cam.orthographicSize = m_cam.orthographicSize - m_scale_speed * Time.deltaTime;
      if (m_cam.orthographicSize > 600)
      {
        m_cam.orthographicSize = 600; // Max size
      }
    }


    if (Input.GetKey(KeyCode.PageDown)) // Also you can change E to anything
    {
      m_cam.orthographicSize = m_cam.orthographicSize + m_scale_speed * Time.deltaTime;
      if (m_cam.orthographicSize < 100)
      {
        m_cam.orthographicSize = 100; // Min size 
      }
    }

    if(Input.GetKey(KeyCode.UpArrow))
    {
      transform.position = transform.position + new Vector3(0, -Time.deltaTime * m_move_speed);
    }
    else if (Input.GetKey(KeyCode.DownArrow))
    {
      transform.position = transform.position + new Vector3(0, Time.deltaTime * m_move_speed);
    }
    else if (Input.GetKey(KeyCode.LeftArrow))
    {
      transform.position = transform.position + new Vector3(Time.deltaTime * m_move_speed, 0);
    }
    else if (Input.GetKey(KeyCode.RightArrow))
    {
      transform.position = transform.position + new Vector3(-Time.deltaTime * m_move_speed, 0);
    }
  }
}
