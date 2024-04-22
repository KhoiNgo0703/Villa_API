using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using VillaAPIProject.Data;
using VillaAPIProject.Model;
using VillaAPIProject.Model.Dto;
using VillaAPIProject.Repository.IRepository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VillaAPIProject.Controllers.v2
{
    //Create the route for API
    //[Route("api/[controller]")
    [Route("api/v{version:apiVersion}/VillaAPI")]
    [ApiController]
    //API version
    [ApiVersion("2.0")]
    public class VillaAPIController : ControllerBase
    {        

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
            _response = new();
        }

        //---------------------------------------------------------------------GET---------
        //Retrieve data 
        //define verb

        [HttpGet]
        
        //cache
        //[ResponseCache(Duration = 30)]
        //[ResponseCache(CacheProfileName="Default30")]
        //response
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> GetVillas([FromQuery(Name = "filterOccupancy")] int? occupancy, [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
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
                //try filter+search+pagination
                IEnumerable<Villa> villaList;
                //filter
                if (occupancy > 0)
                {
                    villaList = await _dbVilla.GetAllAsync(u => u.Occupancy == occupancy, pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                else
                {
                    villaList = await _dbVilla.GetAllAsync(pageSize: pageSize,
                         pageNumber: pageNumber);
                }
                //search
                if (!string.IsNullOrEmpty(search))
                {
                    villaList = villaList.Where(u => u.Name.ToLower().Contains(search));
                }
                //pagination
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

                //IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
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
        //cache
        //[ResponseCache(Duration = 30)]
        //Response Type
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

                //villaID input validation
                if (await _dbVilla.GetAsync(u => u.Id == id) == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessage.Add("Invalid input Id");
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
        [Authorize(Roles = "admin")]
        //Response Type
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromForm] VillaCreateDTO createDTO)
        {
            try
            {               

                //custom validation with repository
                if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessage", "Villa Name is existed");
                    return BadRequest(ModelState);
                }


                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
               
                //with automapping
                Villa villa = _mapper.Map<Villa>(createDTO);


                //with no repository
                //await _db.Villas.AddAsync(model);
                //await _db.SaveChangesAsync();

                //with repository
                await _dbVilla.CreateAsync(villa);

                if (createDTO.Image != null)
                {
                    string fileName = villa.Id + Path.GetExtension(createDTO.Image.FileName);
                    string filePath = @"wwwroot\ProductImage\" + fileName;

                    var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    FileInfo file = new FileInfo(directoryLocation);

                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                    {
                        createDTO.Image.CopyTo(fileStream);
                    }

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    villa.ImageUrl = baseUrl + "/ProductImage/" + fileName;
                    villa.ImageLocalPath = filePath;

                }
                else
                {
                    villa.ImageUrl = "https://placehold.co/600x400";
                }
                await _dbVilla.UpdateAsync(villa);

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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "admin")]
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
                //delete old photo
                if (!string.IsNullOrEmpty(villa.ImageLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), villa.ImageLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);

                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }

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
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromForm] VillaUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                //custom validation with repository
                
                if (await _dbVilla.GetAsync(u => u.Name.ToLower() == updateDTO.Name.ToLower() && u.Id != id) != null)
                {                    
                    ModelState.AddModelError("ErrorMessage", "Villa Name is existed");
                    return BadRequest(ModelState);
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

                //var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked:false) ;
                //update img
                if (updateDTO.Image != null )
                {
                    if (!string.IsNullOrEmpty(model.ImageLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), model.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);

                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    string fileName = updateDTO.Id + Path.GetExtension(updateDTO.Image.FileName);
                    string filePath = @"wwwroot\ProductImage\" + fileName;

                    var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                    {
                        updateDTO.Image.CopyTo(fileStream);
                    }

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    model.ImageUrl = baseUrl + "/ProductImage/" + fileName;
                    model.ImageLocalPath = filePath;

                }
                else
                {
                    //if (!string.IsNullOrEmpty(model.ImageUrl))
                    //{
                    //    model.ImageUrl = villa.ImageUrl;
                    //    model.ImageLocalPath = villa.ImageLocalPath;
                    //}
                    //else model.ImageUrl = "https://placehold.co/600x400";
                    model.ImageUrl = "https://placehold.co/600x400";
                }


                await _dbVilla.UpdateAsync(model);

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
