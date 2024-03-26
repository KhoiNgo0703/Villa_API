using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VillaAPIProject.Data;
using VillaAPIProject.Model;
using VillaAPIProject.Model.Dto;
using VillaAPIProject.Repository.IRepository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VillaAPIProject.Controllers
{
    //Create the route for API
    //[Route("api/[controller]")
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        //---------------------------------------DEPENDENCY INJECTION IMPLEMENTATION WITHOUT REPOSITORY-----
        //logger implementation
        //private readonly ILogger<VillaAPIController> _logger;

        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //{
        //    _logger = logger;
        //}

        //dbContext implementation and not using repository

        //private readonly ApplicationDbContext _db;
        //
        ////automapper implementation
        //private readonly IMapper _mapper;

        //dbContext implementation and not using repository
        //public VillaAPIController(ApplicationDbContext db, IMapper mapper)
        //{

        //    _db = db;
        //    _mapper=mapper;
        //}


        //---------------------------------------------------------------------IMPLEMENTATION----
        //api response
        protected APIResponse _response;

        //use repository
        private readonly IVillaRepository _dbVilla;

        //automapper implementation
        private readonly IMapper _mapper;

        public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
        {
            _dbVilla = dbVilla;
            _mapper = mapper;
            this._response = new();
        }

        //---------------------------------------------------------------------GET---------
        //Retrieve data 
        //define verb
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            //_logger.LogInformation("Getting all the villas");
            // not using AutoMapper way
            // return Ok(await _db.Villas.ToListAsync());

            //automapper way + no repository
            //IEnumerable<Villa> villaList= await _db.Villas.ToListAsync();   
            //return Ok(_mapper.Map<List<VillaDTO>>(villaList));    

            //automapper way + repository+ no API response
            //IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
            //return Ok(_mapper.Map<List<VillaDTO>>(villaList));

            //automapper way + repository +API response
            try
            {
                IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.Message };
            }
            return _response;
        }


        //add parameter to avoid confusion
        [HttpGet("{id:int}", Name = "GetVilla")]
        //Response Type
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type=typeof(VillaDTO))]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                //check validation
                if (id == 0)
                {
                    //_logger.LogError("Get villa error with id" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //no repository
                //var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);

                //repository
                var villa = await _dbVilla.GetAsync(u => u.Id == id);

                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // with no auto mapping
                // return Ok(villa);

                //automapping + no APIResponse
                //return Ok(_mapper.Map<VillaDTO>(villa));

                //automapping + API response
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.Message };
            }
            return _response;


        }
        //---------------------------------------------------------------------ADD----

        //ADD
        [HttpPost]
        //Response Type
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {
            try
            {
                //Validation without [ApiController]
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}

                //custom validation with no repository
                //if (await _db.Villas.FirstOrDefaultAsync(u=>u.Name.ToLower()== createDTO.Name.ToLower())!=null)
                //{
                //    ModelState.AddModelError("CustomError", "Villa Name is existed");
                //    return BadRequest(ModelState);
                //}

                //custom validation with repository
                if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("CustomError", "Villa Name is existed");
                    return BadRequest(ModelState);
                }


                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                //Id of the villa needs to be zero to add a new one
                //if (villaDTO.Id > 0) 
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError);
                //}
                //increase the value of ID by 1
                //villaDTO.Id= VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id+1;

                //add Data
                //VillaStore.villaList.Add(villaDTO);

                //add data to database

                //without automapping
                //Villa model = new()
                //{
                //    //Id = villaDTO.Id, No need since using different DTO class
                //    Name = createDTO.Name,
                //    Details = createDTO.Details,
                //    ImageUrl = createDTO.ImageUrl,
                //    Occupancy = createDTO.Occupancy,
                //    Rate = createDTO.Rate,
                //    Sqft = createDTO.Sqft,
                //    Amenity = createDTO.Amenity
                //};

                //with automapping
                Villa villa = _mapper.Map<Villa>(createDTO);


                //with no repository
                //await _db.Villas.AddAsync(model);
                //await _db.SaveChangesAsync();

                //with repository
                await _dbVilla.CreateAsync(villa);

                //no api response
                //return CreatedAtRoute("GetVilla",new { id = model.Id },model); 

                //api response
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.Message };
            }
            return _response;
        }
        //---------------------------------------------------------------------DELETE-------
        //Delete        
        //Response Type
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //with no repository
                //var villa=await _db.Villas.FirstOrDefaultAsync(u=> u.Id==id);
                //if (villa==null)
                //{
                //    return NotFound();
                //}
                //_db.Villas.Remove(villa);
                //await _db.SaveChangesAsync();

                //with repository
                var villa = await _dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _dbVilla.RemoveAsync(villa);

                //use IActionResult
                //return NoContent();

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.Message };
            }
            return _response;
        }
        //---------------------------------------------------------------------UPDATE-----
        //Update
        //Response Type
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //var villa= VillaStore.villaList.FirstOrDefault(u=>u.Id==id);
                //villa.Name=villaDTO.Name;
                //villa.Sqft=villaDTO.Sqft;
                //villa.Occupancy=villaDTO.Occupancy;

                //without automapping
                //Villa model = new()
                //{
                //    Id = updateDTO.Id,
                //    Name = updateDTO.Name,
                //    Details = updateDTO.Details,
                //    ImageUrl = updateDTO.ImageUrl,
                //    Occupancy = updateDTO.Occupancy,
                //    Rate = updateDTO.Rate,
                //    Sqft = updateDTO.Sqft,
                //    Amenity = updateDTO.Amenity
                //};

                //with automapping
                Villa model = _mapper.Map<Villa>(updateDTO);

                //with no repository
                //_db.Villas.Update(model);
                //await _db.SaveChangesAsync();

                //with repository
                await _dbVilla.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.Message };
            }
            return _response;
        }
        //---------------------------------------------------------------------PATCH-------
        //Patch
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        //Response Type
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            //with no repository
            //for patch need as no tracking so it will not track the name
            //var villa=await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u=>u.Id == id);

            //with repository
            var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);

            //without auto mapping
            //VillaUpdateDTO villaDTO = new()
            //{
            //    Id = villa.Id,
            //    Name = villa.Name,
            //    Details = villa.Details,
            //    ImageUrl = villa.ImageUrl,
            //    Occupancy = villa.Occupancy,
            //    Rate = villa.Rate,
            //    Sqft = villa.Sqft,
            //    Amenity = villa.Amenity
            //};
            //

            ////auto mapping
            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);

            //without automapping
            //Villa model = new Villa()
            //{
            //    Id = villaDTO.Id,
            //    Name = villaDTO.Name,
            //    Details = villaDTO.Details,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft,
            //    Amenity = villaDTO.Amenity
            //};

            //automapping
            Villa model = _mapper.Map<Villa>(villaDTO);


            //no repository
            //_db.Villas.Update(model);
            //await _db.SaveChangesAsync();

            //repository
            await _dbVilla.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

    }
}
