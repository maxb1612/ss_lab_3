using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DataAccess;
using WebApplication1.ViewModels;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebApplication1.Controllers
{
    public class CaptchaController : Controller
    {
        private AppDbContext _context;
        private IHostingEnvironment _environment;

        public CaptchaController(AppDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult NewCaptcha(ActivationViewModel activationViewModel)
        {
            Captcha captcha = new Captcha();
            captcha.Id = Guid.NewGuid();
            captcha.Num = Random.Shared.Next(1000, 9999);
            captcha.UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            string path = Path.Combine(this._environment.WebRootPath, "Captcha");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Random.Shared.Next(1, Int32.MaxValue).ToString() + ".jpeg";

            Bitmap imgFile = CaptchaGenerator.Create(captcha.Num.ToString());
            
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                imgFile.Save(stream, ImageFormat.Jpeg);
            }
            imgFile.Dispose();

            captcha.FilePath = Path.Combine(path, fileName);

            _context.Captchas.Add(captcha);

            CaptchaViewModel captchaViewModel = new CaptchaViewModel
            {
                CaptchaName = fileName
            };
            
            return View(captchaViewModel);
        }
    }
}