using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GrpcGreeter.Models
{
  public enum Proficiency 
  {
    Beginner,
    Adept,
    Expert,
    Master 
  }

  public class SkillModel
  {
    public Guid ID { get; set; }
    public string Name { get; set; }
    public Proficiency Proficiency { get; set; }
    public Guid OwnerID { get; set; }
    public static Protos.Proficiency ConvertFromDbType(Proficiency proficiency)
    {
      return proficiency switch
      {
        Proficiency.Beginner => Protos.Proficiency.Beginner,
        Proficiency.Adept => Protos.Proficiency.Adept,
        Proficiency.Expert => Protos.Proficiency.Expert,
        Proficiency.Master => Protos.Proficiency.Master,
        _ => throw new NotSupportedException(),
      };
    }

    public static Proficiency ConvertFromProtoType(Protos.Proficiency proficiency)
    {
      return proficiency switch
      {
        Protos.Proficiency.Beginner => Proficiency.Beginner,
        Protos.Proficiency.Adept => Proficiency.Adept,
        Protos.Proficiency.Expert => Proficiency.Expert,
        Protos.Proficiency.Master => Proficiency.Master,
        _ => throw new NotSupportedException(),
      };
    }
  }
}
