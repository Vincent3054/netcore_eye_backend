using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyWebsite;

public class MembersDBService
{    //建立與資料庫的連線字串    
    // private readonly static string cnstr = ConfigurationManager.ConnectionStrings["ASP.NET MVC"].ConnectionString;    //建立與資料庫的連線
    private readonly static string cnstr = WebConfigurationManager.ConnectionStrings["PSY"].ConnectionString;

    private readonly SqlConnection conn = new SqlConnection(cnstr);
    
    #region 註冊1   
 
    //註冊新會員方法
    public void Register()
    {
        
        Mapper.CreateMap<UserModel, UserViewModel>()
        // var viewModel2 = Mapper.Map<List<UserViewModel>>(sql.Post.ToList());


        //將密碼Hash過        
        // newMember.Password = HashPassword(newMember.Password);
        //sql新增語法 //IsAdmin 預設為0        
        // string sql = $@" INSERT INTO Members (Account,Password,Name, Email,AuthCode,IsAdmin) VALUES ('{newMember.Account}','{newMember.Password}','{newMember.Name}','{newMember.Email}','{newMember.AuthCode}','0') ";
        string sql = $@" INSERT INTO Members (Email,Passsword, Name,Role) VALUES ('{newMember.Email}','{newMember.Passsword}','{newMember.Name}','0') ";

        //確保程式不會因執行錯誤而整個中斷
        try
        {
            //開啟資料庫連線
            conn.Open();
            //執行Sql指令
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            //丟出錯誤 
            throw new Exception(e.Message.ToString());
        }
        finally
        {
            //關閉資料庫連線   
            conn.Close();
        }
    }
    #endregion
    #region 註冊2
     public void Register(Member newMember)
        {
            //產生member_Id
            // newMember.Member_Id = GetGUID();
            // newMember.Password = HashPassword(newMember.Password);
            string sql = $@"INSERT INTO member  values('{newMember.Member_Id}','{newMember.Account}','{newMember.Name}','{newMember.Email}','{newMember.Password}','{newMember.AuthCode}','{newMember.Phone}','{newMember.Sex}')";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
    #endregion
}


