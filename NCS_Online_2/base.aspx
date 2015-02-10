<%@ Page Title="Клиенты" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="base.aspx.cs" Inherits="NCS_Online_2._base" %>
<%@ MasterType  virtualPath="~/Site.Master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:TextBox ID="request" runat="server" TextMode="SingleLine" Width="922px" 
    Height="21px" Visible="False"></asp:TextBox>
    
       <div class="maintableStyles">
    
     <asp:Label ID="noUser" runat="server" Font-Size="10pt"></asp:Label>

     <asp:GridView ID="GridView1" Width="100%"  runat="server" AutoGenerateColumns="True" 
        DataSourceID="SqlDataSource1" CellPadding="4" ForeColor="#333333" AllowSorting="True" 
        onselectedindexchanged="GridView1_SelectedIndexChanged" 
        BorderStyle="None" onpageindexchanged="GridView1_PageIndexChanged" 
            AutoGenerateSelectButton="True" onsorting="GridView1_Sorting">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
      
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Wrap="True" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
   <asp:Label ID="count" runat="server" Font-Size="10pt"></asp:Label>
    
    
    </div>
    <div class="phonesStyle">
       <h3>Актуальные телефоны</h3>
       <asp:TextBox ID="clientPhones" runat="server" ReadOnly="True" 
           TextMode="MultiLine" Width="280px"  CssClass="textBoxesAddressPhones"></asp:TextBox>           
    
            </div>
   
   <div class="adressStyle">
        <h3>Адреса клиента:</h3>   
       <asp:TextBox ID="clientAddress" runat="server" ReadOnly="True" 
           TextMode="MultiLine" Width="280px"  CssClass="textBoxesAddressPhones"></asp:TextBox>   
   </div>

 <div class="tableStyles">


        <asp:Label ID="clientName" runat="server" BorderStyle="Groove" 
            BorderWidth="2px" Font-Bold="True" Font-Size="12pt">Клиент</asp:Label>


        <asp:GridView ID="GridView2" Width="100%" runat="server" AllowSorting="True"  
            DataSourceID="SqlDataSource2" BorderStyle="Solid" CellPadding="4" 
            ForeColor="#333333" BorderWidth="1px" CssClass="infoStyle" >
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
           
        </asp:GridView>
    


    </div>
      
   


    <div class="addComment" id="commentDiv" runat="server" > 
    <h3>Добавить комментарий</h3>
    
    <asp:Table ID="Table1" runat="server" 
            GridLines="Both"  Width="100%" Height="165px">
        <asp:TableRow runat="server">
            <asp:TableCell runat="server">Действие</asp:TableCell>
            <asp:TableCell runat="server"><asp:DropDownList  runat="server" Width="150px" ID="actionDrop" onselectedindexchanged="actionDrop_SelectedIndexChanged" AutoPostBack="False"></asp:DropDownList></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow2" runat="server">
            <asp:TableCell ID="TableCell3" runat="server">Контакт с</asp:TableCell>
            <asp:TableCell ID="TableCell4" runat="server"><asp:DropDownList  runat="server" Width="150px" ID="contactWithDrop"></asp:DropDownList></asp:TableCell>
        </asp:TableRow>

        <asp:TableRow ID="TableRow6" runat="server">
            <asp:TableCell ID="TableCell8" runat="server">Номер</asp:TableCell>
            <asp:TableCell ID="TableCell9" runat="server"><asp:DropDownList  runat="server" Width="150px" ID="phonesDrop"></asp:DropDownList></asp:TableCell>
        </asp:TableRow>

         <asp:TableRow ID="TableRow1" runat="server">
            <asp:TableCell ID="TableCell1" runat="server">Результат</asp:TableCell>
            <asp:TableCell ID="TableCell2" runat="server"><asp:DropDownList  runat="server" Width="150px" ID="resultDrop"></asp:DropDownList></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow3" runat="server">
            <asp:TableCell ID="TableCell5" runat="server">Комментарий</asp:TableCell>          
        </asp:TableRow>

        <asp:TableRow ID="TableRow4" runat="server">
            <asp:TableCell ID="TableCell6" runat="server" ColumnSpan="10"><asp:TextBox runat="server" ID="comemntTextInTable" TextMode="MultiLine" Width="97%" Height="80"></asp:TextBox></asp:TableCell> 
        </asp:TableRow>
        <asp:TableRow ID="TableRow5" runat="server">
            <asp:TableCell ID="TableCell7" runat="server" ColumnSpan="10"><asp:Button runat="server" Text="Добавить действие" ID="saveComment" Width="100%" onclick="saveComment_Click" /></asp:TableCell> 
        </asp:TableRow>
        </asp:Table>
       
        
        
        </div>


    <div class="addPhone" id="addPhoneDiv" runat="server" >
    <h3>Добавить телефон</h3>
    
    <asp:Table ID="Table2" runat="server" 
            GridLines="Both"  Width="100%" Height="85px">
        <asp:TableRow ID="TableRow7" runat="server">
            <asp:TableCell ID="TableCell10" runat="server">Тип телефона</asp:TableCell>
            <asp:TableCell ID="TableCell11" runat="server"><asp:DropDownList  runat="server" Width="150px" ID="phoneTypeDrop" ></asp:DropDownList></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow8" runat="server">
            <asp:TableCell ID="TableCell12" runat="server">Номер телефона</asp:TableCell>
            <asp:TableCell ID="TableCell13" runat="server"><asp:TextBox runat="server" Width="145px" ID="newPhoneTextbox"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
            
        <asp:TableRow ID="TableRow9" runat="server">
            <asp:TableCell ID="TableCell14" runat="server" ColumnSpan="10"><asp:Button runat="server" Text="Добавить телефон" ID="savePhone" Width="100%" onclick="savePhone_Click" /></asp:TableCell> 
        </asp:TableRow>
        </asp:Table>
        



    </div>


   

   
   



   
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:mainConnect %>" 
        onselecting="SqlDataSource1_Selecting" 
        ProviderName="<%$ ConnectionStrings:mainConnect.ProviderName %>">
        <SelectParameters>
            <asp:Parameter Name="Login" />
        </SelectParameters>
        </asp:SqlDataSource>


    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
        ConnectionString="<%$ ConnectionStrings:mainConnect %>" 
        ProviderName="<%$ ConnectionStrings:mainConnect.ProviderName %>" >
       
       
        </asp:SqlDataSource>
    <br />
    </asp:Content>
