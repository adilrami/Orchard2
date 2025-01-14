﻿using Microsoft.AspNet.Mvc;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Display;
using Orchard.DisplayManagement.Admin;
using Orchard.DisplayManagement.ModelBinding;
using System.Threading.Tasks;
using YesSql.Core.Services;

namespace Orchard.Demo.Controllers
{
    public class ContentController : Controller, IUpdateModel
    {
        private readonly IContentItemDisplayManager _contentDisplay;
        private readonly IContentManager _contentManager;
        private readonly ISession _session;

        public ContentController(
            IContentManager contentManager,
            IContentItemDisplayManager contentDisplay,
            ISession session)
        {
            _contentManager = contentManager;
            _contentDisplay = contentDisplay;
            _session = session;
        }

        public async Task<ActionResult> Display(int id)
        {
            var contentItem = await _contentManager.GetAsync(id);

            if (contentItem == null)
            {
                return HttpNotFound();
            }

            var shape = await _contentDisplay.BuildDisplayAsync(contentItem, this);
            return View(shape);
        }

        [Admin]
        public async Task<ActionResult> Edit(int id)
        {
            var contentItem = await _contentManager.GetAsync(id);

            if (contentItem == null)
            {
                return HttpNotFound();
            }

            var shape = await _contentDisplay.BuildEditorAsync(contentItem, this);
            return View(shape);
        }

        [Admin, HttpPost, ActionName("Edit")]
        public async Task<ActionResult> EditPost(int id)
        {
            var contentItem = await _contentManager.GetAsync(id);

            if (contentItem == null)
            {
                return HttpNotFound();
            }

            var shape = await _contentDisplay.UpdateEditorAsync(contentItem, this);

            if (!ModelState.IsValid)
            {
                _session.Cancel();
                return View("Edit", shape);
            }

            _session.Save(contentItem);
            return RedirectToAction("Edit", id);


        }
    }
}
