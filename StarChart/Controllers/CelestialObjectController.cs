﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;

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
            var CelestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Name == name);

            if (CelestialObject == null)
            {
                return NotFound();
            }

            CelestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == CelestialObject.Id).ToList();

            return Ok(CelestialObject);
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
    }
}
