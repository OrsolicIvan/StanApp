using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APARTMENTS.Models;
using APARTMENTS.Interfaces;
using APARTMENTS.Dtos;
using APARTMENTS.DtosContract;
using AutoMapper;
using APARTMENTS.Extensions;
using APARTMENTS.DtosPhoto;
using APARTMENTS.DtosUser;
using AutoMapper.QueryableExtensions;

namespace APARTMENTS.Controller
{
    
    [Route("api/[controller]")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly Context _context;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, Context context, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context;
            _photoService = photoService;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUsers()
        {
            return await _context.Users.ProjectTo<GetUserDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            
        }

        



        
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{
        //    var user = await _context.Users.Where(i => i.Id == id).Include(p => p.Photos).Include(oa => oa.OwnedApartments).Include(c => c.RentedApartments).ThenInclude(ap => ap.Apartment).SingleOrDefaultAsync();

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}
        //[HttpGet("{username}", Name = "GetUser")]
        //public async Task<ActionResult<MemberDto>> GetUSer(string username) 
        //{
        //    return await _userRepository.GetMemberAsync(username);
        //}




        [HttpPut("UserDetails")]
        public async Task<ActionResult<User>> AddDetails(UserDetailDto userDetail)
        {
            var user = await _context.Users.Where(i => i.Id == userDetail.Id).SingleOrDefaultAsync();
            if (user != null)
            {
                user.Email = userDetail.Email;
                user.PhoneNumber = userDetail.PhoneNumber;
                await _context.SaveChangesAsync();
            }
            else return NotFound();
            return Ok(user);
        }



        [HttpPost("ApartmentAdress")]
        public async Task<ActionResult<Adress>> AddAdressToApartment(CreateAdressDto adress)
        {
            var apartment = await _context.Apartments
                .Include(x => x.Adress)
                .SingleOrDefaultAsync(x => x.Id == adress.ApartmentId);
            if (apartment == null) 
                return NotFound();
            var newAdress = new Adress
            {
                City = adress.City,
                Apartment = apartment,
                BuildingNumber = adress.BuildingNumber,
                ApartmentNumber = adress.ApartmentNumber,
                ApartmentId = adress.ApartmentId
            };
            
            apartment.Adress.Add(newAdress);
            _context.Adresses.Add(newAdress);
            await _context.SaveChangesAsync();

            return Ok();
        }
        //HttpPost metoda kojom osoba koja je vlasnik stan taj stan objavi 
        [HttpPost("OwnedApartment")]
        public async Task<ActionResult<User>> AddOwnedApartment(CreateApDto ownedApartment)
        {
            //var apartment = await _context.Apartments.FindAsync(contract.ApartmentId);
            //if (apartment == null)
            //    return NotFound();
            var user = await _context.Users
                .Include(x => x.OwnedApartments)
                .SingleOrDefaultAsync(x => x.Id == ownedApartment.OwnerId);
            
            if (user == null)
                return NotFound();
            var newApartment = new Apartment
            {
                ApartmentDescription = ownedApartment.ApartmentDescription,
                Owner = user,
                NumberOfRooms = ownedApartment.NumberOfRooms,
                MonthlyPrice = ownedApartment.MonthlyPrice,
                Contracts = null
            };
            
            user.OwnedApartments.Add(newApartment);
            _context.Apartments.Add(newApartment);
            
            await _context.SaveChangesAsync();
            return Ok();
        }


        //HttpPost metoda za stvaranje kontrakta tj da osoba moze kupiti stan
        [HttpPost("contract")]
        public async Task<ActionResult<User>> AddContract(AddContract contract)
        {
            var apartment = await _context.Apartments
                .Include(x => x.Contracts)
                .SingleOrDefaultAsync(x => x.Id == contract.ApartmentId);
            if (apartment == null)
                return NotFound();
            var user = await _context.Users
                .Include(x => x.RentedApartments)
                .SingleOrDefaultAsync(x => x.Id == contract.UserId);

            if (user == null)
                return NotFound();
            var newContract = new Contract
            {
                UserId = user.Id,
                User = user,
                ApartmentId = apartment.Id,
                Apartment = apartment
                
            };

            user.RentedApartments.Add(newContract);
            apartment.Contracts.Add(newContract);
           

            _context.Contracts.Add(newContract);
            await _context.SaveChangesAsync();

            return Ok();
        }



        [HttpPost("add-photo")]
        public async Task<ActionResult<List<PhotoDto>>> AddPhoto(IFormFile file)
        {   
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            
            user.Photos.Add(photo);


            if (await _userRepository.SaveAllAsync()) {
                
                return CreatedAtRoute("GetUser", new {username = user.UserName} , _mapper.Map<PhotoDto>(photo));
                
            }
                
            return BadRequest("problem adding photo");
        }



    }
}
