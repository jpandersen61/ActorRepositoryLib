using System;
using System.Collections.Generic;
using System.Linq;
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
            throw new NotImplementedException();
        }

        public IEnumerable<Actor> Get(int? birthYearBefore = null, int? birthYearAfter = null)
        {
            IQueryable<Actor> query = _context.Actors.AsQueryable();
            return query; 
        }

        public Actor? Update(int id, Actor data)
        {
            throw new NotImplementedException();
        }
    }
}
