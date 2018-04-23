Imports System
Imports System.Collections.Generic
Imports System.ComponentModel

Public Class Invoice
	Public Property Id() As Integer?
	Public Property Description() As String
	Public Property Price() As Decimal?
	Public Property RegisterDate() As Date?

	<DataObjectMethod(DataObjectMethodType.Select)>
	Public Shared Function GetData() As List(Of Invoice)
		Dim invoices As New List(Of Invoice)()
		Const count As Integer = 9

		For i As Integer = 0 To count - 1
			invoices.Add(New Invoice() With {.Id = i, .Description = "Invoice" & i.ToString(), .Price = i * 10, .RegisterDate = Date.Today.AddDays(i - count)})
		Next i

	   invoices.Add(New Invoice() With {.Id = count, .Description = Nothing, .Price = 100, .RegisterDate = Nothing})

		Return invoices
	End Function
End Class