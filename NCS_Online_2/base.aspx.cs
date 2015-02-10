using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
namespace NCS_Online_2
{




    public partial class _base : System.Web.UI.Page
    {
       
        string user = "";
        Int64 userID=0;

        //Когда будут меняться столбцы в таблице, обязательно надо заменить эти поля:

        int placeofcontractid = 1;
        int placeofclientid = 2;
        int placeofclientname = 3;

        //Когда будут меняться столбцы в таблице, обязательно надо заменить эти поля:

        bool isFieldCur = false;
        bool isMainCur = false;


        //для того, что бы дать пользователю необходимые права, необходимо в базе Collect дабавить в таблицу UserRole
        //запись, соответственно особым правам пользователя, 15-региональный куратор, 16-главный куратор компании
        private bool isMainCurator() {

           

            bool result = false;
            int temp = 0;
            SqlConnection conny = new SqlConnection(SqlDataSource1.ConnectionString);
            conny.Open();
            SqlCommand commandy = conny.CreateCommand();

            commandy.CommandText = "select Count(*) from [User] left join [UserRole] on [User].id=UserRole.UserId where UserRole.RoleId=16  and [User].[Login]='" + user + "'";

            //commandy.CommandText = "select Count(*) from [User] where isManager=1 and [Login]='" + user + "'";

            temp = (int)commandy.ExecuteScalar();
            conny.Close();


            if (temp > 0) result = true;

            return result;
        }

        private bool isFieldCurator() {
            bool result = false;
            int temp = 0;
            SqlConnection conny = new SqlConnection(SqlDataSource1.ConnectionString);
            conny.Open();
            SqlCommand commandy = conny.CreateCommand();

            commandy.CommandText = "select Count(*) from [User] left join [UserRole] on [User].id=UserRole.UserId where UserRole.RoleId=15  and [User].[Login]='" + user + "'";  

            //commandy.CommandText = "select Count(*) from [User] where isManager=1 and [Login]='" + user + "'";

            temp = (int)commandy.ExecuteScalar();
            conny.Close();


            if (temp > 0) result = true;

            return result;
        
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            
             user = HttpContext.Current.User.Identity.Name;
             user = user.Substring(6);
             //user = "atischenko";//куратор над кураторами, видит всех
             //user = "SOskoma";//куратор,видит клиентов своих подчиненных
             SqlDataSource1.SelectParameters["Login"].DefaultValue = user;
            //if(SqlDataSource1.SelectParameters.Count<1)SqlDataSource1.SelectParameters.Add("Login", user);
             noUser.Visible = true;           

            
            try {


                SqlConnection connx = new SqlConnection(SqlDataSource1.ConnectionString);
                connx.Open();
                SqlCommand commandx = connx.CreateCommand();


                commandx.CommandText = "Select id from [User] where Login='"+user+"'";

                userID = (Int64)commandx.ExecuteScalar();
                connx.Close();


            }
            catch {
                noUser.Text = "Не могу найти UserId";
            }

            isMainCur = isMainCurator();
            isFieldCur = isFieldCurator();



            if (request.Text.Length < 1) setRequestText();

            updateRequestToDB(); //загружать ли таблицу сразу при загрузке страницы
            // makeDropLists();

            count.Text = "Всего "+GridView1.Rows.Count.ToString()+" записей";

        }
        
        private void updateRequestToDB() {
            
            SqlDataSource1.SelectCommand = request.Text;
            GridView1.DataBind();
            
        }

        private void setRequestText() {





            if (isMainCur)
                //если это главный куратор(сейчас Тищенко) то он видит всех клиентов всех регионов
            {
                request.Text = @"SET CONCAT_NULL_YIELDS_NULL OFF  SELECT id, ClientId, ClientSurname+' '+ClientName+' '+ClientMiddleName as [Name],
convert(varchar(10),Birthday,104) as Birthday,CONVERT(varchar(15),SummToClose)+' '+Currency as SummToCLose,ReestrName,LastPaymentDate,IsZalog,StringTaskGoal, 
NameWorker FROM [FieldContactsFull] SET CONCAT_NULL_YIELDS_NULL ON";

            }

            else if (isFieldCur)
            {
                //если это куратор определенного региона, то он видит всех клиентов своих подчиненных по регионам           
                request.Text = @"SET CONCAT_NULL_YIELDS_NULL OFF  SELECT id, ClientId, ClientSurname+' '+ClientName+' '+ClientMiddleName as [Name],
convert(varchar(10),Birthday,104) as Birthday,CONVERT(varchar(15),SummToClose)+' '+Currency as SummToCLose,ReestrName,LastPaymentDate,IsZalog,StringTaskGoal, 
NameWorker FROM [FieldContactsFull]  WHERE ([CuratorId] = " + userID + ") SET CONCAT_NULL_YIELDS_NULL ON";

            }

            else
            {
                //если это простой пользователь, то он видит только своих клиентов
                request.Text = @"SET CONCAT_NULL_YIELDS_NULL OFF  SELECT id, ClientId, ClientSurname+' '+ClientName+' '+ClientMiddleName as [Name],
convert(varchar(10),Birthday,104) as Birthday,CONVERT(varchar(15),SummToClose)+' '+Currency as SummToCLose,ReestrName,LastPaymentDate,IsZalog,StringTaskGoal FROM [FieldContactsFull]  
WHERE ([UserId] = "+userID+") SET CONCAT_NULL_YIELDS_NULL ON";

            }

            //request.Text = "SELECT id,ClientSurname+' '+ClientName+' '+ClientMiddleName as [Name],Passport,PassportInfo,convert(varchar(10),Birthday,104) as Birthday,CONVERT(varchar(15),SummToClose)+' '+Currency as SummToCLose,ClientId,ReestrName,LastPaymentDate,IsZalog,StringTaskGoal FROM [FieldContactsFull]  WHERE ([Login] = @Login)";

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {               
                updateRequestToDB();       
            }
            catch
            {

            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            noUser.Text = "";
            clientName.Text = GridView1.SelectedRow.Cells[placeofclientname].Text;
                 

            clientPhones.Text = getPhones();


            clientAddress.Text = getAddress();

            showInfo();

            makeDropLists();

        }       

       /* private string getNewComment() {

            string input = "";

                input = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss  ") + commentText.Text+"\n";
       
            return input;
        }
        */
        /*
        protected void saveComment_Click(object sender, EventArgs e)
        {
            if (commentText.Text.Length > 0 && checkIfSelected())
            {
                string comment = getNewComment();


                SqlConnection conn = new SqlConnection(SqlDataSource1.ConnectionString);
                conn.Open();
                SqlCommand myCommand = conn.CreateCommand();



                myCommand.CommandText = "UPDATE [Contract] SET Comment = isnull(Comment,'') + ' " + comment + "' WHERE id='" + GridView1.SelectedRow.Cells[1].Text + "'";

                //по id идет выборка из Контрактов, еще надо сделать выборку телефонов по КлиентИД
                int save = myCommand.ExecuteNonQuery();
                if (save > 1)
                {
                    noUser.Text = "Внимание, изменение были внесены в " + save + " строк";
                }
                else if (save == 1)
                {
                    noUser.Text = "Изменение выполнено успешно";
                }
                else
                {
                    noUser.Text = "Сохранено не выполнено";
                }


                conn.Close();
                GridView1.DataBind();
                commentText.Text = "";
                GridView1_SelectedIndexChanged(null, null);
            }

            else
            {
                if(commentText.Text.Length<0)noUser.Text = "Укажите комментарий!";
                if (checkIfSelected() == false) noUser.Text += " Выберите клиента";
                
            }
            
        }
        */
        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {

        }

        protected void clientComment_TextChanged(object sender, EventArgs e)
        {

        }

       /* protected void saveNewPhone_Click(object sender, EventArgs e)
        {

            if (checkIfSelected() && newActualNumber.Text.Length > 0)
            {

                SqlConnection conn4 = new SqlConnection(SqlDataSource1.ConnectionString);
                conn4.Open();
                SqlCommand myCommand2 = conn4.CreateCommand();

                myCommand2.CommandText = "Select Count(*) from ClientContact where [ContactTypeId] = 230 AND [ClientId]=" + GridView1.SelectedRow.Cells[11].Text;
                int res = (int)myCommand2.ExecuteScalar();

                noUser.Text = res.ToString();
                if (res == 0)
                {
                    //если возвращается отсутствие полей таких-то создаем новое инсертом               


                    myCommand2.CommandText = "INSERT INTO ClientContact(ClientId,ContactTypeId,Value) Values(" + GridView1.SelectedRow.Cells[11].Text + ",230," + newActualNumber.Text + ")";

                    myCommand2.ExecuteNonQuery();

                    noUser.Text = "Телефон добавлен успешно";

                }


                else
                {
                    //если такая строка есть-то апдейт

                    myCommand2.CommandText = "UPDATE [ClientContact] SET Value =  ' " + newActualNumber.Text + "' WHERE ClientId='" + GridView1.SelectedRow.Cells[11].Text + "' AND ContactTypeId=230";

                    myCommand2.ExecuteNonQuery();
                    noUser.Text = "Телефон изменен";
                }



                conn4.Close();

                newActualNumber.Text = "";
            }
            else {

                if (checkIfSelected() == false) noUser.Text = "Выберите клиента";
                if (newActualNumber.Text.Length == 0) noUser.Text += " Введите номер телефона";
            
            }


        }        
        */
        private string getPhones() {
            
                string output = "";
                string result = "";
                try
                {
                    if (checkIfSelected())
                    {

                        SqlConnection conn3 = new SqlConnection(SqlDataSource1.ConnectionString);
                        conn3.Open();

                        SqlCommand command = conn3.CreateCommand();

                        command.CommandText = "SELECT  [Value], ContactTypeId FROM [ClientContact] WHERE (ContactStatusId is Null or ContactStatusId in(117,366188)) and [ClientId]=" + GridView1.SelectedRow.Cells[placeofclientid].Text;

                        SqlDataReader reader;
                        reader = command.ExecuteReader();

                        Int64 type;
                        while (reader.Read())
                        {

                            result = reader.GetString(0) + "\n";
                            type = (int)reader.GetInt64(1);


                            switch (type)
                            {
                                case 220:
                                    result = "Поручитель: " + result;

                                    break;
                                case 221:
                                    result = "Дополнительный2: " + result;
                                    break;
                                case 222:
                                    result = "Дополнительный1: " + result;
                                    break;
                                case 223:
                                    result = "По месту проживания: " + result;
                                    break;
                                case 224:
                                    result = "Рабочий2: " + result;
                                    break;
                                case 225:
                                    result = "Мобильный: " + result;
                                    break;
                                case 226:
                                    result = "По месту регистрации: " + result;
                                    break;
                                case 227:
                                    result = "Контактное лицо: " + result;
                                    break;
                                case 228:
                                    result = "Отдел кадров/бухгалтерия: " + result;
                                    break;
                                case 229:
                                    result = "Другой: " + result;
                                    break;
                                case 636066:
                                    result = "Best Phone PTP: " + result;
                                    break;
                                case 636065:
                                    result = "Best Phone RPC: " + result;
                                    break;
                                case 618456:
                                    result = "e-mail: " + result;
                                    break;



                            }

                            output += result;
                        }
                        conn3.Close();
                    }
                }
                catch { }
            return output;

        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {
            clearPage();

        }

        private bool checkIfSelected() {

            bool res;

            if (GridView1.SelectedIndex > -1) res = true;
            else res = false;


            return res;
        
        }

     


        private void showInfo() {


            SqlDataSource2.SelectCommand = "select u.Login as Login, a.ExecuteDate  as Date, d1.Name as Action, cc.Value as Phone, d2.Name as Result, cwd.Name as ContactWith, p.ExecutionDate as PayDate, p.Summ as PromisedSum, n.Description as Comment "
                + " from Activities a left join Dictionary d1 on a.ActivityTypeId=d1.Id left join Dictionary d2 on a.ResultId=d2.Id left join [User] u on a.ExecutorId=u.Id left join ContactWithDict cwd on a.ContactWithId=cwd.id left join Note n on a.NoteId=n.id left join ClientContact cc on a.ContactId = cc.id left join Promise p on a.id=p.ActivityId "
                + " where a.Contractid=" + GridView1.SelectedRow.Cells[placeofcontractid].Text + " order by a.ExecuteDate desc";


           GridView2.DataBind();
        
        
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            clearPage();
            
        }

        private void clearPage() {
            GridView1.SelectedIndex = -1;
            clientAddress.Text = "";
            clientPhones.Text = "";
            clientName.Text = "";
            SqlDataSource2.SelectCommand = "";
            GridView2.DataBind();
            noUser.Text = "";
        
        }

       
        private void makeDropLists() {
                commentDiv.Style["visibility"] = "visible";
                actionDrop.Items.Clear();
                contactWithDrop.Items.Clear();
                resultDrop.Items.Clear();
                phonesDrop.Items.Clear();
                addPhoneDiv.Style["visibility"] = "visible";
                phoneTypeDrop.Items.Clear();

                SqlConnection conn6 = new SqlConnection(SqlDataSource2.ConnectionString);
                conn6.Open();

                SqlCommand command6 = conn6.CreateCommand();

                command6.CommandText = "select id,[Name] from Dictionary where specId=8";
                SqlDataReader reader;
                reader = command6.ExecuteReader();

                while (reader.Read())
                {

                    ListItem i=new ListItem(reader.GetString(1),reader.GetInt64(0).ToString());

                    actionDrop.Items.Add(i);

                   
                }


                conn6.Close();

                conn6.Open();

                command6.CommandText = "select id,[Name] from ContactWithDict";

                reader = command6.ExecuteReader();

                while (reader.Read())
                {

                    ListItem i = new ListItem(reader.GetString(1), reader.GetInt64(0).ToString());

                    contactWithDrop.Items.Add(i);


                }


                conn6.Close();

                conn6.Open();

                command6.CommandText = "select id,[Name] from Dictionary where specId=31";
                
                reader = command6.ExecuteReader();

                while (reader.Read())
                {

                    ListItem i = new ListItem(reader.GetString(1), reader.GetInt64(0).ToString());

                    resultDrop.Items.Add(i);


                }





                conn6.Close();

                conn6.Open();

                command6.CommandText = "SELECT id,[Value] FROM [ClientContact] WHERE (ContactStatusId is Null or ContactStatusId in(117,366188)) and [ClientId]=" + GridView1.SelectedRow.Cells[placeofclientid].Text;

                reader = command6.ExecuteReader();
                phonesDrop.Items.Add("");
                while (reader.Read())
                {

                    ListItem i = new ListItem(reader.GetString(1), reader.GetInt64(0).ToString());
                    phonesDrop.Items.Add(i);


                }
            

                conn6.Close();




                conn6.Open();

                command6.CommandText = "select id,[Name] from Dictionary where specId=15";

                reader = command6.ExecuteReader();

                while (reader.Read())
                {

                    ListItem i = new ListItem(reader.GetString(1), reader.GetInt64(0).ToString());


                    phoneTypeDrop.Items.Add(i);


                }


                conn6.Close();

           

        }



        private string getAddress()
        {


            string output = "";
            string result = "";
            int type = 0;
            try
            {
                if (checkIfSelected())
                {
                    SqlConnection conn5 = new SqlConnection(SqlDataSource1.ConnectionString);
                    conn5.Open();

                    SqlCommand command5 = conn5.CreateCommand();

                    command5.CommandText = "select Address,AddrTypeId from Address where id IN(SELECT AddressId from ClientAddress where ClientID=" + GridView1.SelectedRow.Cells[placeofclientid].Text + " )";
                    SqlDataReader reader;
                    reader = command5.ExecuteReader();

                    while (reader.Read())
                    {

                        result = reader.GetString(0) + "\n\n";
                        type = (int)reader.GetInt64(1);

                        if (type == 444)
                        {
                            result = "Адрес проживания: " + result;
                        }

                        else if (type == 445)
                        {
                            result = "Адрес регистрации: " + result;
                        }

                        else
                        {
                            result = "Доп. Адрес: " + result;
                        }

                        output += result;
                    }
                    conn5.Close();
                }
            }

            catch { }
            return output;


        }

        protected void saveComment_Click(object sender, EventArgs e)
        {
            //сначала вносится комментарий, берется его ИД и используется в таблице активитис
            if (checkIfSelected())
            {
                Int64 noteID = 0;
                try
                {
                    SqlConnection conn7 = new SqlConnection(SqlDataSource2.ConnectionString);
                    conn7.Open();
                    SqlCommand myCommand7 = conn7.CreateCommand();
                    //ВНОСИМ КОММЕНТАРИЙ в NOTE и Выбираем его ИД

                    myCommand7.CommandText = "INSERT INTO [Note]([Description])VALUES ('" + comemntTextInTable.Text + "')" +
                         "SELECT CONVERT(BigInt,SCOPE_IDENTITY()) AS [value] ";
                    noteID = (Int64)myCommand7.ExecuteScalar();
                    conn7.Close();


                    //Запись в Activities 
                    //добавить проверку на пустое поле в добавлении
                    string phone = phonesDrop.SelectedIndex == 0 ? "NULL" : phonesDrop.SelectedValue;
                    conn7.Open();
                    myCommand7.CommandText = "INSERT INTO Activities([ContractId],[ActivityTypeId],[ActivityDate],[ResultId],[ContactWithId],[UserId],[NoteId],[ExecutorId],[ExecuteDate],[ContactId])" +
                        " Values(" + GridView1.SelectedRow.Cells[placeofcontractid].Text + ", " + actionDrop.SelectedValue + ", GetDate(), " + resultDrop.SelectedValue + ", " + contactWithDrop.SelectedValue + ", " + userID + ", " + noteID + ", " + userID + ", GetDate(), " + phone + ")";
                    myCommand7.ExecuteNonQuery();
                    conn7.Close();
                    comemntTextInTable.Text = "";

                }

                catch
                {
                    comemntTextInTable.Text = "Ошибка при добавлении нового комментария";
                    return;
                }
                finally {
                    showInfo();
                }
            }
            else {
                noUser.Text = "Для добавления действия, сначала выберите клиента!";
            }
              
        }     
                
        protected void savePhone_Click(object sender, EventArgs e)
        {
            if (checkIfSelected())
            {
                try
                {
                    if (newPhoneTextbox.Text.Length > 4)
                    {
                        SqlConnection conn10 = new SqlConnection(SqlDataSource2.ConnectionString);
                        conn10.Open();
                        SqlCommand myCommand10 = conn10.CreateCommand();
                        myCommand10.CommandText = "INSERT INTO [ClientContact](ClientId,ContactTypeId,Value)" +
                            " Values(" + GridView1.SelectedRow.Cells[placeofclientid].Text + ", " + phoneTypeDrop.SelectedValue + ", " + newPhoneTextbox.Text + ")";
                        myCommand10.ExecuteNonQuery();
                        conn10.Close();
                        newPhoneTextbox.Text = "";
                    }
                }

                catch
                {
                    clientPhones.Text = "Ошибка при сохранении телефона";
                }
            }
            else {
                noUser.Text = "Для добавления телефона, сначала выберите клиента!";
            }
        }



        protected void actionDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

     
            




        //добавить поле выбора телефона, если в типе дейтвия был выбран звонок телефон
   }
}