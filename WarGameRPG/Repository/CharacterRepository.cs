using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGameRPG.DataAccesLayer;
using WarGameRPG.Models;

namespace WarGameRPG.Repository
{
    internal class CharacterRepository : IGeneric<Character>
    {
        dataContext db = new dataContext();
        public bool Add(Character entity)
        {
            bool status = false;
            try
            {
                db.Character.Add(entity);
                db.SaveChanges();
                status = true;
            }
            catch
            {
                status = false;
            }
            return status;
        }

        public bool Edit(Character obj)
        {
            bool status = false;
            Character character = db.Character.Find(obj.Id);
            if (character != null)
            {
                character.Name = obj.Name;
                character.Power = obj.Power;
                character.Durability = obj.Durability;
                character.Health = obj.Health;
                character.Speed = obj.Speed;
                character.Score = obj.Score;
                character.Level = obj.Level;
                db.SaveChanges();
            }
            return status;
        }
    }
}
