using System.Text;

namespace ToDo.API.Email
{
    public class EmailTemplateReader
    {
        private readonly IWebHostEnvironment _environment;

        public EmailTemplateReader(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> ReadTemplateAsync(string templateName)
        {
            var path = Path.Combine(_environment.WebRootPath, "email", "templates", templateName);

            if (!File.Exists(path))
            {
                return string.Empty;
            }

            return await File.ReadAllTextAsync(path, Encoding.UTF8);
        }

        public async Task<string> ReadTemplateAsync(string templateName, IDictionary<string, object> templateValues)
        {
            templateName = templateName.EndsWith(".html") ? templateName : $"{templateName}.html";

            var path = Path.Combine(_environment.WebRootPath, "email", "templates", templateName);

            if (!File.Exists(path))
            {
                return string.Empty;
            }

            var emailTemplate = await File.ReadAllTextAsync(path, Encoding.UTF8);

            if (string.IsNullOrEmpty(emailTemplate))
            {
                return string.Empty;
            }

            foreach (var value in templateValues)
            {
                emailTemplate = emailTemplate.Replace($"{{{{{value.Key}}}}}", value.Value.ToString());
            }

            return emailTemplate;
        }
    }
}
