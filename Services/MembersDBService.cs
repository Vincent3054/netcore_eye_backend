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
{   
    private readonly MyContext _myContext;

    public MembersDBService(MyContext Context)
    {
        this._myContext = Context; 

    }

    //註冊新會員方法
    public async Task AddUser(string url)
    {
        var User = new UserModel { Name = url };
        await _myContext.User.AddAsync(User);
        await _myContext.SaveChangesAsync();   
    }    

        public static async Task AddBlogAsync(string url)
        {
            using (var context = new BloggingContext())
            {
                var blog = new Blog { Url = url };
                context.Blogs.Add(blog);
                await context.SaveChangesAsync();
            }
        }
    #region 註冊
    public void Register(UserViewModel newMember)
        {
            //Mapper.CreateMap<UserModel, UserViewModel>();
            // var viewModel2 = Mapper.Map<List<UserViewModel>>(sql.Post.ToList());

            //產生member_Id
            // newMember.Member_Id = GetGUID();
            //將密碼Hash過  
            // newMember.Password = HashPassword(newMember.Password);

            string sql = $@"INSERT INTO member  values('{newMember.Email}','{newMember.Passsword}','{newMember.Name}','0')";
            //string sql = $@" INSERT INTO Members (Email,Passsword, Name,Role) VALUES ('{newMember.Email}','{newMember.Passsword}','{newMember.Name}','0') ";

            try  //確保程式不會因執行錯誤而整個中斷
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
}


