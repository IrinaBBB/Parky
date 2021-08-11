using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return _db.Trails.Include(t => t.NationalPark).FirstOrDefault(p => p.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(t => t.NationalPark).OrderBy(p => p.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            return _db.Trails.Any(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool TrailExists(int trailId)
        {
            return _db.Trails.Any(p => p.Id == trailId);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int id)
        {
            return _db.Trails.Include(t => t.NationalPark).Where(t => t.NationalParkId == id).ToList();
        }
    }
}
