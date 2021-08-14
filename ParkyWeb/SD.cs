using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb
{
    public static class SD
    {
        public static string APIBaseUrl = "https://localhost:5001/api/"; 
        public static string NationalParkAPIPath = APIBaseUrl + "nationalparks/"; 
        public static string TrailAPIPath = APIBaseUrl + "trails/"; 
        public static string RegisterAPIPath = APIBaseUrl + "users/register"; 
        public static string AuthenticateAPIPath = APIBaseUrl + "users/authenticate"; 
    }
}
