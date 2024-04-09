using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;
using System.Net;

namespace NZWalksAPI.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }
        //create walk
        //POST: /api/walks
        [HttpPost]
        [VaildateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map Dto to Domain Model
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
            await _walkRepository.CreateAsync(walkDomainModel);

            //Map domain model to dto
            //_mapper.Map<WalkDto>(walkDomainModel);
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        //GET Walks
        //GET: /api/walks?filterOn=Name&filterQuery=Track&sortBY=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {

            var walksDomainModel = await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
            //thorw exception
            //throw new Exception("This is a new exception");
            //Map Domain to Dto
            return Ok(_mapper.Map<List<WalkDto>>(walksDomainModel));

        }

        //GET Walk by id
        //GET: /api/Walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walksDomainModel = await _walkRepository.GetByIdAsync(id);
            if (walksDomainModel == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<WalkDto>(walksDomainModel));
        }

        //Update Walk by id
        //PUT: /api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [VaildateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {

            var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);
            walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            //Map domain to Dto
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }


        //Delete a walk by id 
        //DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await _walkRepository.DeleteAsync(id);
            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<WalkDto>(deletedWalkDomainModel));
        }
    }
}
