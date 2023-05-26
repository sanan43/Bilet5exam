using bilet5exam.Areas.Admin.ViewModel;
using bilet5exam.DAL;
using bilet5exam.Models;
using bilet5exam.Utilities.Constants;
using bilet5exam.Utilities.extension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bilet5exam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _rootPath;
        public TeamsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team");
        }


        public async Task<IActionResult> Index()
        {
            List<Team> teams = await _context.Teams.ToListAsync();
            
            return View(teams);
        }



        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamsVM teamsVM)
        {
            if (!ModelState.IsValid)
            {
                return View(teamsVM);
            }
            if (!teamsVM.Image.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", ErrorMessages.FileMustBeImageType);
                return View(teamsVM);
            }
            if (!teamsVM.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", ErrorMessages.FileSizeMustLessThan200KB);
                return View(teamsVM);
            }
            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img","team");
            string fileName = await teamsVM.Image.SaveAsync(rootPath);
            Team team = new Team()
            {
                ImagePath = fileName,
                Name = teamsVM.Name,
                Surname = teamsVM.Surname,
                JobDescription = teamsVM.JobDescription,
                Position = teamsVM.Position

            };
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int id)
        {
            Team? team =await _context.Teams.FindAsync(id);
            if (team==null)
            {
                return NotFound();
                
            }
            string filePath = Path.Combine(_rootPath,team.ImagePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Update(int id)
        {
            Team team = await _context.Teams.FindAsync(id);
            if (team == null) return NotFound();
            UpdateTeamVM updateTeamVM = new UpdateTeamVM()
            {
                
                Name = team.Name,
                Surname = team.Surname,
                JobDescription = team.JobDescription,
                Position = team.Position,
                Id = id
                
            };
            return View(updateTeamVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateTeamVM teamVM)
        {
            if (!ModelState.IsValid)
            {
                return View(teamVM);
            }
            if (!teamVM.Image.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", ErrorMessages.FileMustBeImageType);
                return View(teamVM);
            }
            if (!teamVM.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", ErrorMessages.FileSizeMustLessThan200KB);
                return View(teamVM);
            }
            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img","team");
            Team team = await _context.Teams.FindAsync(teamVM.Id);

            string filePath = Path.Combine(rootPath, team.ImagePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            string newFileName = await teamVM.Image.SaveAsync(rootPath);

            team.ImagePath = newFileName;
            team.Name = teamVM.Name;
            team.Surname = teamVM.Surname;
            team.Position = teamVM.Position;
            team.JobDescription = teamVM.JobDescription;
            

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
