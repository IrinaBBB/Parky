using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _db;

        public NationalParkRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public bool CreateNationalPark(NationalParkDto nationalPark)
        {
            _db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalParkDto nationalPark)
        {
            _db.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalParkDto GetNationalPark(int nationalParkId)
        {
            return _db.NationalParks.FirstOrDefault(p => p.Id == nationalParkId);
        }

        public ICollection<NationalParkDto> GetNationalParks()
        {
            return _db.NationalParks.OrderBy(p => p.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            return _db.NationalParks.Any(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool NationalParkExists(int nationalParkId)
        {
            return _db.NationalParks.Any(p => p.Id == nationalParkId);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(NationalParkDto nationalPark)
        {
            _db.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
