using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sitecore;
using Sitecore.Data;
using Sitecore.ExperienceAnalytics.Client;
using Sitecore.ExperienceAnalytics.Client.Mvc.Presentation;
using Sitecore.ExperienceAnalytics.Core.Diagnostics;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Globalization;

namespace OneNorth.ExperienceAnalyticsTableControl.Mvc.Presentation
{
    public class ExperienceAnalyticsTableControlViewModel : DvcRenderingModel
    {
        private readonly ILogger logger;

        public string ColumnsItemID { get; protected set; }

        public string DataSource { get; protected set; }

        public ExperienceAnalyticsTableControlViewModel()
        {
            UseTimeResolution = false;
            logger = ClientContainer.GetLogger();
        }

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);

            Control.Class = "sc-ExperienceAnalyticsListControl";
            Control.Requires.Script("/sitecore/shell/client/Applications/ExperienceAnalytics/Common/Layouts/Renderings/ExperienceAnalyticsTableControl.js");
            Control.HasNestedComponents = true;

            var keysCount = Control.GetInt("KeysCount", 0) == 0 ? 10 : Control.GetInt("KeysCount", 0);

            var keysSortByMetric = Control.GetString("KeysSortByMetric");
            var keysSortDirectionId = Control.GetString("KeysSortDirection");
            var keysSortDirection = string.IsNullOrEmpty(keysSortDirectionId) ? GetTextValue(Sitecore.ExperienceAnalytics.Client.Globals.SortDirection.Descending) : GetTextValue(new ID(keysSortDirectionId));

            if (!ID.IsID(Control.DataSource))
                throw new ApplicationException(string.Format("Please assign a DataSource for the '{0}' rendering.", rendering.Parameters["Id"]));
            var dataSourceItem = Context.Database.GetItem(new ID(Control.DataSource));
            if (dataSourceItem == null)
                throw new ApplicationException(string.Format("Cannot find DataSource '{0}' for the '{1}' rendering.", Control.DataSource, rendering.Parameters["Id"]));
            ColumnsItemID = dataSourceItem.ID.ToString(); //.Fields["{C5C0EECC-E1AC-42D3-AB46-0628B227FB60}"].Value;

            DataSource = Control.DataSource;
            Control.Attributes["data-sc-datasource"] = Control.DataSource;

            var columns = Context.Database.GetItems(ColumnsItemID);
            var dictionary = new Dictionary<string, object>();
            foreach (var column in columns)
            {
                string dataFieldValue = GetDataFieldValue(column.ID);
                object numberScaleObject = GetNumberScaleObject(column.ID);
                if (numberScaleObject != null)
                    dictionary.Add(dataFieldValue, numberScaleObject);
            }
            Control.Attributes.Add("data-sc-columnfields", JsonConvert.SerializeObject(dictionary, Formatting.Indented, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));

            Control.Attributes["data-sc-keygrouping"] = GetKeyGrouping(Control);
            Control.Attributes["data-sc-componenttype"] = GetTextValue(Sitecore.ExperienceAnalytics.Client.Globals.System.Texts.List);
            Control.DataBind = "visible: isVisible";

            var targetPageId = Control.GetString("TargetPage");
            var hasTargetPageId = true;
            var hasTargetPage = true;

            if (!string.IsNullOrEmpty(targetPageId))
            {
                hasTargetPageId = ID.IsID(targetPageId);
                if (hasTargetPageId)
                {
                    var targetPage = Context.Database.GetItem(new ID(targetPageId));
                    hasTargetPage = targetPage != null;
                    if (hasTargetPage)
                        Control.Attributes.Add("data-sc-targetpageurl", LinkManager.GetItemUrl(targetPage));
                    else
                        logger.Warn("The TargetPage does not contain a valid item id for controlid: " + Control.ControlId);
                }
                else
                    logger.Warn("The TargetPage does not contain a valid item id for controlid: " + Control.ControlId);
            }
            if (!string.IsNullOrEmpty(keysSortByMetric))
                Control.Attributes.Add("data-sc-orderkeysby", GetDataFieldValue(new ID(keysSortByMetric)));
            Control.Attributes.Add("data-sc-keysdirection", keysSortDirection);
            Control.Attributes.Add("data-sc-keyscount", keysCount.ToString(CultureInfo.InvariantCulture));
            var errorTexts = new
            {
                GenericServerError = GetTextValue(Sitecore.ExperienceAnalytics.Client.Globals.System.Texts.ErrorMessages.GenericServerError),
                GenericServerWarning = GetTextValue(Sitecore.ExperienceAnalytics.Client.Globals.System.Texts.ErrorMessages.GenericServerWarning),
                NotAllowedCharacters = GetTextValue(Sitecore.ExperienceAnalytics.Client.Globals.System.Texts.ErrorMessages.NotAllowedCharacters),
                WrongTargetPage = !hasTargetPage || !hasTargetPageId ? GetTextValue(Sitecore.ExperienceAnalytics.Client.Globals.System.Texts.ErrorMessages.GenericServerWarning) : string.Empty
            };
            Control.Attributes.Add("data-sc-errortexts", JsonConvert.SerializeObject(errorTexts));
        }
    }
}