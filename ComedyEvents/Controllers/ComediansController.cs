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
    public class ComediansController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public ComediansController(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ComedianDto[]>> GetComedians()
        {
            try
            {
                var result = await _eventRepository.GetComedians();

                var mappedEntities = _mapper.Map<ComedianDto[]>(result);

                return Ok(mappedEntities);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<ComedianDto[]>> GetComediansByEvent(int eventId)
        {
            try
            {
                var result = await _eventRepository.GetComediansByEvent(eventId);

                if (!result.Any())
                {
                    return NotFound();
                }

                var mappedEntities = _mapper.Map<ComedianDto[]>(result);

                return Ok(mappedEntities);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{comedianId}")]
        public async Task<ActionResult<ComedianDto>> GetComedian(int comedianId)
        {
            try
            {
                var result = await _eventRepository.GetComedian(comedianId);

                if (result == null)
                {
                    return NotFound();
                }

                var mappedEntity = _mapper.Map<ComedianDto>(result);

                return Ok(mappedEntity);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ComedianDto>> Post([FromBody] ComedianDto dto)
        {
            try
            {
                var mappedEntity = _mapper.Map<Comedian>(dto);
                _eventRepository.Add(mappedEntity);

                if (await _eventRepository.Save())
                {
                    var location = Url.Action(action: "GetComedian", new { comedianId = mappedEntity.ComedianId });
                    return Created(location, _mapper.Map<ComedianDto>(mappedEntity));
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return BadRequest();
        }

        [HttpPut("{comedianId}")]
        public async Task<ActionResult<ComedianDto>> Put(int comedianId, [FromBody] ComedianDto dto)
        {
            try
            {
                var oldComedian = await _eventRepository.GetComedian(comedianId);

                if (oldComedian == null)
                {
                    return NotFound($"Could not find comedian with id {comedianId}");
                }

                var newComedian = _mapper.Map(dto, oldComedian);

                _eventRepository.Update(newComedian);

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

        [HttpDelete("{comedianId}")]
        public async Task<IActionResult> Delete(int comedianId)
        {
            try
            {
                var oldComedian = await _eventRepository.GetComedian(comedianId);

                if (oldComedian == null)
                {
                    return NotFound($"Could not find comedian with id {comedianId}");
                }

                _eventRepository.Delete(oldComedian);

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
