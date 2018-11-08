﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Finance101.Models;
using Finance101.Models.Custom;
using Finance101.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace Finance101.Controllers
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        public ActionResult Invite()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Invite(string email)
        {
            var code = Guid.NewGuid();
            var callbackUrl = Url.Action("CreateJoinHousehold", "Home", new { code }, protocol: Request.Url.Scheme);

            EmailService ems = new EmailService();
            IdentityMessage msg = new IdentityMessage();

            msg.Body = "Please join my household.... And bring ALL of your money!!!" + Environment.NewLine + "Please click the following link to join <a href=\"" + callbackUrl + "\">JOIN</a>";
            msg.Destination = email;
            msg.Subject = "Invite to Household";

            await ems.SendMailAsync(msg);

            //Create record in the Invites table
            Invite model = new Invite();
            model.Email = email;
            model.HHToken = code;
            model.HouseholdId = User.Identity.GetHouseholdId().Value;
            model.InviteDate = DateTimeOffset.Now;
            model.InvitedById = User.Identity.GetUserId();

            db.Invites.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Leavehousehold()
        {
            Household model = db.Households.Find(User.Identity.GetHouseholdId().Value);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Leavehousehold(Household model)
        {
            Household hh = db.Households.Find(model.Id);
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            hh.Members.Remove(user);
            db.SaveChanges();

            await ControllerContext.HttpContext.RefreshAuthentication(user);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult CreateJoinHousehold(Guid? code)
        {
            //If the current user accessing this page already has a HouseholdId, send them to their dashboard
            if (User.Identity.IsInHousehold())
            {
                return RedirectToAction("Index", "Home");
            }

            HouseholdViewModel vm = new HouseholdViewModel();

            //Determine whether the user has been sent an invite and set property values 
            if (code != null)
            {
                string msg = "";
                if (ValidInvite(code, ref msg))
                {
                    Invite result = db.Invites.FirstOrDefault(i => i.HHToken == code);

                    vm.IsJoinHouse = true;
                    vm.HHId = result.HouseholdId;
                    vm.HHName = result.Household.Name;

                    //Set USED flag to true for this invite

                    result.HasBeenUsed = true;

                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                    user.InviteEmail = result.Email;
                    db.SaveChanges();
                }
                else
                {
                    return RedirectToAction("InviteError", new { errMsg = msg });
                }
            }
            return View(vm);
        }

        private bool ValidInvite(Guid? code, ref string message)
        {

            if ((DateTime.Now - db.Invites.FirstOrDefault(i => i.HHToken == code).InviteDate).TotalDays < 6)
            {
                bool result = db.Invites.FirstOrDefault(i => i.HHToken == code).HasBeenUsed;
                if (result)
                {
                    message = "invalid";
                }
                else
                {
                    message = "valid";
                }

                return !result;
            }
            else
            {
                message = "expired";
                return false;
            }

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateHousehold(HouseholdViewModel vm)
        {
            //Create new Household and save to DB
            Household hh = new Household();
            hh.Name = vm.HHName;
            db.Households.Add(hh);
            db.SaveChanges();

            //Add the current user as the first member of the new household
            var user = db.Users.Find(User.Identity.GetUserId());
            hh.Members.Add(user);
            db.SaveChanges();

            //Soution1
            //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            //Solution2
            //((ClaimsIdentity)identity).RemoveClaim(identity.FindFirst(ClaimTypes.Name));
            //((ClaimsIdentity)identity).AddClaim(new Claim(ClaimTypes.Name, "new_name"));

            //Solution3
            //Task SignInManager<>.RefreshSignInAsync(vm.Member);

            //Solution4
            //identity.AddClaim(new Claim("myClaimType", "myClaimValue"));

            //var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            //authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

            //Solution5 **BEST**
            await ControllerContext.HttpContext.RefreshAuthentication(user);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> JoinHousehold(HouseholdViewModel vm)
        {
            Household hh = db.Households.Find(vm.HHId);
            var user = db.Users.Find(User.Identity.GetUserId());

            hh.Members.Add(user);
            db.SaveChanges();

            await ControllerContext.HttpContext.RefreshAuthentication(user);

            return RedirectToAction("Index", "Home");
        }


        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Households.Add(household);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
