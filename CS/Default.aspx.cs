using DevExpress.Data.Filtering;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class _Default : System.Web.UI.Page {

    protected void Page_Load(object sender, EventArgs e) {
        if (!IsPostBack)
            ASPxGridView1.DataBind();
    }

    protected void ASPxGridView1_DataBinding(object sender, EventArgs e) {
        ASPxGridView1.DataSource = Invoice.GetData();
    }

    protected void ASPxGridView1_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e) {
        if ((int)e.GetFieldValue("Id") % 2 == 0)
            e.DisplayText = string.Empty;
    }

    protected void ASPxGridView1_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e) {
        ASPxGridView grid = sender as ASPxGridView;
        if (e.Parameters != "patchFilterExpression" || string.IsNullOrEmpty(grid.FilterExpression))
            return;
        var fieldsToReplace = GetFieldsToReplace(grid);
        var criteria = CriteriaHelper.ReplaceCriteriaByFieldName(fieldsToReplace, CriteriaOperator.Parse(grid.FilterExpression));
        grid.FilterExpression = criteria.ToString();
    }

    protected void ASPxGridView1_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e) {
        ASPxGridView grid = sender as ASPxGridView;
        string filterControlID = String.Format("{0}$DXPFCForm$DXPFC", grid.UniqueID);
        if (Request["__CALLBACKID"] != null && Request["__CALLBACKID"].Equals(filterControlID))
            e.Properties["cpRequirePatchFilterExpression"] = true;
    }

    private List<string> GetFieldsToReplace(ASPxGridView grid) {
        return grid.DataColumns.Where(c => c.Settings.FilterMode == ColumnFilterMode.DisplayText).Select(c => c.FieldName).ToList();
    }

}