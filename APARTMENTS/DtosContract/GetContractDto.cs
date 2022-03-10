using APARTMENTS.Dtos;
using APARTMENTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APARTMENTS.DtosContract
{
    public class GetContractDto
    {
        public int ApartmentId { get; set; }
        public GetApartmentDto apartment { get; set; }
    }
}
