﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var CelestialObject = _context.CelestialObjects.Find(id);

            if (CelestialObject == null)
            {
                return NotFound();
            }

            CelestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == CelestialObject.Id).ToList();

            return Ok(CelestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var CelestialObjects = _context.CelestialObjects.Where(x => x.Name == name).ToList();

            if (!CelestialObjects.Any())
            {
                return NotFound();
            }

            foreach (var CelestialObject in CelestialObjects)
            {
                CelestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == CelestialObject.Id).ToList();
            }

            return Ok(CelestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var CelestialObjects = _context.CelestialObjects.ToList();

            foreach (var CelestialObject in CelestialObjects)
            {
                CelestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == CelestialObject.Id).ToList();
            }

            return Ok(CelestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var CelestialObject = _context.CelestialObjects.Find(id);

            if (CelestialObject == null)
            {
                return NotFound();
            }

            CelestialObject.Name = celestialObject.Name;
            CelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            CelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(CelestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var CelestialObject = _context.CelestialObjects.Find(id);

            if (CelestialObject == null)
            {
                return NotFound();
            }

            CelestialObject.Name = name;

            _context.CelestialObjects.Update(CelestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(x => x.Id == id || x.OrbitedObjectId == id);

            if (!celestialObjects.Any())
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
