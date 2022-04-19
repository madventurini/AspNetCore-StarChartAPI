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
            found.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();
            return Ok(found);
        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var results = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (results.Count < 1) return NotFound();
            foreach (var result in results)
            {
                result.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == result.Id).ToList();
            }
            return Ok(results);

        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var results = _context.CelestialObjects.ToList();
            foreach (var result in results)
            {
                result.Satellites = results.Where(x => x.OrbitedObjectId == result.Id).ToList();
            }
            return Ok(results);

        }
        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject newlObject)
        {
            CelestialObject celestialObject = new CelestialObject() { Id = newlObject.Id, Name = newlObject.Name, OrbitalPeriod = newlObject.OrbitalPeriod, OrbitedObjectId = newlObject.OrbitedObjectId };
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { Id = celestialObject.Id }, celestialObject);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject newCelestial)
        {
            var found = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (found == null) return NotFound();
            found.Name = newCelestial.Name;
            found.OrbitalPeriod = newCelestial.OrbitalPeriod;
            found.OrbitedObjectId = newCelestial.OrbitedObjectId;
            _context.CelestialObjects.Update(found);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var found = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if (found == null) return NotFound();
            found.Name = name;
            _context.CelestialObjects.Update(found);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("int:id")]
        public IActionResult Delete(int id)
        {
        var found = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
        if (found == null) return NotFound();
        _context.CelestialObjects.Remove(found);
        _context.SaveChanges(true);
        return NoContent(); 
    }
     }
}
