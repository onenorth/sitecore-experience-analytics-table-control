using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sitecore.Data;
using Sitecore.Services.Infrastructure.Web.Http;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Sitecore.Data.Fields;
using Sitecore.Services.Core;

namespace OneNorth.ExperienceAnalyticsTableControl.Api
{
    public class ExperienceAnalyticsTableControlController : ServicesApiController
    {

        [HttpGet]
        public IHttpActionResult Get(string datasource, string dateFrom, string dateTo, string siteName)
        {
            if (string.IsNullOrEmpty(datasource))
                throw new ApplicationException("datasource is not specified");

            var dataSourceItem = Database.GetDatabase("core").GetItem(new ID(datasource));
            if (dataSourceItem == null)
                throw new ApplicationException("datasource could not be found");

            var typeField = dataSourceItem.Fields["{9710F017-C2E6-486D-A8CB-91BB6A50E819}"];
            if (typeField == null)
                throw new ApplicationException("could not find Type field for datasource");

            var typeName = typeField.Value;
            if (string.IsNullOrEmpty(typeName))
                throw new ApplicationException("type not specified");

            var type = Type.GetType(typeName);
            if (type == null)
                throw new ApplicationException("Could not load type: " + typeName);

            var instance = Activator.CreateInstance(type);
            if (instance == null)
                throw new ApplicationException("type could not be instantiated: " + type.Name);

            var methodField = dataSourceItem.Fields["{78B2FBD7-07EA-419A-BBAF-5F23DF5DD84B}"];
            if (methodField == null)
                throw new ApplicationException("could not find Method field for datasource");

            var methodName = methodField.Value;
            if (string.IsNullOrEmpty(methodName))
                throw new ApplicationException("method not specified");

            var method = type.GetMethod(methodName);
            if (method == null)
                throw new ApplicationException("method could not be found: " + methodName);

            var nameValueListField = (NameValueListField) dataSourceItem.Fields["{340ED707-A5F2-4F8E-B0BD-F00693FDE654}"];
            if (nameValueListField == null)
                throw new ApplicationException("could not find Parameters field for datasource");

            var parameters = method.GetParameters()
                .Select(p =>
                {
                    if (string.Equals(p.Name, "datefrom", StringComparison.OrdinalIgnoreCase))
                        return Convert.ChangeType(DateTime.ParseExact(dateFrom, "dd-MM-yyyy", new DateTimeFormatInfo()), p.ParameterType);
                    if (string.Equals(p.Name, "dateto", StringComparison.OrdinalIgnoreCase))
                        return Convert.ChangeType(DateTime.ParseExact(dateTo, "dd-MM-yyyy", new DateTimeFormatInfo()), p.ParameterType);
                    if (string.Equals(p.Name, "sitename", StringComparison.OrdinalIgnoreCase))
                        return Convert.ChangeType(siteName, p.ParameterType);
                    return TypeDescriptor.GetConverter(p.ParameterType).ConvertFromInvariantString(HttpUtility.UrlDecode(nameValueListField.NameValues[p.Name]));
                }).ToArray();

            var response = method.Invoke(instance, parameters) as ExperienceAnalyticsTableControlResponse;
            if (response == null)
                throw new ApplicationException("Response is not of type ExperienceAnalyticsCustomReportResponse");

            var serializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            return new JsonResult<ExperienceAnalyticsTableControlResponse>(response, serializerSettings, Encoding.UTF8, this);
        }
    }
}