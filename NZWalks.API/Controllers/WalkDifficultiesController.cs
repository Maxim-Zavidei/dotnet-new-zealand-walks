using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WalkDifficultiesController : Controller
{
    private readonly IWalkDifficultyRepository walkDifficultyRepository;
    private readonly IMapper mapper;

    public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
    {
        this.walkDifficultyRepository = walkDifficultyRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWalkDifficultiesAsync()
    {
        var walkDifficultyList = await walkDifficultyRepository.GetAllAsync();
        return Ok(mapper.Map<IEnumerable<Models.DTO.WalkDifficulty>>(walkDifficultyList));
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ActionName("GetWalkDifficultyAsync")]
    public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
    {
        var walkDifficulty = await walkDifficultyRepository.GetAsync(id);

        if (walkDifficulty == null)
        {
            return NotFound();
        }

        var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

        return Ok(walkDifficultyDTO);
    }

    [HttpPost]
    public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest request)
    {
        // Validate the incoming request:
        if (!ValidateAddWalkDifficultyAsync(request))
        {
            return BadRequest(ModelState);
        }

        // Convert DTO to Domain model:
        var walkDifficultyDomain = new Models.Domain.WalkDifficulty
        {
            Code = request.Code
        };

        // Call repository:
        walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

        // Convert Domain to DTO:
        var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

        // Return response:
        return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateWalkDifficultyAsync(Guid id, UpdateWalkDifficultyRequest request)
    {
        // Validate the incoming request:
        if (!ValidateUpdateWalkDifficultyAsync(request))
        {
            return BadRequest(ModelState);
        }

        // Convert DTO to Domain model:
        var walkDifficultyDomain = new Models.Domain.WalkDifficulty
        {
            Code = request.Code
        };

        // Call repository to update:
        walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

        if (walkDifficultyDomain == null)
        {
            return NotFound();
        }

        // Convert Domain to DTO:
        var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

        // Return response:
        return Ok(walkDifficultyDTO);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
    {
        var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);

        if (walkDifficultyDomain == null)
        {
            return NotFound();
        }

        var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

        return Ok(walkDifficultyDTO);
    }

    #region Validations

    private bool ValidateAddWalkDifficultyAsync(AddWalkDifficultyRequest request)
    {
        if (request == null)
        {
            ModelState.AddModelError(nameof(request), $"Request can not be empty.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            ModelState.AddModelError(nameof(request.Code), $"{nameof(request.Code)} must not be null or empty or whitespace.");
        }

        if (ModelState.ErrorCount > 0)
        {
            return false;
        }

        return true;
    }

    private bool ValidateUpdateWalkDifficultyAsync(UpdateWalkDifficultyRequest request)
    {
        if (request == null)
        {
            ModelState.AddModelError(nameof(request), $"Request can not be empty.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            ModelState.AddModelError(nameof(request.Code), $"{nameof(request.Code)} must not be null or empty or whitespace.");
        }

        if (ModelState.ErrorCount > 0)
        {
            return false;
        }

        return true;
    }

    #endregion
}
