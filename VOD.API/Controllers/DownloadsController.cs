using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Extensions;
using VOD.Common.Services;

namespace VOD.API.Controllers
{
    [Route("api/courses/{courseId}/modules/{moduleId}/downloads")]
    [ApiController]
    public class DownloadsController : ControllerBase
    {
        #region Properties and Variables
        private readonly LinkGenerator _linkGenerator;
        private readonly IAdminService _db;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public DownloadsController(IAdminService db, IMapper mapper, LinkGenerator linkGenerator)
        {
            _db = db;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }
        #endregion

        #region Actions
        [HttpGet()]
        public async Task<ActionResult<List<DownloadDTO>>> Get(int courseId, int moduleId, bool include = false)
        {
            try
            {
                return courseId.Equals(0) || moduleId.Equals(0) ?
                    await _db.GetAsync<Download, DownloadDTO>(include) :
                    await _db.GetAsync<Download, DownloadDTO>(g => g.CourseId.Equals(courseId) && g.ModuleId.Equals(moduleId), include);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DownloadDTO>> Get(int id, int courseId, int moduleId)
        {
            try
            {
                var entity = courseId.Equals(0) || moduleId.Equals(0) ?
                    await _db.SingleAsync<Download, DownloadDTO>(s => s.Id.Equals(id), true) :
                    await _db.SingleAsync<Download, DownloadDTO>(s => s.CourseId.Equals(courseId) && s.ModuleId.Equals(moduleId) && s.Id.Equals(id),
                        true);
                if (entity == null) return NotFound();
                return entity;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DownloadDTO>> Post(DownloadDTO model, int courseId, int moduleId)
        {
            try
            {
                if (courseId.Equals(0)) courseId = model.CourseId;
                if (moduleId.Equals(0)) moduleId = model.ModuleId;
                if (model == null) return BadRequest("No entity provided");
                if (!model.CourseId.Equals(courseId))
                    return BadRequest("Differing ids");
                if (model.Title.IsNullOrEmptyOrWhiteSpace())
                    return BadRequest("Title is required");
                var exists = await _db.AnyAsync<Course>(a => a.Id.Equals(courseId));
                if (!exists) return BadRequest(
                    "Could not find related entity");
                exists = await _db.AnyAsync<Module>(a => a.Id.Equals(moduleId) && a.CourseId.Equals(courseId));
                if (!exists) return BadRequest(
                    "Could not find related entity");
                var id = await _db.CreateAsync<DownloadDTO, Download>(
                    model);
                if (id < 1) return BadRequest("Unable to add the entity");
                var dto = await _db.SingleAsync<Download, DownloadDTO>(s => s.CourseId.Equals(courseId) && s.ModuleId.Equals(moduleId) && s.Id.Equals(id), true);
                if (dto == null) return BadRequest("Unable to add the entity");
                var uri = _linkGenerator.GetPathByAction("Get", "Downloads", new {
                        courseId = dto.CourseId,
                        moduleId = dto.ModuleId,
                        id = dto.Id
                    });
                return Created(uri, dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add the entity");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int courseId, int moduleId,
            int id, DownloadDTO model)
        {
            try
            {
                if (model == null) return BadRequest("Missing entity");
                if (!model.Id.Equals(id)) return BadRequest(
                    "Differing ids");
                if (model.Title.IsNullOrEmptyOrWhiteSpace())
                    return BadRequest("Title is required");
                var exists = await _db.AnyAsync<Course>(a => a.Id.Equals(courseId));
                if (!exists) return BadRequest(
                    "Could not find related entity");
                exists = await _db.AnyAsync<Module>(a => a.Id.Equals(moduleId) && a.CourseId.Equals(courseId));
                if (!exists) return BadRequest(
                    "Could not find related entity");
                exists = await _db.AnyAsync<Module>(a => a.Id.Equals(model.ModuleId) && a.CourseId.Equals(model.CourseId));
                if (!exists) return BadRequest(
                    "Could not find related entity");
                exists = await _db.AnyAsync<Download>(a => a.Id.Equals(id));
                if (!exists) return BadRequest("Could not find entity");
                if (await _db.UpdateAsync<DownloadDTO, Download>(model))
                    return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the entity");
            }
            return BadRequest("Unable to update the entity");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, int courseId, int moduleId)
        {
            try
            {
                var exists = await _db.AnyAsync<Course>(g => g.Id.Equals(courseId));
                if (!exists) return BadRequest("Could not find related entity");
                exists = await _db.AnyAsync<Module>(g => g.CourseId.Equals(courseId) && g.Id.Equals(moduleId));
                if (!exists) return BadRequest("Could not find related entity");
                exists = await _db.AnyAsync<Download>(g => g.Id.Equals(id) && g.CourseId.Equals(courseId) && g.ModuleId.Equals(moduleId));
                if (!exists) return BadRequest("Could not find entity");
                if (await _db.DeleteAsync<Download>(d => d.Id.Equals(id) && d.CourseId.Equals(courseId) && d.ModuleId.Equals(moduleId))) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete the entity");
            }
            return BadRequest("Unable to update the entity");
        }
        #endregion
    }
}