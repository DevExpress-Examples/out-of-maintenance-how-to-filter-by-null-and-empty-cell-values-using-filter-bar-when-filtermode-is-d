<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function OnEndCallback(s, e) {
            if (s.cpRequirePatchFilterExpression) {
                s.cpRequirePatchFilterExpression = null;
                s.PerformCallback("patchFilterExpression");
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <dx:ASPxGridView ID="ASPxGridView1" runat="server" ClientInstanceName="grid" KeyFieldName="Id" AutoGenerateColumns="False"
            OnDataBinding="ASPxGridView1_DataBinding" OnCustomColumnDisplayText="ASPxGridView1_CustomColumnDisplayText"
            OnCustomCallback="ASPxGridView1_CustomCallback" OnCustomJSProperties="ASPxGridView1_CustomJSProperties">

            <ClientSideEvents EndCallback="OnEndCallback" />
            <Settings ShowHeaderFilterButton="true" ShowFilterBar="Visible" />

            <Columns>
                <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True">
                    <EditFormSettings Visible="False" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataColumn FieldName="Description">
                    <Settings FilterMode="DisplayText" />
                </dx:GridViewDataColumn>
                <dx:GridViewDataTextColumn FieldName="Price">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="RegisterDate">
                    <Settings FilterMode="DisplayText" />
                </dx:GridViewDataDateColumn>
            </Columns>
        </dx:ASPxGridView>
    </form>
</body>
</html>
