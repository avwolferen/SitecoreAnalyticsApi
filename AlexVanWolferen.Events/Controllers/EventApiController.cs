using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlexVanWolferen.Events.Controllers
{
    using Sitecore.Data;
    //using System.Web.Mvc;
    using System.Net;
    using Sitecore.Mvc.Controllers;
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using System.Web.Http.Results;
    using System.Web.Http;

    public class EventApiController : BaseController
    {
        /// <summary>
        /// Registers the goal.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public ActionResult RegisterGoal(string id)
        {
            if (!this.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ID.IsID(id))
            {
                this.Services.Tracker.RegisterGoal(new ID(id));
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [System.Web.Http.HttpPost]
        public ActionResult RegisterEvent(string id, string data)
        {
            if (!this.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ID.IsID(id))
            {
                // support for multiple custom values is work in progress
                this.Services.Tracker.TrackPageEvent(new ID(id), data); //, data?.CustomValues);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        /// <summary>
        /// Registers a searchterm.
        /// </summary>
        /// <param name="term">The searchterm.</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public ActionResult RegisterSearchTerm(string term)
        {
            this.Services.Tracker.RegisterSearchTerm(term);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}