using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain;
using Persistence;


namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private readonly DataContext _context;
        public ActivitiesController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task <ActionResult<List<Activity>>> GetActivities()
        {
            return await _context.Activities.ToListAsync();

        }
        [HttpGet("{id}")]
        public async Task <ActionResult<Activity>> GetActivitie(Guid id)
        {
            return await _context.Activities.FindAsync(id);
        }
    }
}