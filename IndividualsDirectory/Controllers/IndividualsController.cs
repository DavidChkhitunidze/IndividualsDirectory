using IndividualsDirectory.Entities;
using IndividualsDirectory.Helpers.CustomActionFilters;
using IndividualsDirectory.Helpers.Extensions;
using IndividualsDirectory.Models;
using IndividualsDirectory.Models.Create;
using IndividualsDirectory.Models.Delete;
using IndividualsDirectory.Models.Get;
using IndividualsDirectory.Models.ResourceControls;
using IndividualsDirectory.Models.ResourceControls.Extensions;
using IndividualsDirectory.Models.Response;
using IndividualsDirectory.Models.Update;
using IndividualsDirectory.Repositories.Abstracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IndividualsDirectory.Controllers
{
    public class IndividualsController : Controller
    {
        #region PrivateFields

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IEntityRepository _repository;

        #endregion

        #region Constructor

        public IndividualsController(IHostingEnvironment hostingEnvironment, IEntityRepository repository)
        {
            _hostingEnvironment = hostingEnvironment;
            _repository = repository;
        }

        #endregion

        #region HttpGetMethods

        [HttpGet]
        public async Task<IActionResult> Index(ResourceParams<GetIndividualPreviewViewModel> resourceParams)
        {
            ViewData["Search"] = resourceParams.Search;
            ViewData["CurrentSort"] = resourceParams.Sort;
            ViewData["SortProperty"] = resourceParams.OrderBy;
            ViewData["Sort"] = resourceParams.Sort == "asc" ? "desc" : "asc";

            var response = new Response<PagedList<GetIndividualPreviewViewModel>>();

            var peopleResult = await _repository.GetEntitiesAsync<Individual>();

            if (!peopleResult.Success)
            {
                response.SetErrorMessages(peopleResult.ErrorMessages);
                return View(response);
            }

            var peopleEntities = peopleResult.Model;
            if (!string.IsNullOrEmpty(resourceParams.Search))
            {
                peopleEntities = peopleEntities.Where(i => i.FirstName.ToLowerInvariant().Contains(resourceParams.Search)
                                                        || i.LastName.ToLowerInvariant().Contains(resourceParams.Search)
                                                        || i.PersonalNumber.ToLowerInvariant().Contains(resourceParams.Search));
            }

            var filteredPeopleViewModels = peopleEntities.GetFilteredData(resourceParams, GetIndividualPreviewViewModel.Projection);

            response.SetSuccess();
            response.SetModel(filteredPeopleViewModels);

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = new Response<GetIndividualViewModel>();

            var personResult = await _repository.GetEntityAsync<Individual>
                (id, nameof(Individual.City), nameof(Individual.PhoneNumbers), $"{nameof(Individual.RelatedIndividuals)}.{nameof(RelatedIndividual.RelatedTo)}");

            if (!personResult.Success)
            {
                response.SetErrorMessages(personResult.ErrorMessages);
                return View(response);
            }

            var personEntity = personResult.Model;
            var personViewModel = GetIndividualViewModel.Projection.Compile().Invoke(personEntity);

            response.SetSuccess();
            response.SetModel(personViewModel);

            return View(response);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var individualResult = await _repository.GetEntityAsync<Individual>
                (id, nameof(Individual.PhoneNumbers), nameof(Individual.RelatedIndividuals));

            if (!individualResult.Success)
            {
                ModelState.AddModelError("", string.Join("\r\n", individualResult.ErrorMessages));
                return View();
            }
            var individualEntity = individualResult.Model;
            var individualViewModel = UpdateIndividualViewModel.Projection(individualEntity);

            return View(individualViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id) 
        {
            var individualResult = await _repository.GetEntityAsync<Individual>(id);
            if (!individualResult.Success)
            {
                ModelState.AddModelError("", string.Join("\r\n", individualResult.ErrorMessages));
                return View();
            }

            var individualEntity = individualResult.Model;
            var individualViewModel = DeleteIndividualViewModel.Projection(individualEntity);

            return View(individualViewModel);
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion

        #region HttpPostMethods

        [HttpPost]
        [ValidateModelState]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateIndividualViewModel individual)
        {
            var files = HttpContext.Request.Form.Files;
            if (files != null && files.Count > 0)
            {
                var uploadResult = await files.UploadFileIfExistsAsync(_hostingEnvironment);
                if (!uploadResult.Success)
                {
                    ModelState.AddModelError(nameof(CreateIndividualViewModel.Image), string.Join("\r\n", uploadResult.ErrorMessages));
                    return View(individual);
                }

                individual.Image = uploadResult.Model;
            }

            var individualEntity = CreateIndividualViewModel.Projection(individual);
            var createResult = await _repository.CreateEntityAsync(individualEntity);
            if (!createResult.Success)
            {
                ModelState.AddModelError("", string.Join("\r\n", createResult.ErrorMessages));
                return View(individual);
            }

            return RedirectToAction("Details", new { id = createResult.Model.Id });
        }

        [HttpPost]
        [ValidateModelState]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateIndividualViewModel individual)
        {
            var individualEntityResult = await _repository.GetEntityAsync<Individual>
                (individual.Id, nameof(Individual.PhoneNumbers), nameof(Individual.RelatedIndividuals));

            if (!individualEntityResult.Success)
            {
                ModelState.AddModelError("", string.Join("\r\n", individualEntityResult.ErrorMessages));
                return View(individual);
            }

            var individualEntity = individualEntityResult.Model;

            var files = HttpContext.Request.Form.Files;
            if (files != null && files.Count > 0)
            {
                var uploadResult = await files.UploadFileIfExistsAsync(_hostingEnvironment);
                if (!uploadResult.Success)
                {
                    ModelState.AddModelError(nameof(CreateIndividualViewModel.Image), string.Join("\r\n", uploadResult.ErrorMessages));
                    return View(individual);
                }

                _hostingEnvironment.DeleteFileIfExists(individual.Image);

                individual.Image = uploadResult.Model;
            }

            var updatedIndividualEntity = UpdateIndividualViewModel.UpdateProjection(individual, individualEntity);
            var updateResult = await _repository.UpdateEntityAsync(updatedIndividualEntity, Convert.FromBase64String(individual.RowVersion));
            if (!updateResult.Success)
            {
                ModelState.AddModelError("", string.Join("\r\n", updateResult.ErrorMessages));
                return View(individual);
            }

            return RedirectToAction("Details", new { id = updateResult.Model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteIndividualViewModel individual)
        {
            var individualResult = await _repository.GetEntityAsync<Individual>(individual.Id);
            if (!individualResult.Success)
            {
                ModelState.AddModelError("", string.Join("\r\n", individualResult.ErrorMessages));
                return View(individual);
            }

            var individualEntity = individualResult.Model;
            var deleteResult = await _repository.DeleteEntityAsync(individualEntity);
            if (!deleteResult.Success)
            {
                ModelState.AddModelError("", string.Join("\r\n", individualResult.ErrorMessages));
                return View(individual);
            }

            _hostingEnvironment.DeleteFileIfExists(individual.Image);

            return RedirectToAction("Index");
        }

        #endregion

        #region HelperMethods

        [HttpGet]
        public IActionResult SetLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreatePhoneNumbersEntryRaw() =>  PartialView("_CreatePhoneNumbersEditorPartial");

        [HttpGet]
        public IActionResult EditPhoneNumbersEntryRaw() => PartialView("_EditPhoneNumbersEditorPartial");

        [HttpGet]
        public IActionResult CreateRelatedIndividualsEntryRaw() => PartialView("_CreateRelatedIndividualsEditorPartial");

        [HttpGet]
        public IActionResult EditRelatedIndividualsEntryRaw() => PartialView("_EditRelatedIndividualsEditorPartial");

        #endregion
    }
}
