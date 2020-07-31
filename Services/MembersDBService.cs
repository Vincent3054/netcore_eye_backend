using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyWebsite;
namespace project.Services
{
    public class MembersDBService
    {
        private readonly MyContext _DBContext; //全域變數

        public MembersDBService(MyContext DBContext)//建構子
        {
            this._DBContext = DBContext;//server一樣要注入一次DB
        }
        #region 登入

        public async Task<bool> LoginCheck(UserModel logindata)
        {
            //根據帳號去查會員資料
            if (await GetMemberByAccount(logindata.Account) != null)//如果有資料
            {
                //進行帳號密碼確認
                if (PasswordCheck(LoginMember, Password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region 註冊會員
        public async Task<bool> Register(UserModel newUser) //回傳值 <bool>
        {
            //判斷此帳號使否已被註冊
            if (await AccountCheck(newUser.Account))
            {
                try//確保程式不會因執行錯誤而整個中斷
                {
                    // user.Id = _users.Count() == 0 ? 1 : _users.Max(c => c.Id) + 1; 產生M_Id
                    // newUser.M_Id=_DBContext.User.Count()==0?1:_DBContext.User.Max(c=>c.M_Id)+1; 目前是字串不能用max 還有+1
                    newUser.M_Id = GetGUID();
                    newUser.Password = HashPassword(newUser.Password);//密碼 hash
                    newUser.CreateTime = DateTime.Now;//產生建立時間
                    //sql語法
                    //string sql = $@" INSERT INTO User VALUES ('{newUser.M_Id}','{newUser.Account}','{newUser.Password}','{newUser.Name}','{newUser.Email}','{newUser.Role}','{newUser.CreateTime}' )";
                    await this._DBContext.User.AddAsync(newUser);
                    await this._DBContext.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateException e)
                {
                    throw new DbUpdateException(e.Message.ToString());
                }
            }
            else
            {
                return false;
            }

        }
        #endregion 

        #region 取得所有會員資料
        public async Task<List<UserModel>> GetMember()//回傳多個List(用UserModel型態)
        {
            try
            {
                return await this._DBContext.User.ToListAsync(); //查全部
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException(e.Message.ToString());
            }
        }
        #endregion

        #region 藉由帳號取得單筆會員資料
        public async Task<UserModel> GetMemberByAccount(string Account)
        {
            try
            {
                return await this._DBContext.User
                                 .Where(b => b.Account == Account)
                                 .FirstOrDefaultAsync(); //會補空值     
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException(e.Message.ToString());
            }

        }
        #endregion

        #region 修改會員 (?)
        public async Task PutMember(UserModel UpData)
        {
            try
            {
                // UserModel editdata = await this._DBContext.User.Where(p => p.Account == UpData.Account).FirstAsync();
                // this._DBContext.Update(editdata);
                this._DBContext.Update(UpData);
                await this._DBContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException(e.Message.ToString());
            }
        }
        #endregion

        #region 刪除會員
        public async Task DeleteMember(string Account) //不回傳值
        {
            try
            {
                //sql語法
                //string sql = $@"Delete from User WHERE Account='{Account}'";
                var User = this._DBContext.User
                               .Where(b => b.Account == Account);//篩選
                                                                 // .Single(b => b.Account == Account) 載入單一實體                    
                this._DBContext.Remove(User);
                await this._DBContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException(e.Message.ToString());
            }
        }
        #endregion

        #region Hash密碼
        //Hash密碼用的方法
        public string HashPassword(string Password)
        {   //宣告Hash時所添加的無意義亂數值    
            string saltkey = "1q2w3e4r5t6y7u8ui9o0po7tyy";
            //將剛剛宣告的字串與密碼結合    
            string saltAndPassword = String.Concat(Password, saltkey);
            //定義SHA256的HASH物件    
            SHA256CryptoServiceProvider sha256Hasher = new SHA256CryptoServiceProvider();
            //取得密碼轉換成byte資料    
            byte[] PasswordData = Encoding.Default.GetBytes(saltAndPassword);
            //取得Hash後byte資料    
            byte[] HashDate = sha256Hasher.ComputeHash(PasswordData);
            //將Hash後byte資料轉換成string    
            string Hashresult = Convert.ToBase64String(HashDate);
            //回傳Hash後結果    
            return Hashresult;
        }
        #endregion

        #region 產生GUID
        //產生GUID
        public string GetGUID()
        {
            Guid Id = Guid.NewGuid();
            return Id.ToString();
        }
        #endregion

        #region 確認帳號是否已被使用
        public async Task<bool> AccountCheck(string Account)
        {
            UserModel Data = await GetMemberByAccount(Account);
            return (Data == null);
        }
        #endregion

        #region 確認密碼是否正確
        public bool PasswordCheck(Member CheckMember, string Password)
        {
            bool result = CheckMember.Password.Equals(HashPassword(Password));
            return result;

        }
        #endregion
    }
}
#region 筆記
/* WebApi筆記
    DI注入DB伺服器
    不用conn.Open、conn.Close來開關伺服器，DI注入DB已經有這個功能了。
    catch錯誤訊息Exception改成DbUpdateException
    Async 意思是 等這段執行完才會執行下段 建議有SQL的地方都要用

    資料庫要用ORM的寫法 (和LINEQ)
    新增 await this._DBContext.User.AddAsync(newUser);
    修改 await this._DBContext.User.Update(newUser);
    刪除 await this._DBContext.User.Remove(newUser);
    查詢 await this._DBContext.User.ToListAsync();
    查詢單筆 
    await this._DBContext.User.Where(b => b.Account == Account)
    .ToListAsync();  (印出List資料)     .FirstAsync (選取第一筆資料)
*/
/* 參考程式碼
    1.非ORM寫法(註冊)
    public void Register(UserViewModel newMember)
    {
        string sql = $@" INSERT INTO Members (Email,Passsword, Name,Role) VALUES ('{newMember.Email}','{newMember.Passsword}','{newMember.Name}','0') ";
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
    2.帶值傳入MODEL
    public async Task AddUser(string url)
    {
        var User = new UserModel { Name = url };
        await _DBContext.User.AddAsync(User);
        await _DBContext.SaveChangesAsync();       
    }       
*/
#endregion