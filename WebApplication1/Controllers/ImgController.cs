using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DataAccess;
using WebApplication1.ViewModels;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebApplication1.Controllers
{
    public class ImgController : Controller
    {
        private readonly AppDbContext _context;
        private IHostingEnvironment _environment;


        public ImgController(AppDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult ImgShow(IFormFile imgFile)
        {
            Console.WriteLine(imgFile.ToString());
            string path = Path.Combine(this._environment.WebRootPath, "Uploads");
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
 
            string fileName = Path.GetFileName(imgFile.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                imgFile.CopyTo(stream);
                ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }
            
            Image img = Image.FromFile(Path.Combine(path, fileName));
            
            ImgViewModel model = new ImgViewModel
            {
                Img = fileName,
                Format = img.RawFormat.ToString(),
                Size = Convert.ToInt32(imgFile.Length),
                Width = img.Width,
                Height = img.Height
            };

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool isActivated = _context.Users.Find(userId).IsActivated;
            
            if (isActivated || (!isActivated && imgFile.Length <= 100000))
            {
                return View(model);
            }
            else
            {
                return View("ImgNotActivated");
            }
        }
    }
}