using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using MediaTypeHeaderValue = Microsoft.Net.Http.Headers.MediaTypeHeaderValue;

namespace FinanceApi.Extensions
{
    class RawRequestBodyFormatter : InputFormatter
    {
        public RawRequestBodyFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(MediaTypeNames.Text.Plain));
        }

        public override bool CanRead(InputFormatterContext context)
        {
            var contentType = context.HttpContext.Request.ContentType;
            return string.IsNullOrEmpty(contentType) || contentType.StartsWith(MediaTypeNames.Text.Plain);
        }

        /// <inheritdoc/>
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            if (CanRead(context))
            {
                using var reader = new StreamReader(context.HttpContext.Request.Body);

                var content = await reader.ReadToEndAsync();
                return await InputFormatterResult.SuccessAsync(content);
            }

            return await InputFormatterResult.FailureAsync();
        }
    }
}