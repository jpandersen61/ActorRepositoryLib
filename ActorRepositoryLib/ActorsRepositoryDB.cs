using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ActorRepositoryLib
{
    public class ActorsRepositoryDB : IActorsRepository
    {
        private readonly ActorsDbContext _context;
        
        public ActorsRepositoryDB(ActorsDbContext dbContext) 
        {
            dbContext.Database.EnsureCreated();
            _context = dbContext;
        }
        public Actor Add(Actor actor)
        {
            actor.Validate();
            actor.Id = 0;
            _context.Actors.Add(actor);
            _context.SaveChanges();
            return actor;
        }

        public Actor? Delete(int id)
        {
            Actor foundActor = _context.Actors.ToList<Actor>().Find(x => x.Id == id);
            if (foundActor != null) 
            {
                _context.Actors.Remove(foundActor); 
                _context.SaveChanges();
            }
            
            return foundActor;
        }

        public IEnumerable<Actor> Get(int? birthYearBefore = null,
                                      int? birthYearAfter = null,
                                      string? orderBy = null,
                                      bool descending = false)
        {
            IQueryable<Actor> result = _context.Actors.AsQueryable();

            
            if (birthYearBefore != null)
            {
                result = result.Where<Actor>(a => a.BirthYear < birthYearBefore);
            }

            if (birthYearAfter != null)
            {
                result = result.Where<Actor>(a => a.BirthYear > birthYearAfter);
            }

            return result;
        }

        public Actor? Update(int id, Actor data)
        {
            data.Validate();
            Actor? actorToUpdate = _context.Actors.FirstOrDefault(a => a.Id == id);
            
            if (actorToUpdate != null)
            {
                actorToUpdate.Name = data.Name;
                actorToUpdate.BirthYear = data.BirthYear;

                _context.Actors.Update(actorToUpdate);
                _context.SaveChanges();
            }

            return actorToUpdate;
        }
    }
}
