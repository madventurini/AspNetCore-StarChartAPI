using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
       
        [HttpGet("{id: int}")]
        [Route("GetById")]
        public IActionResult GetById(int id)
        {
            foreach( var o in _context.CelestialObjects)
            {
                if (o.Id == id) return Ok(o);

            }
           return NotFound();
        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
           var results=_context.CelestialObjects.Where(x=>x.Name == name).ToList();
            if(results.Count>0) return Ok(results);
            return NotFound();
            
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.CelestialObjects);

        }
     }
}
