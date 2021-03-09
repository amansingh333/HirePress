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
        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult Employer()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Employer(SignupViewModel model, string ReturnUrl)
        {
            if (model.RVM != null)
            {
                if (ModelState.IsValid)
                {

                    var user = new ApplicationUser { UserName = model.RVM.Email, Email = model.RVM.Email, PhoneNumber = model.RVM.PhoneNo };

                    var result = await UserManager.CreateAsync(user, model.RVM.Password);

                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


                        RegisterViewModel rvm = new RegisterViewModel()
                        {
                            FirstName = model.RVM.FullName.Split(new String[] { " " }, StringSplitOptions.None).Length < 2 ? model.RVM.FullName : model.RVM.FullName.Split(new String[] { " " }, StringSplitOptions.None)[0],
                            LastName = model.RVM.FullName.Split(new String[] { " " }, StringSplitOptions.None).Length < 2 ? null : model.RVM.FullName.Split(new String[] { " " }, StringSplitOptions.None)[1],
                            Email = model.RVM.Email
                        };
                        Util.CreateUserDetails(rvm);

                        // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Hi, <br/>Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                        await UserManager.AddToRoleAsync(user.Id, "employer");

                        Session["Email"] = user.Email;

                        return View();
                    }
                    AddErrors(result);
                }

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true

                var user = await UserManager.FindByEmailAsync(model.LVM.Email);

                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    ViewBag.Message = String.Format("User Doesn't Exist.");
                    return View();
                }
                if (!(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ViewBag.Message = String.Format("Please Confirm your Email!");
                    return View();
                }
                var result = new SignInStatus();
                if (UserManager.IsInRole(user.Id, "admin") || UserManager.IsInRole(user.Id, "superadmin") || UserManager.IsInRole(user.Id, "employer")) {
                    result = await SignInManager.PasswordSignInAsync(user.UserName, model.LVM.Password, isPersistent: false, shouldLockout: false);
                }
                else
                {
                    ViewBag.Message = String.Format("Sorry, only Employer can Login from here!");
                    return View();
                }
                switch (result)
                {
                    case SignInStatus.Success:
                        if (UserManager.IsInRole(user.Id, "admin"))
                        {
                            Session["UserAdmin"] = "Admin";
                            if (UserManager.IsInRole(user.Id, "superadmin"))
                            {
                                Session["UserSuperAdmin"] = "SuperAdmin";
                                CreateSession(model.LVM.Email);
                                return RedirectToLocal(ReturnUrl, "superadmin");
                            }
                            else
                            {
                                CreateSession(model.LVM.Email);
                                return RedirectToLocal(ReturnUrl, "admin");
                            }

                        }
                        else
                        {
                            Session["UserEmployer"] = "Employer";
                            CreateSession(model.LVM.Email);
                            return RedirectToLocal(ReturnUrl, "employer");
                        }
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = ReturnUrl });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        ViewBag.Message = String.Format("Invalid login attempt.");
                        return View(model);
                }
            }
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
        public async Task<ActionResult> Login(LoginViewModel model, string ReturnUrl)
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
            var result = new SignInStatus();
            if (UserManager.IsInRole(user.Id, "admin") || UserManager.IsInRole(user.Id, "superadmin") || !UserManager.IsInRole(user.Id, "employer"))
            {
                result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, isPersistent: false, shouldLockout: false);
            }
            else
            {
                ViewBag.Message = String.Format("Employer cannot Login from here.");
                return View();
            }
            switch (result)
            {
                case SignInStatus.Success:
                    if (UserManager.IsInRole(user.Id, "admin"))
                    {
                        Session["UserAdmin"] = "Admin";
                        if(UserManager.IsInRole(user.Id, "superadmin"))
                        {
                            Session["UserSuperAdmin"] = "SuperAdmin";
                            CreateSession(model.Email);
                            return RedirectToLocal(ReturnUrl, "superadmin");
                        }
                        else
                        {
                            CreateSession(model.Email);
                            return RedirectToLocal(ReturnUrl, "admin");
                        }

                    }
                    else
                    {
                        CreateSession(model.Email);
                        return RedirectToLocal(ReturnUrl, "user");
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = ReturnUrl });
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
            Session["UserAdmin"] = null;
            Session["UserSuperAdmin"] = null;
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignOut();
            Session.Abandon();
            if(Session["UserEmployer"] == null)            
                return RedirectToAction("Login", "Account");
            else
            {
                Session["UserEmployer"] = null;
                return RedirectToAction("Employer", "Account");
            }

        }

        public void CreateSession(string email)
        {
            Session["Email"] = null;
            Session["UserEmail"] = email;
            Session["UserType"] = "LoggedIn";
            Session["UserName"] = Util.GetUserName(email);
        }

        [AllowAnonymous]
        public ActionResult OnBoard()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult Admin()
        {
            return View();  
        }

        [Authorize(Roles = "admin,superadmin")]
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

        private ActionResult RedirectToLocal(string ReturnUrl, string role)
        {
            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            if (role == "user")
                return RedirectToAction("Index", "Candidate");
            else if(role == "admin")
                return RedirectToAction("Admin");
            else if (role == "employer")
                return RedirectToAction("Index","Employer");
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