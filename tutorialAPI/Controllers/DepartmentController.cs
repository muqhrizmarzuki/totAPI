using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tutorialAPI.Data;
using tutorialAPI.Models;
using tutorialAPI.Models.Dtos;

namespace tutorialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public DepartmentController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of department
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetDepartments()
        {
            IEnumerable<Department> objFromDb = _db.Departments.OrderBy(x => x.Name).ToList();
            return Ok(objFromDb);
        }

        /// <summary>
        /// Get single record of department
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpGet("{departmentId:int}", Name = "GetDepartment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetDepartment(int departmentId)
        {
            var objFromDb = _db.Departments.FirstOrDefault(x => x.Id == departmentId);

            if(objFromDb == null)
            {
                return NotFound();
            }
            return Ok(objFromDb);
        }

        /// <summary>
        /// Create Department
        /// </summary>
        /// <param name="departmentCreateDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateDepartment(Models.Dtos.DepartmentCreateDto departmentCreateDto)
        {
            
            var isExists = _db.Departments.Any(x => x.Name.ToLower().Trim() == departmentCreateDto.Name.ToLower().Trim());

            if (departmentCreateDto == null)
            {
                return BadRequest();
            }

            if(isExists == true)
            {
                ModelState.AddModelError("", "Department is exists");
                return StatusCode(404, ModelState);
            }

            var department = _mapper.Map<Department>(departmentCreateDto);

            _db.Departments.Add(department);
            _db.SaveChanges();

            return CreatedAtRoute(nameof(GetDepartment), new { departmentId = department.Id }, department);

        }

        /// <summary>
        /// Update Department
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateDepartment(int departmentId, Department department)
        {
            if(department == null && departmentId != department.Id)
            {
                return BadRequest();
            }

            _db.Departments.Update(department);
            _db.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Delete Department
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteDepartment(int departmentId)
        {
            Department department = _db.Departments.FirstOrDefault(x => x.Id == departmentId);
            if(departmentId != department.Id)
            {
                return NotFound();
            }

            _db.Departments.Remove(department);
            _db.SaveChanges();

            return NoContent();
        }
     
    }
}