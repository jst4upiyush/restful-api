using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Api.OpenApiHelpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AlsoProducesAttribute : Attribute
    {
        public AlsoProducesAttribute(int statusCode, string acceptHeader, Type producedType)
        {
            Statuscode = statusCode;
            AcceptHeader = acceptHeader;
            ProducedType = producedType;
        }

        public int Statuscode { get; }
        public string AcceptHeader { get; }
        public Type ProducedType { get; }
    }

    public class CustomMediaTypeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // 1. Get AlsoProducesAttribute for the invoked Method
            var alsoProducesAttributes = context.MethodInfo
                .GetCustomAttributes(typeof(AlsoProducesAttribute), true)
                .Select(attr => attr as AlsoProducesAttribute)
                .ToList();

            // 2. Add the MediaType and Response definition in the Generated documentation
            alsoProducesAttributes.ForEach(attr =>
            {
                var schema = context.SchemaGenerator.GenerateSchema(attr.ProducedType, context.SchemaRepository);
                operation.Responses[attr.Statuscode.ToString()].Content[attr.AcceptHeader]
                    = new OpenApiMediaType { Schema = schema };
            });
        }
    }
}
