using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web.Mvc;
using System.Threading.Tasks;
using HirePressCore.Model;
using HirePressCore.Partial;

namespace HirePress.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Account
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    RegisterViewModel rvm = new RegisterViewModel()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email
                    };
                    Util.CreateUserDetails(rvm);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Hi, <br/>Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    Session["Email"] = user.Email;
                    
                    return RedirectToAction("OnBoard");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            Session["Email"] = null;
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true

            var user = await UserManager.FindByEmailAsync(model.Email);
            
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                ViewBag.Message = String.Format("User Doesn't Exist.");
                return View("Login");
            }
            if (!(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                ViewBag.Message = String.Format("Please Confirm your Email!");
                return View("Login");
            }
            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, shouldLockout: false);
            
            switch (result)
            {
                case SignInStatus.Success:
                    Session["Email"] = null;
                    Session["UserEmail"] = model.Email;
                    Session["UserType"] = "LoggedIn";
                    Session["UserName"] = Util.GetUserName(model.Email);
                    if (UserManager.IsInRole(user.Id, "admin"))
                    {
                        Session["UserAdmin"] = "Admin";
                        if(UserManager.IsInRole(user.Id, "superadmin"))
                        {
                            Session["UserSuperAdmin"] = "SuperAdmin";
                            return RedirectToLocal(returnUrl, "superadmin");
                        }
                        else
                        {
                            return RedirectToLocal(returnUrl, "admin");
                        }

                    }
                    else
                    {
                        return RedirectToLocal(returnUrl, "user");
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    ViewBag.Message = String.Format("Invalid login attempt.");
                    return View("Login", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Session["UserType"] = null;
            Session["UserEmail"] = null;
            Session["UserName"] = null;
            Session["Email"] = null;
            Session["UserRole"] = null;
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult OnBoard()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();  
        }
        public ActionResult SuperAdmin()
        {
            return View();
        }

        //For Super Admin
        [HttpPost]
        public async Task<ActionResult> SetRemoveAdminRole(string Id, bool Update)
        {
            if (Update)
            {
                await UserManager.AddToRoleAsync(Id, "admin");
            }
            else
            {
                await UserManager.RemoveFromRoleAsync(Id, "admin");
            }
            return RedirectToAction("SuperAdmin");
        }

        //For Super Admin
        [HttpGet]
        public JsonResult GetUserDetailsData()
        {
            var data = Util.GetUserDetails();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl, string role)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            if (role == "user")
                return RedirectToAction("Index", "Candidate");
            else if(role == "admin")
                return RedirectToAction("Admin");
            else
                return RedirectToAction("SuperAdmin");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}