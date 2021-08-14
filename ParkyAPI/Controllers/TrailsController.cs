using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _repo;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets list of trails
        /// </summary>
        /// <returns>list of trails</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        [ProducesDefaultResponseType]
        public IActionResult GetTrails()
        {
            var objList = _repo.GetTrails();
            var objDto = new List<TrailDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Gets an individual trail
        /// </summary>
        /// <param name="id">The id of the trail</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetTrail")]
        //[Authorize(Policy = "RequireAdmin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int id)
        {
            var obj = _repo.GetTrail(id);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<TrailDto>(obj);

            return Ok(objDto);
        }

        /// <summary>
        /// Gets trails by park id
        /// </summary>
        /// <param name="id">The id of the park</param>
        /// <returns></returns>
        [HttpGet("GetTrailsInNationalPark/{id:int}", Name = "GetTrailsByParkId")]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailsInNationalPark(int id)
        {
            var objList = _repo.GetTrailsInNationalPark(id);
            if (objList == null)
            {
                return NotFound();
            }

            var objListDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objListDto.Add(_mapper.Map<TrailDto>(obj));
            }

            return Ok(objListDto);
        }

        /// <summary>
        /// Creates an individual trail
        /// </summary>
        /// <param name="objDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateTrail(TrailCreateDto objDto)
        {
            if (objDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_repo.TrailExists(objDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }

            var obj = _mapper.Map<Trail>(objDto);

            if (!_repo.CreateTrail(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { id = obj.Id }, obj);
        }

        /// <summary>
        /// Updates an individual trail 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="objDto"></param>
        /// <returns></returns>
        [HttpPatch("{id:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateTrail(int id, TrailUpdateDto objDto)
        {
            if (objDto == null || id != objDto.Id)
            {
                return BadRequest(ModelState);
            }

            var obj = _mapper.Map<Trail>(objDto);

            if (!_repo.UpdateTrail(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes an individual trail 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateTrail(int id)
        {
            if (!_repo.TrailExists(id))
            {
                return NotFound();
            }

            var obj = _repo.GetTrail(id);

            if (!_repo.DeleteTrail(obj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {obj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
