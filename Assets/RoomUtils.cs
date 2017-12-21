using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class RoomUtils
{
  public const string PRG_WORK = "WORK";
  public const string PRG_CIRCU = "CIRCULATE";
  public const string PRG_MEET = "MEET";
  public const string PRG_SERVE = "SERVE";
  public const string PRG_OPERATE = "OPERATE";
  public const string PRG_WASH = "WASH";

  public const string WORK_PRIVATE_OFFICE = "PRIVATE OFFICE";
  public const string WORK_DD = "DEDICATED DESKS";
  public const string WORK_HOTDESK = "HOT DESKS";

  public const string CIRCU_ELEV_LOBBY = "ELEV LOBBY";
  public const string CIRCU_COMMU_STAIR = "COMMUNICATING STAIR";

  public const string MEET_MICRO_AV = "MICRO AV";
  public const string MEET_SMALL_CONV = "SMALL CONV";
  public const string MEET_SMALL_CONF = "SMALL CONF";
  public const string MEET_SMALL_AV = "SMALL AV";
  public const string MEET_MEDIUM_BRAIN = "MEDIUM BRAIN";
  public const string MEET_LARGE_AV = "LARGE AV";
  public const string MEET_PING_PONG = "PING PONG";
  public const string MEET_CLASSROOM = "CLASSROOM";
  public const string MEET_NOOK = "NOOK";
  public const string MEET_PB = "PHONE BOOTH";

  public const string SERVE_LOUNGE = "LOUNGE";
  public const string SERVE_PANTRY = "PANTRY";
  public const string SERVE_HONEST_MARKET = "HONESTY MARKET";
  public const string SERVE_COMMU_BAR = "COMMUNITY BAR";
  public const string SERVE_RECEPTION = "RECEPTION";
  public const string SERVE_RETAIL = "RETAIL";
  public const string SERVE_OUTDOOR = "OUTDOOR";
  public const string SERVE_ESPRESSO = "ESPRESSO";
  public const string SERVE_MOTHER = "MOTHER RM";
  public const string SERVE_QUIET = "QUIET RM";
  public const string SERVE_WELLNESS = "WELLNESS RM";
  public const string SERVE_GAME = "GAME RM";
  public const string SERVE_BIKE = "BIKE STOR";
  public const string SERVE_COAT = "COAT RM";
  public const string SERVE_PRINTER = "PRINTER";

  public const string WASH_WC_MEN = "MEN WC";
  public const string WASH_WC_WOMEN = "WOMEN WC";
  public const string WASH_WC_UNISEX = "UNISEX";
  public const string WASH_WC_ADA = "ADA WC";
  public const string WASH_SHOWER = "SHOWER";

  public const string OPERATE_IT = "IT";
  public const string OPERATE_JC = "JC";
  public const string OPERATE_FB = "F&B";
  public const string OPERATE_STOR = "STOR";
  public const string OPERATE_MAIL = "MAIL";
  public const string OPERATE_MGMT = "MGMT";
  public const string OPERATE_TRASH = "TRASH";

  public const string OPERATE_ELEC = "ELEC";
  public const string OPERATE_FIRE = "FIRE";
  public const string OPERATE_MECH = "MECH";

  public static Color GetColor(string prgType)
  {
    switch (prgType)
    {
      case PRG_CIRCU:
        return new Color(255f / 255f, 247f / 255f, 223f / 255f);
      case PRG_WORK:
        return new Color(171f / 255f, 221f / 255f, 231f / 255f);
      case PRG_MEET:
        return new Color(183f / 255f, 240f / 255f, 217f / 255f);
      case PRG_OPERATE:
        return new Color(226f / 255f, 226f / 255f, 226f / 255f);
      case PRG_WASH:
        return new Color(195f / 255f, 195f / 255f, 195f / 255f);
      case PRG_SERVE:
      default:
        return new Color(255f / 255f, 210f / 255f, 106f / 255f);
    }
  }

  public static string GetRoomTag(JRoom jRm)
  {
    if(jRm.program_type == PRG_WORK)
    {
      return jRm.number + "\r\n" + jRm.desk_count;
    }
    else if(jRm.program_type == PRG_MEET)
    {
      if(jRm.name != MEET_PB && jRm.name != MEET_NOOK)
      {
        return jRm.name + "\r\n" + jRm.number;
      }
    }
    else if(jRm.program_type == PRG_OPERATE)
    {
      return null;
    }
    else if(jRm.name == SERVE_PRINTER)
    {
      return null;
    }
    else
    {
      return jRm.name;
    }
    return null;
  }
}
