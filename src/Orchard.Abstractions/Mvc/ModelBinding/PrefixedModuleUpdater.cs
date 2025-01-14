﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.ModelBinding;
using System.Linq.Expressions;

namespace Orchard.DisplayManagement.ModelBinding
{
    public class PrefixedModelUpdater : IUpdateModel
    {
        private readonly IUpdateModel _updateModel;

        public PrefixedModelUpdater(IUpdateModel updateModel) : this(updateModel, x => x)
        {
        }

        public PrefixedModelUpdater(IUpdateModel updateModel, Func<string, string> prefix)
        {
            _updateModel = updateModel;
            Prefix = prefix;
        }

        public ModelStateDictionary ModelState => _updateModel.ModelState;

        public Func<string, string> Prefix { get; set; }

        public Task<bool> TryUpdateModelAsync(object model, Type modelType, string prefix)
        {
            return _updateModel.TryUpdateModelAsync(model, modelType, Prefix(prefix));
        }

        public Task<bool> TryUpdateModelAsync<TModel>(TModel model, string prefix, params Expression<Func<TModel, object>>[] includeExpressions) where TModel : class
        {
            return _updateModel.TryUpdateModelAsync(model, Prefix(prefix), includeExpressions);
        }

        public bool TryValidateModel(object model)
        {
            return _updateModel.TryValidateModel(model);
        }

        public bool TryValidateModel(object model, string prefix)
        {
            return TryValidateModel(model, Prefix(prefix));
        }
    }
}
