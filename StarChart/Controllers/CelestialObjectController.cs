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
       
        
        [Route("GetById")]
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var found = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (found == null) return NotFound();
            found.Satellites=_context.CelestialObjects.Where(x=>x.OrbitedObjectId == id).ToList();
            return Ok(found);
        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var results = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (results.Count<1) return NotFound();
            foreach(var result in results)
            {
                result.Satellites=_context.CelestialObjects.Where(x=>x.OrbitedObjectId==result.Id).ToList();
            }
            return Ok(results);
            
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var results = _context.CelestialObjects.ToList();
            foreach(var result in results)
            {
                result.Satellites=results.Where(x=>x.OrbitedObjectId==result.Id).ToList();
            }
            return Ok(results);

        }
     }
}
