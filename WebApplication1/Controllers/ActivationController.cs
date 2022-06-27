using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using security;
using WebApplication1.DataAccess;
using WebApplication1.ViewModels;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebApplication1.Controllers
{
    public class ActivationController : Controller
    {
        private readonly AppDbContext _context;
        private IHostingEnvironment _environment;


        public ActivationController(AppDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        
        
        public IActionResult Activation()
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

            captcha.FilePath = fileName;

            _context.Captchas.Add(captcha);
            _context.SaveChanges();
            
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = _context.Users.Find(userId);
            
            ActivationViewModel model = new ActivationViewModel();
            model.IsActivated = user.IsActivated;
            model.CaptchaName = fileName;
            return View(model);
        }

        [HttpPost]
        public IActionResult Activation(ActivationViewModel model)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = _context.Users.Find(userId);

            if (ModelState.IsValid)
            {
                

                if (model.CaptchaText == _context.Captchas.FirstOrDefault(captcha => captcha.UserId == userId && captcha.FilePath == model.CaptchaName)?.Num)
                {
                    Captcha captcha = _context.Captchas.FirstOrDefault(captcha =>
                        captcha.UserId == userId && captcha.FilePath == model.CaptchaName);
                    _context.Captchas.Remove(captcha);
                    _context.SaveChanges();
                    if (Caesar.Decipher(model.ActivationKey, user.Key) == user.Email)
                    {
                        user.IsActivated = true;
                        _context.SaveChanges();
                    }
                    ModelState.AddModelError(string.Empty, "Невірний ключ");
                    return Activation();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Невірна Captcha");
                }
                
                return Activation();
            }
            
            return Activation();
        }

        public IActionResult ShowKey()
        {
            ActivationViewModel model = new ActivationViewModel();
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            User user = _context.Users.Find(userId);

            model.ActivationKey = Caesar.Encipher(user.Email, user.Key);
            
            return View(model);
        }
    }
}