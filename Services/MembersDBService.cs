using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
    // public async Task AddUser(string url)
    // {
    //     var User = new UserModel { Name = url };
    //     await _myContext.User.AddAsync(User);
    //     await _myContext.SaveChangesAsync();   
    // }    
    
    public async Task<bool> PostMember(UserModel newMember)
    {
        try
        {
            //把沒有的補進去 new guid... 
            //密碼 hash
            //建立時間 ...
            newMember.Name="123";
            await _myContext.User.AddAsync(newMember);
            await _myContext.SaveChangesAsync();  
            return true;
        }
        catch (DbUpdateException  e)
        {
            throw new DbUpdateException (e.Message.ToString());
        }
    }   
    //FindIndex不用
    //可能會用 select where (箭頭函示的寫法)
    public async Task<List<UserModel>> GetMember()
    {
        try
        {
            
            //email 方法傳進來
            //非同步查詢符合的第一筆資料 orm efcored1 (lineq Where)
            // return await  _myContext.User.Where.(p=>p.Email==email).firs.....+Async\

            return await _myContext.User.ToListAsync(); //查全部
        }
        catch (DbUpdateException  e)
        {
            throw new DbUpdateException (e.Message.ToString());
        }
    }
    public async Task DeleteMember(UserModel newMember)
    {
        try
        {
            _myContext.Model.Name
            _myContext.Remove(newMember);
            await _myContext.SaveChangesAsync();  
            // return await _myContext.User.ToListAsync();
        }
        catch (DbUpdateException  e)
        {
            throw new DbUpdateException (e.Message.ToString());
        }
    }
    public async Task<List<UserModel>> PutMember(UserModel newMember)
    {
        try
        {
            _myContext.Update(newMember);
            await _myContext.SaveChangesAsync();  
            return await _myContext.User.ToListAsync();
        }
        catch (DbUpdateException  e)
        {
            throw new DbUpdateException (e.Message.ToString());
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


