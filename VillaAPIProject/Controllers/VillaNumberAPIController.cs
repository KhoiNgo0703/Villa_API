using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VillaAPIProject.Data;
using VillaAPIProject.Model;
using VillaAPIProject.Model.Dto;
using VillaAPIProject.Repository.IRepository;

namespace VillaAPIProject.Controllers
{
    //create route for the api
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController: ControllerBase
    {
        //---------------------------------------------------------------------IMPLEMENTATION--------------------------------------------        //api response
        protected APIResponse _response;

        //repository
        private readonly IVillaNumberRepository _dbVillaNumber;
        //villa repository for villaID validation purpose
        private readonly IVillaRepository _dbVilla;

        //auto mapper
        private readonly IMapper _mapper;

        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, IMapper mapper, IVillaRepository dbVilla )
        {
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
            _dbVilla = dbVilla;
            this._response = new();
        }

        //---------------------------------------------------------------------GET ALL--------------------------------------------------
        //http verb
        [HttpGet]
        //Response type
        [ProducesResponseType(StatusCodes.Status200OK)]
        //Action Method
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
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
        //---------------------------------------------------------------------GET ONE---------------------------------------------------
        //http verb+ bind name and id parameter to avoid confusion
        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        //Response type
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
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
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);

                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // with no auto mapping
                // return Ok(villa);

                //automapping + no APIResponse
                //return Ok(_mapper.Map<VillaDTO>(villa));

                //automapping + API response
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
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

        //---------------------------------------------------------------------ADD-------------------------------------------------------
        //http verb
        [HttpPost]
        //Response Type
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //action method
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
        {
            try
            {
                //custom validation with repository
                if (await _dbVillaNumber.GetAsync(u => u.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("CustomError", "Villa Number is existed");
                    return BadRequest(ModelState);
                }

                //villaID validation
                if (await _dbVilla.GetAsync(u => u.Id == createDTO.VillaID) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa ID is invalid");
                    return BadRequest(ModelState);
                }

                //with automapping
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDTO);             

                //with repository
                await _dbVillaNumber.CreateAsync(villaNumber);

                //api response
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villaNumber.VillaNo }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.Message };
            }
            return _response;
        }
        //---------------------------------------------------------------------DELETE----------------------------------------------------
        //http verb
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        //type of response
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //action method
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //with repository
                var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _dbVillaNumber.RemoveAsync(villaNumber);
                
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
        //---------------------------------------------------------------------UPDATE----------------------------------------------------
        //http verb
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        //response type
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //action verb
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.VillaNo)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //villaID validation
                if (await _dbVilla.GetAsync(u => u.Id == updateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa ID is invalid");
                    return BadRequest(ModelState);
                }

                //with automapping
                VillaNumber model = _mapper.Map<VillaNumber>(updateDTO);

                //with repository
                await _dbVillaNumber.UpdateAsync(model);
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
        //---------------------------------------------------------------------PATCH-----------------------------------------------------
    }
}
