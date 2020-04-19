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
    public class VenuesController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public VenuesController(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<VenueDto>> Post([FromBody] VenueDto dto)
        {
            try
            {
                var mappedEntity = _mapper.Map<Venue>(dto);
                _eventRepository.Add(mappedEntity);

                if (await _eventRepository.Save())
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return BadRequest();
        }

        [HttpPut("{venueId}")]
        public async Task<ActionResult<VenueDto>> Put(int venueId, [FromBody] VenueDto dto)
        {
            try
            {
                var events = await _eventRepository.GetEvents();
                var oldVenue = events.Where(v => v.VenueId == venueId).Select(c => c.Venue).FirstOrDefault();

                if (oldVenue == null)
                {
                    return NotFound($"Could not find venue with id {venueId}");
                }

                var newVenue = _mapper.Map(dto, oldVenue);

                _eventRepository.Update(newVenue);

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

        [HttpDelete("{venueId}")]
        public async Task<IActionResult> Delete(int venueId)
        {
            try
            {
                var events = await _eventRepository.GetEvents();
                var oldVenue = events.Where(v => v.VenueId == venueId).Select(c => c.Venue).FirstOrDefault();

                if (oldVenue == null)
                {
                    return NotFound($"Could not find a venue with id {venueId}");
                }

                _eventRepository.Delete(oldVenue);

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
