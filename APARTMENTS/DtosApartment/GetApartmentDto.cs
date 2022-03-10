using APARTMENTS.DtosContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APARTMENTS.Dtos
{
    public class GetApartmentDto
    { 
        public string ApartmentDescription { get; set; }
        public int NumberOfRooms { get; set; }
        public int MonthlyPrice { get; set; }
        public ICollection<GetAdressDto> Adress { get; set; }
        public ICollection<GetContractDto> Contracts { get; set; }
    }
}
