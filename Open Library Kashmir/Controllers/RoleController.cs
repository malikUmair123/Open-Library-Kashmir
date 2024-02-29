using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Open_Library_Kashmir.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Open_Library_Kashmir.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationRoleManager _roleManager;
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
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRole(RegisterRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole { Name = model.RoleName };
                IdentityResult result = RoleManager.Create(role);
                if (result.Succeeded)
                {
                    string RoleId = role.Id;
                    return RedirectToAction("Index", "Home", new { Id = RoleId });
                }
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole roleToEdit = RoleManager.FindById(model.RoleId);
                if (roleToEdit == null)
                {
                    return HttpNotFound();
                }
                if (roleToEdit.Name != model.RoleName)
                {
                    roleToEdit.Name = model.RoleName;
                }

                IdentityResult result = RoleManager.Update(roleToEdit);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { Id = model.RoleId });
                }
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteRole(DeleteRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole roleToDelete = RoleManager.FindById(model.RoleId);
                if (roleToDelete == null)
                {
                    return HttpNotFound();
                }
                IdentityResult result = RoleManager.Delete(roleToDelete);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult AssignUserToRole(string userId, string roleName)
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            IdentityResult result = UserManager.AddToRole(userId, roleName);

            if (result.Succeeded)
            {
                //Whichever Page you want, you can redirect
                return RedirectToAction("Index", "Home");
            }

            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View();
        }

        private IList<string> GetUserRoles(string userId)
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roles = UserManager.GetRoles(userId);

            return roles;
        }

    }
}