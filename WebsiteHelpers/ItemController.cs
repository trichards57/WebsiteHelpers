﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebsiteHelpers.Interfaces;

namespace WebsiteHelpers
{
    public abstract class ItemController<TModel, TDetails, TCreate, TSummary> : Controller
        where TModel : class, IDetailable<TDetails>, ISummarisable<TSummary>
        where TCreate : class, ICreateViewModel<TModel>
    {
        public ItemController(IItemService<TModel> service)
        {
            Service = service;
        }

        protected IItemService<TModel> Service { get; }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            await Service.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(int id)
        {
            var item = await Service.GetAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item.ToDetail());
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            var items = await Service.GetAllAsync();

            if (items == null)
                return NotFound();

            return Ok(items.Select(i => i.ToSummary()));
        }

        [HttpPatch("{id}")]
        public virtual async Task<IActionResult> Patch(int id, [FromBody]JsonPatchDocument<TModel> patch)
        {
            var item = await Service.GetAsync(id);

            if (item == null)
                return NotFound();

            patch.ApplyTo(item, ModelState);

            var valContext = new ValidationContext(item);
            var valResults = new List<ValidationResult>();
            var valResult = Validator.TryValidateObject(item, valContext, valResults);

            foreach (var p in typeof(TModel).GetProperties())
            {
                valContext.MemberName = p.Name;
                valResult &= Validator.TryValidateProperty(p.GetValue(item), valContext, valResults);
            }

            foreach (var res in valResults)
                ModelState.AddModelError("", res.ErrorMessage);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await Service.UpdateAsync(item);

            return Ok(item.ToDetail());
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody]TCreate createModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = createModel.ToItem();

            var id = await Service.AddAsync(model);

            var newItem = await Service.GetAsync(id);

            return CreatedAtAction("Get", new { id }, newItem.ToDetail());
        }
    }
}