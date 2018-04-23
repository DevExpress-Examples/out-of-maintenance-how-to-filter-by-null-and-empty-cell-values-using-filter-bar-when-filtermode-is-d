Imports DevExpress.Data.Filtering
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Public Class CriteraVisitorBase
	Implements IClientCriteriaVisitor(Of CriteriaOperator)

	Public Sub New()
	End Sub

	#Region "IClientCriteriaVisitor Members"

	Public Overridable Function Visit(ByVal theOperand As JoinOperand) As CriteriaOperator Implements IClientCriteriaVisitor(Of CriteriaOperator).Visit
		Dim condition As CriteriaOperator = TryCast(theOperand.Condition.Accept(Me), CriteriaOperator)
		Dim expression As CriteriaOperator = TryCast(theOperand.AggregatedExpression.Accept(Me), CriteriaOperator)
		If Object.ReferenceEquals(condition, Nothing) OrElse Object.ReferenceEquals(expression, Nothing) Then
			Return Nothing
		End If
		Return New JoinOperand(theOperand.JoinTypeName, condition, theOperand.AggregateType, expression)
	End Function

	Public Overridable Function Visit(ByVal theOperand As OperandProperty) As CriteriaOperator Implements IClientCriteriaVisitor(Of CriteriaOperator).Visit
		Return theOperand
	End Function

	Public Overridable Function Visit(ByVal theOperand As AggregateOperand) As CriteriaOperator Implements IClientCriteriaVisitor(Of CriteriaOperator).Visit
		Dim operand As OperandProperty = TryCast(theOperand.CollectionProperty.Accept(Me), OperandProperty)
		Dim condition As CriteriaOperator = TryCast(theOperand.Condition.Accept(Me), CriteriaOperator)
		Dim expression As CriteriaOperator = TryCast(theOperand.AggregatedExpression.Accept(Me), CriteriaOperator)
		If Object.ReferenceEquals(condition, Nothing) OrElse Object.ReferenceEquals(expression, Nothing) OrElse Object.ReferenceEquals(operand, Nothing) Then
			Return Nothing
		End If
		Return New AggregateOperand(operand, expression, theOperand.AggregateType, condition)
	End Function
	#End Region

	#Region "ICriteriaVisitor Members"

	Public Overridable Function Visit(ByVal theOperator As FunctionOperator) As CriteriaOperator Implements DevExpress.Data.Filtering.ICriteriaVisitor(Of CriteriaOperator).Visit
		Dim operators As New List(Of CriteriaOperator)()
		For Each op As CriteriaOperator In theOperator.Operands
			Dim temp As CriteriaOperator = op.Accept(Of CriteriaOperator)(Me)
			If Object.ReferenceEquals(temp, Nothing) Then
				Return Nothing
			End If
			operators.Add(temp)
		Next op
		Return New FunctionOperator(theOperator.OperatorType, operators)
	End Function

	Public Overridable Function Visit(ByVal theOperand As OperandValue) As CriteriaOperator Implements DevExpress.Data.Filtering.ICriteriaVisitor(Of CriteriaOperator).Visit
		Return theOperand
	End Function

	Public Overridable Function Visit(ByVal theOperator As GroupOperator) As CriteriaOperator Implements DevExpress.Data.Filtering.ICriteriaVisitor(Of CriteriaOperator).Visit
		Dim operators As New List(Of CriteriaOperator)()
		For Each op As CriteriaOperator In theOperator.Operands
			Dim temp As CriteriaOperator = op.Accept(Of CriteriaOperator)(Me)
			If Object.ReferenceEquals(temp, Nothing) Then
				Continue For
			End If
			operators.Add(temp)
		Next op
		Return New GroupOperator(theOperator.OperatorType, operators)
	End Function

	Public Overridable Function Visit(ByVal theOperator As InOperator) As CriteriaOperator Implements DevExpress.Data.Filtering.ICriteriaVisitor(Of CriteriaOperator).Visit
		Dim leftOperand As CriteriaOperator = theOperator.LeftOperand.Accept(Of CriteriaOperator)(Me)
		Dim operators As New List(Of CriteriaOperator)()
		For Each op As CriteriaOperator In theOperator.Operands
			Dim temp As CriteriaOperator = op.Accept(Of CriteriaOperator)(Me)
			If Object.ReferenceEquals(temp, Nothing) Then
				Continue For
			End If
			operators.Add(temp)
		Next op
		If Object.ReferenceEquals(leftOperand, Nothing) Then
			Return Nothing
		End If
		Return New InOperator(leftOperand, operators)
	End Function

	Public Overridable Function Visit(ByVal theOperator As UnaryOperator) As CriteriaOperator Implements DevExpress.Data.Filtering.ICriteriaVisitor(Of CriteriaOperator).Visit
		Dim operand As CriteriaOperator = theOperator.Operand.Accept(Of CriteriaOperator)(Me)
		If Object.ReferenceEquals(operand, Nothing) Then
			Return Nothing
		End If
		Return New UnaryOperator(theOperator.OperatorType, operand)
	End Function

	Public Overridable Function Visit(ByVal theOperator As BinaryOperator) As CriteriaOperator Implements DevExpress.Data.Filtering.ICriteriaVisitor(Of CriteriaOperator).Visit
		Dim leftOperand As CriteriaOperator = theOperator.LeftOperand.Accept(Of CriteriaOperator)(Me)
		Dim rightOperand As CriteriaOperator = theOperator.RightOperand.Accept(Of CriteriaOperator)(Me)
		If Object.ReferenceEquals(leftOperand, Nothing) OrElse Object.ReferenceEquals(rightOperand, Nothing) Then
			Return Nothing
		End If
		Return New BinaryOperator(leftOperand, rightOperand, theOperator.OperatorType)
	End Function

	Public Overridable Function Visit(ByVal theOperator As BetweenOperator) As CriteriaOperator Implements DevExpress.Data.Filtering.ICriteriaVisitor(Of CriteriaOperator).Visit
		Dim test As CriteriaOperator = theOperator.TestExpression.Accept(Of CriteriaOperator)(Me)
		Dim begin As CriteriaOperator = theOperator.BeginExpression.Accept(Of CriteriaOperator)(Me)
		Dim [end] As CriteriaOperator = theOperator.EndExpression.Accept(Of CriteriaOperator)(Me)
		If Object.ReferenceEquals(test, Nothing) OrElse Object.ReferenceEquals(begin, Nothing) OrElse Object.ReferenceEquals([end], Nothing) Then
			Return Nothing
		End If
		Return New BetweenOperator(test, begin, [end])
	End Function
	#End Region
End Class