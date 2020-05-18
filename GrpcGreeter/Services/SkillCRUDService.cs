using Grpc.Core;
using GrpcGreeter.Models;
using GrpcGreeter.Protos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Services
{
  public class SkillCRUDService : SkillCRUD.SkillCRUDBase
  {
    private readonly AppDbContext db = null;
    public SkillCRUDService(AppDbContext db)
    {
      this.db = db;
    }
    public override Task<Skill> GetByID(SkillFilter request, ServerCallContext context)
    {
      var data = db.Skills.FirstOrDefault(p => p.ID == Guid.Parse(request.Id));
      if (data == null)
        throw new ArgumentNullException();

      var skill = new Skill
      {
        Id = data.ID.ToString(),
        Name = data.Name,
        Proficiency = SkillModel.ConvertFromDbType(data.Proficiency),
        OwnerId = data.OwnerID.ToString()
      };
      return Task.FromResult(skill);

    }

    public override Task<Skills> GetSkills(Empty request, ServerCallContext context)
    {
      var skills = new Skills();
      var query = from s in db.Skills
                  select new Skill
                  {
                    Id = s.ID.ToString(),
                    Name = s.Name,
                    Proficiency = SkillModel.ConvertFromDbType(s.Proficiency),
                    OwnerId = s.OwnerID.ToString()
                  };
      skills.Items.AddRange(query.ToArray());
      return Task.FromResult(skills);
    }

    public override Task<Empty> Insert(Skill request, ServerCallContext context)
    {
      if (db.Skills.FirstOrDefault(s => s.Name == request.Name) != null)
        throw new ArgumentException("Skill already exists");

      db.Skills.Add(new SkillModel
      {
        ID = Guid.Parse(request.Id),
        Name = request.Name,
        Proficiency = SkillModel.ConvertFromProtoType(request.Proficiency),
        OwnerID = Guid.Parse(request.OwnerId)
      });
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Update(Skill request, ServerCallContext context)
    {
      db.Skills.Update(new SkillModel
      {
        ID = Guid.Parse(request.Id),
        Name = request.Name,
        Proficiency = SkillModel.ConvertFromProtoType(request.Proficiency),
        OwnerID = Guid.Parse(request.OwnerId)
      });
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }

    public override Task<Empty> Delete(SkillFilter request, ServerCallContext context)
    {
      var skill = db.Skills.FirstOrDefault(s => s.ID == Guid.Parse(request.Id));
      if (skill == null)
        throw new ArgumentNullException();

      db.Skills.Remove(skill);
      db.SaveChanges();
      return Task.FromResult(new Empty());
    }
  }
}
