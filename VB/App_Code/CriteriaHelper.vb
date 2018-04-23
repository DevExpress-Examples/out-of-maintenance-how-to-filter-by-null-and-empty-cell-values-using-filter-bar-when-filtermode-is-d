Imports DevExpress.Data.Filtering
Imports System.Collections.Generic

Public NotInheritable Class CriteriaHelper

	Private Sub New()
	End Sub

	Private Shared visitor_Renamed As ReplaceCriteriaVisitor
	Private Shared ReadOnly Property Visitor() As ReplaceCriteriaVisitor
		Get
			If visitor_Renamed Is Nothing Then
				visitor_Renamed = New ReplaceCriteriaVisitor()
			End If
			Return visitor_Renamed
		End Get
	End Property

	Public Shared Function ReplaceCriteriaByFieldName(ByVal fieldsToReplace As List(Of String), ByVal op As CriteriaOperator) As CriteriaOperator
		Return Visitor.ReplaceCriteriaByFieldName(fieldsToReplace, op)
	End Function
End Class

Public Class ReplaceCriteriaVisitor
	Inherits CriteraVisitorBase

	Private fieldsToReplace As List(Of String)
	Public Sub New()
	End Sub

	Public Function ReplaceCriteriaByFieldName(ByVal fieldsToReplace As List(Of String), ByVal op As CriteriaOperator) As CriteriaOperator
		If fieldsToReplace.Count = 0 Then
			Return Nothing
		End If
		Me.fieldsToReplace = fieldsToReplace
		Return op.Accept(Of CriteriaOperator)(Me)
	End Function

	Public Overrides Function Visit(ByVal theOperator As UnaryOperator) As CriteriaOperator
		Dim operand As CriteriaOperator = theOperator.Operand.Accept(Of CriteriaOperator)(Me)
		If Object.ReferenceEquals(operand, Nothing) Then
			Return Nothing
		End If

		If TypeOf operand Is OperandProperty Then
			Dim propertyName As String = (TryCast(operand, OperandProperty)).PropertyName
			If fieldsToReplace.Contains(propertyName) AndAlso theOperator.OperatorType = UnaryOperatorType.IsNull Then
				Return New FunctionOperator(FunctionOperatorType.IsNullOrEmpty, operand)
			End If
		End If
		Return MyBase.Visit(theOperator)
	End Function
End Class
