﻿using AutoMapper;
using ComedyEvents.DTO;
using ComedyEvents.Models;
using ComedyEvents.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComedyEvents.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventsController(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<EventDto[]>> Get(bool includeGigs = false)
        {
            try
            {
                var result = await _eventRepository.GetEvents(includeGigs);

                var mappedEntities = _mapper.Map<EventDto[]>(result);

                return Ok(mappedEntities);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult<EventDto>> Get(int eventId, bool includeGigs = false)
        {
            try
            {
                var result = await _eventRepository.GetEvent(eventId, includeGigs);

                if (result == null)
                {
                    return NotFound();
                }

                var mappedEntity = _mapper.Map<EventDto>(result);

                return Ok(mappedEntity);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<EventDto[]>> SearchByDate(DateTime date, bool includeGigs = false)
        {
            try
            {
                var result = await _eventRepository.GetEventsByDate(date, includeGigs);

                if (!result.Any())
                {
                    return NotFound();
                }

                var mappedEntities = _mapper.Map<EventDto[]>(result);

                return Ok(mappedEntities);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> Post([FromBody] EventDto dto)
        {
            try
            {
                var mappedEntity = _mapper.Map<Event>(dto);
                _eventRepository.Add(mappedEntity);

                if (await _eventRepository.Save())
                {
                    var location = Url.Action(action: "Get", new { eventId = mappedEntity.EventId });
                    return Created(location, _mapper.Map<EventDto>(mappedEntity));
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return BadRequest();
        }

        [HttpPut("{eventId}")]
        public async Task<ActionResult<EventDto>> Put(int eventId, [FromBody] EventDto dto)
        {
            try
            {
                var oldEvent = await _eventRepository.GetEvent(eventId);

                if(oldEvent == null)
                {
                    return NotFound($"Could not find event with id {eventId}");
                }

                var newEvent = _mapper.Map(dto, oldEvent);

                _eventRepository.Update(newEvent);
                
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

        [HttpDelete("{eventId}")]
        public async Task<IActionResult> Delete(int eventId)
        {
            try
            {
                var oldEvent = await _eventRepository.GetEvent(eventId);

                if (oldEvent == null)
                {
                    return NotFound($"Could not find event with id {eventId}");
                }

                _eventRepository.Delete(oldEvent);

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
