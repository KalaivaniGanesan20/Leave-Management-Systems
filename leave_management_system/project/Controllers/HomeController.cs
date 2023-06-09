using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using project.Models;
namespace project.Controllers;
#nullable disable
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> userManager;
    public IConfiguration Configuration{get;}

    public HomeController(ILogger<HomeController> logger,UserManager<IdentityUser> _usermanager,IConfiguration configuration)
    {
        _logger = logger;
        userManager=_usermanager;
        Configuration=configuration;
    }

    public IActionResult Index(string rolename)
    {
        var user = User.Identity.Name;
        
        ViewBag.user=user;
        //Logged in users Roles

        ViewBag.Role=rolename;
        return View();
        
    }
    public IActionResult Policy()
    {
        return View();
    }

    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}
