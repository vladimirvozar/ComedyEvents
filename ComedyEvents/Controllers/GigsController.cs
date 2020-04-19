using AutoMapper;
using ComedyEvents.DTO;
using ComedyEvents.Models;
using ComedyEvents.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComedyEvents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GigsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public GigsController(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }


        [HttpGet("searchByEvent")]
        public async Task<ActionResult<GigDto[]>> GetGigsByEvent(int eventId, bool includeComedians = false)
        {
            try
            {
                var result = await _eventRepository.GetGigsByEvent(eventId, includeComedians);

                if (!result.Any())
                {
                    return NotFound();
                }

                var mappedEntities = _mapper.Map<GigDto[]>(result);

                return Ok(mappedEntities);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("searchByVenue")]
        public async Task<ActionResult<GigDto[]>> GetGigsByVenue(int venueId, bool includeComedians = false)
        {
            try
            {
                var result = await _eventRepository.GetGigsByVenue(venueId, includeComedians);

                if (!result.Any())
                {
                    return NotFound();
                }

                var mappedEntities = _mapper.Map<GigDto[]>(result);

                return Ok(mappedEntities);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{gigId}")]
        public async Task<ActionResult<GigDto>> GetGig(int gigId, bool includeComedians = false)
        {
            try
            {
                var result = await _eventRepository.GetGig(gigId, includeComedians);

                if (result == null)
                {
                    return NotFound();
                }

                var mappedEntity = _mapper.Map<GigDto>(result);

                return Ok(mappedEntity);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<GigDto>> Post([FromBody] GigDto dto)
        {
            try
            {
                var mappedEntity = _mapper.Map<Gig>(dto);
                _eventRepository.Add(mappedEntity);

                if (await _eventRepository.Save())
                {
                    var location = Url.Action(action: "GetGig", new { gigId = mappedEntity.GigId });
                    return Created(location, _mapper.Map<GigDto>(mappedEntity));
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return BadRequest();
        }

        [HttpPut("{gigId}")]
        public async Task<ActionResult<GigDto>> Put(int gigId, [FromBody] GigDto dto)
        {
            try
            {
                var oldGig = await _eventRepository.GetGig(gigId);

                if (oldGig == null)
                {
                    return NotFound($"Could not find a gig with id {gigId}");
                }

                var newGig = _mapper.Map(dto, oldGig);

                _eventRepository.Update(newGig);

                if (await _eventRepository.Save())
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        [HttpDelete("{gigId}")]
        public async Task<IActionResult> Delete(int gigId)
        {
            try
            {
                var oldGig = await _eventRepository.GetGig(gigId);

                if (oldGig == null)
                {
                    return NotFound($"Could not find a gig with id {gigId}");
                }

                _eventRepository.Delete(oldGig);

                if (await _eventRepository.Save())
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

    }
}
