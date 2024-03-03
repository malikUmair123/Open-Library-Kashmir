using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Open_Library_Kashmir.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Linq.Expressions;

namespace Open_Library_Kashmir.Controllers
{
    [LogCustomExceptionFilter]
    public class UsersAdminController : Controller
    {
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;
        //private ApplicationSignInManager _signInManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
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

        //public ApplicationSignInManager SignInManager
        //{
        //    get
        //    {
        //        return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        //    }
        //    private set
        //    {
        //        _signInManager = value;
        //    }
        //}

        //No need of this, use DI method through following parametrised contructor
        private readonly ApplicationDbContext _context;

        public UsersAdminController()
        {
            _context = new ApplicationDbContext();
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_context));
            RoleManager = new ApplicationRoleManager(new RoleStore<IdentityRole>(_context));
            //SignInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();


        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToListAsync());
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            var roleNames = await UserManager.GetRolesAsync(user.Id);

            return View(new UserDetailsViewModel { User = user, Roles = roleNames });
        }

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Id", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, string RoleId)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.UserName = userViewModel.Email;
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User Admin to Role Admin
                if (adminresult.Succeeded)
                {
                    if (!String.IsNullOrEmpty(RoleId))
                    {
                        //Find Role Admin
                        var role = await RoleManager.FindByIdAsync(RoleId);
                        var result = await UserManager.AddToRoleAsync(user.Id, role.Name);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First().ToString());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Id", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First().ToString());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                return View();
            }
        }

        //
        // GET: /Users/Edit/1
        public ActionResult Edit(string id)
        {
            //Creating an Instance of EditUserViewModel
            EditUserViewModel model = new EditUserViewModel();

            //Fetch the User Details by UserId using the FindById method
            ApplicationUser UserToEdit = UserManager.FindById(id);

            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");

            //If the user exists then map the data to EditUserViewModel properties
            if (UserToEdit != null)
            {
                model.UserId = UserToEdit.Id;
                model.FirstName = UserToEdit.FirstName;
                model.LastName = UserToEdit.LastName;
                model.Email = UserToEdit.Email;
                model.PhoneNumber = UserToEdit.PhoneNumber;

            } else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(model);
            //if (id == null)
            //{
            //}
            //ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");

            //var user = await UserManager.FindByIdAsync(id);
            //if (user == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(user);
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model, string RoleId)
        {

            if (ModelState.IsValid)
            {
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                var user = await UserManager.FindByIdAsync(model.UserId);
                user.UserName = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                //Update the user details
                await UserManager.UpdateAsync(user);

                //If user has existing Role then remove the user from the role
                // This also accounts for the case when the Admin selected Empty from the drop-down and
                // this means that all roles for the user must be removed
                var rolesForUser = await UserManager.GetRolesAsync(model.UserId);
                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser)
                    {
                        var result = await UserManager.RemoveFromRoleAsync(model.UserId, item); ;
                    }
                }

                if (!String.IsNullOrEmpty(RoleId))
                {
                    //Find Role
                    var role = await RoleManager.FindByIdAsync(RoleId);
                    //Add user to new role
                    var result = await UserManager.AddToRoleAsync(model.UserId, role.Name);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.First().ToString());
                        ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                        return View();
                    } else
                    {
                        // Save changes to the database...is it needed?
                        await UserManager.UpdateAsync(user);
                        // After updating the user's roles: Reissue the user's cookie
                        //if (User.Identity.GetUserId() == model.UserId)
                        //{
                        //    await SignInManager.SignInAsync(user, false, false);
                        //}
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.RoleId = new SelectList(RoleManager.Roles, "Id", "Name");
                return View();
            }
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                var logins = user.Logins;
                foreach (var login in logins)
                {
                    var loginInfo = new UserLoginInfo(login.LoginProvider, login.ProviderKey);
                    await UserManager.RemoveLoginAsync(id, loginInfo);
                }
                var rolesForUser = await UserManager.GetRolesAsync(id);
                if (rolesForUser.Count() > 0)
                {

                    foreach (var item in rolesForUser)
                    {
                        await UserManager.RemoveFromRoleAsync(user.Id, item);
                    }
                }
                await UserManager.DeleteAsync(user);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
    }
}