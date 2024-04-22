using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Villa_Utility;
using Villa_WebMVC.Models;
using Villa_WebMVC.Models.Dto;
using Villa_WebMVC.Services.IServices;


namespace Villa_WebMVC.Controllers
{
	public class AuthController : Controller
	{
		//------------------implement------------------------------------------
		private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }
        //------------------login----------------------------------------------
        //GET
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO obj = new();
            return View(obj);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            //get response from loginAsync action 
            APIResponse response= await _authService.LoginAsync<APIResponse>(obj);
            if (response != null && response.IsSuccess)
            {
                
                //deserialize the response
                TokenDTO model = JsonConvert.DeserializeObject<TokenDTO>(Convert.ToString(response.Result));

                //Retrieve values from jwt token
                var handler = new JwtSecurityTokenHandler();
                var jwt=handler.ReadJwtToken(model.AccessToken);

                //tell the httpContext that user is logged In
                var identity=new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                //get role and username from User model
                //identity.AddClaim(new Claim(ClaimTypes.Name,model.User.UserName));
                //identity.AddClaim(new Claim(ClaimTypes.Role, model.User.Role));

                //get role and username from token
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u=>u.Type=="unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);

                ////retrieve token and store it in the session
                //HttpContext.Session.SetString(SD.AccessToken, model.AccessToken);

                //token provider
                _tokenProvider.SetToken(model);

                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("Custom Error",response.ErrorMessage.FirstOrDefault());
                return View(obj);
            }            
        }

        //------------------register-------------------------------------------
        //GET
        [HttpGet]
		public IActionResult Register()
		{
            //populate the roles 
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.Admin,Value=SD.Admin},
                new SelectListItem{Text=SD.Customer,Value=SD.Customer},
            };  
            ViewBag.RoleList = roleList;
			return View();
		}
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterationRequestDTO obj)
		{
            //assign the default role if not chosen
            if (string.IsNullOrEmpty(obj.Role))
            {
                obj.Role = SD.Customer;
            }
			APIResponse result = await _authService.RegisterAsync<APIResponse>(obj);    
            if (result != null && result.IsSuccess) 
            {
                return RedirectToAction("Login");
            }

            //reload the page with the list of roles
            //populate the roles 
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=SD.Admin,Value=SD.Admin},
                new SelectListItem{Text=SD.Customer,Value=SD.Customer},
            };
            ViewBag.RoleList = roleList;

            return View();
		}

        //------------------------Logout---------------------------------------
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            var token = _tokenProvider.GetToken();
            await _authService.LogoutAsync<APIResponse>(token);
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

		//------------------------Access Denied--------------------------------
		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
