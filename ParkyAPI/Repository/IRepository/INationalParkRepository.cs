using ParkyAPI.Models;
using System.Collections.Generic;

namespace ParkyAPI.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalParkDto> GetNationalParks();

        NationalParkDto GetNationalPark(int nationalParkId);

        bool NationalParkExists(string name);

        bool NationalParkExists(int nationalParkId);

        bool CreateNationalPark(NationalParkDto nationalPark);

        bool UpdateNationalPark(NationalParkDto nationalPark);

        bool DeleteNationalPark(NationalParkDto nationalPark);

        bool Save();
    }
}
