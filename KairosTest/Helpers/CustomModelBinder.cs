using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KairosTest.Helpers
{
    public class CustomBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(decimal))
            {
                return new DecimalModelBinder();
            }

            return null;
        }

        public class DecimalModelBinder : IModelBinder
        {
            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                if (valueProviderResult == null)
                {
                    return Task.CompletedTask;
                }

                var value = valueProviderResult.FirstValue;

                if (string.IsNullOrEmpty(value))
                {
                    return Task.CompletedTask;
                }

                // Remove unnecessary commas and spaces
                value = value.Replace(".", string.Empty).Trim();

                decimal myValue = 0;
                if (!decimal.TryParse(value, out myValue))
                {
                    // Error
                    bindingContext.ModelState.TryAddModelError(
                                            bindingContext.ModelName,
                                            "Must be a number.");
                    return Task.CompletedTask;
                }

                bindingContext.Result = ModelBindingResult.Success(myValue);
                return Task.CompletedTask;
            }
        }
    }
}
