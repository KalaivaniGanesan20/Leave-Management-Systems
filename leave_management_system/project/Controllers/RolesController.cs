using Microsoft.AspNetCore.Identity;
namespace app.Controllers;

public class RolesController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<RolesController> _logger;

    public RolesController(ILogger<RolesController> logger,RoleManager<IdentityRole> roleManager)
    {
        _roleManager=roleManager;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var roles=_roleManager.Roles;
        return View(roles);
    }


    [HttpGet]
    public IActionResult create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult create(IdentityRole model)
    {
      if(!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
      {
        _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
      }
      return RedirectToAction("Index");
    }
}
    