using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class JFurniture
{
  public string name;
  public string uuid;
  public string level;
  public int count;
  public string type;
  public string location ;
  public string category ;
  public object type_mark ;
  public object we_work_sku ;
  public object number ;
}

[Serializable]
public class JRoom
{
  public string name ;
  public string number ;
  public string uuid ;
  public string program_type ;
  public double area ;
  public string level ;
  public string location ;
  public List<List<string>> outline ;
  public int desk_count ;
  public object table_count ;
  public object internal_room_count ;
  public object extra_furniture_count ;
  public int soft_seating_count ;
  public bool has_window ;
  public List<JFurniture> furniture ;
}
