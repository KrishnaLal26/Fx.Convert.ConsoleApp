namespace Fx.Convert.Framework
{
    public static class UrlTemplateExtensions
    {
        public static string InjectParamValues(this string urlTemplate, params (string Key, string Value)[] parameters)
        {
            if (string.IsNullOrWhiteSpace(urlTemplate) || parameters == null || parameters.Length == 0)
            {
                return urlTemplate;
            }
            foreach (var (Key, Value) in parameters)
            {
                var valueKey = "{{"+Key+"}}";
                urlTemplate = urlTemplate.Replace(valueKey, Value);
            }
            return urlTemplate;
        }
    }
}
