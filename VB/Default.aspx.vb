Imports DevExpress.Data.Filtering
Imports DevExpress.Web
Imports System
Imports System.Collections.Generic
Imports System.Linq

Partial Public Class _Default
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		If Not IsPostBack Then
			ASPxGridView1.DataBind()
		End If
	End Sub

	Protected Sub ASPxGridView1_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
		ASPxGridView1.DataSource = Invoice.GetData()
	End Sub

	Protected Sub ASPxGridView1_CustomColumnDisplayText(ByVal sender As Object, ByVal e As ASPxGridViewColumnDisplayTextEventArgs)
		If DirectCast(e.GetFieldValue("Id"), Integer) Mod 2 = 0 Then
			e.DisplayText = String.Empty
		End If
	End Sub

	Protected Sub ASPxGridView1_CustomCallback(ByVal sender As Object, ByVal e As ASPxGridViewCustomCallbackEventArgs)
		Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
		If e.Parameters <> "patchFilterExpression" OrElse String.IsNullOrEmpty(grid.FilterExpression) Then
			Return
		End If
		Dim fieldsToReplace = GetFieldsToReplace(grid)
		Dim criteria = CriteriaHelper.ReplaceCriteriaByFieldName(fieldsToReplace, CriteriaOperator.Parse(grid.FilterExpression))
		grid.FilterExpression = criteria.ToString()
	End Sub

	Protected Sub ASPxGridView1_CustomJSProperties(ByVal sender As Object, ByVal e As ASPxGridViewClientJSPropertiesEventArgs)
		Dim grid As ASPxGridView = TryCast(sender, ASPxGridView)
		Dim filterControlID As String = String.Format("{0}$DXPFCForm$DXPFC", grid.UniqueID)
		If Request("__CALLBACKID") IsNot Nothing AndAlso Request("__CALLBACKID").Equals(filterControlID) Then
			e.Properties("cpRequirePatchFilterExpression") = True
		End If
	End Sub

	Private Function GetFieldsToReplace(ByVal grid As ASPxGridView) As List(Of String)
		Return grid.DataColumns.Where(Function(c) c.Settings.FilterMode = ColumnFilterMode.DisplayText).Select(Function(c) c.FieldName).ToList()
	End Function

End Class