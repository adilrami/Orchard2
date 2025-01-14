﻿using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Orchard.ContentManagement;
using Orchard.Contents;
using System.Threading.Tasks;

namespace Orchard.Content.Controllers
{
    public class ApiController : Controller
    {
        private readonly IContentManager _contentManager;
        private readonly IAuthorizationService _authorizationService;

        public ApiController(
            IContentManager contentManager,
            IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            _contentManager = contentManager;
        }

        public async Task<IActionResult> Get(int id)
        {
            var contentItem = await _contentManager.GetAsync(id);

            if (contentItem == null)
            {
                return HttpNotFound();
            }

            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ViewContent, contentItem))
            {
                return new HttpUnauthorizedResult();
            }

            return new ObjectResult(contentItem);
        }
    }
}
