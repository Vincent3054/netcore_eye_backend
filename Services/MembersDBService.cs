using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using project.Resources.Request;
using project.Resources.Response;

namespace project.Services
{
    public class MembersDBService
    {
        private readonly MyContext _DBContext; //全域變數
        private readonly IMapper _mapper;//AutoMap

        public MembersDBService(IMapper mapper, MyContext DBContext)
        {
            this._mapper = mapper;
            this._DBContext = DBContext;
        }

        #region 註冊會員
        public async Task<bool> RegisterAsync(RegisterResources newRegister) //回傳值 <bool>
        {
            var memberDTO = this._mapper.Map<RegisterResources, MemberModel>(newRegister);//AutoMap<來源,欲修改>(來源)連到Profile檔的設置
            //根據帳號去查會員資料
            // MemberModel Member = await GetMemberByAccountAsync(memberDTO.Account);
            //判斷此帳號使否已被註冊
            if (await GetMemberByAccountAsync(memberDTO.Account) == null)//如果沒有這個號資料
            {
                try
                {
                    memberDTO.M_ID = await GetGUIDAsync();
                    memberDTO.Password = HashPassword(memberDTO.Password);//密碼 hash
                    memberDTO.CreateTime = DateTime.Now;//產生帳號建立時間
                    await this._DBContext.Member.AddAsync(memberDTO);
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

        #region 登入
        public async Task<bool> LoginCheckAsync(LoginResources newLogin)
        {
            var memberDTO = this._mapper.Map<LoginResources, MemberModel>(newLogin);
            //根據帳號去查會員資料
            MemberModel Member = await GetMemberByAccountAsync(memberDTO.Account);
            //判斷是否有此帳號
            if (Member != null)//如果有這個帳號
            {
                //進行密碼確認
                if (Member.Password.Equals(HashPassword(memberDTO.Password)))
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

        #region 藉由帳號取得單筆會員資料
        public async Task<MemberModel> GetMemberByAccountAsync(string Account)
        {
            try
            {
                return await this._DBContext.Member
                                 .Where(b => b.Account == Account)
                                 .FirstOrDefaultAsync(); //只抓第一筆(會補空值)
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException(e.Message.ToString());
            }
        }
        #endregion

        #region 取得所有會員資料
        public async Task<List<MembersAllResources>> GetMemberAsync()//回傳多個List(用MemberModel型態)
        {
            try
            {
                List<MemberModel> Member = await this._DBContext.Member.ToListAsync();//查全部
                var memberDTO = this._mapper.Map<List<MemberModel>, List<MembersAllResources>>(Member);
                return memberDTO; 
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException(e.Message.ToString());
            }
        }
        #endregion

        #region 修改會員
        public async Task<bool> EditMemberAsync(EditResources newEdit, string Account)
        {
            try
            {
                var oriUser = this._DBContext.Member.Single(x => x.Account == Account);
                _DBContext.Entry(oriUser).CurrentValues.SetValues(newEdit);
                // MemberModel editdata = await this._DBContext.Member.Where(p => p.Account == Account).FirstAsync();
                // this._DBContext.Update(editdata);
                //this._DBContext.Update(newEdit);
                await this._DBContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException(e.Message.ToString());
            }
        }
        #endregion

        #region 刪除會員
        public async Task<bool> DeleteMemberAsync(string Account)
        {
            if (await GetMemberByAccountAsync(Account) != null)//如果有這個帳號
            {
                try
                {
                    MemberModel Member = await this._DBContext.Member
                                .Where(b => b.Account == Account)
                                .FirstOrDefaultAsync();
                    this._DBContext.Remove(Member);
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

        #region Hash密碼
        //Hash密碼用的方法 (另一種加密MD5)
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
        public async Task<string> GetGUIDAsync()
        {
            try
            {
                MemberModel Member = new MemberModel();
                String checkGuid;
                do// 產生沒有重複使用的GUID
                {
                    Guid Id = Guid.NewGuid();
                    checkGuid = Id.ToString();
                    Member = await this._DBContext.Member
                                 .Where(b => b.M_ID == checkGuid)
                                 .FirstOrDefaultAsync();
                } while (Member != null);
                return checkGuid;
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException(e.Message.ToString());
            }
        }
        #endregion

        #region 忘記密碼
        public async Task<bool> ForgetPasswordCheckAsync(string Account, string AuthCode)
        {
            MemberModel Member = await GetMemberByAccountAsync(Account);//判斷是否有此帳號
            if (Member != null)//如果有這個帳號
            {
                try
                {
                    var oriUser = this._DBContext.Member.Single(x => x.Account == Account);
                    oriUser.AuthCode = AuthCode;//把驗證碼傳入這個會員的資料裡
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
        #region 寄信驗證
        public async Task<string> EmailValidate(string Account, string AuthCode)
        {
            string ValidateStr = string.Empty; //宣告空字串    
            MemberModel ValidateMember = await GetMemberByAccountAsync(Account);//根據帳號去查會員資料
            if (ValidateMember != null)//如果有這個帳號
            {
                if (ValidateMember.AuthCode == AuthCode)//判斷傳入驗證碼與資料庫中是否相同
                {
                    ValidateStr = "";
                }
                else
                {
                    ValidateStr = "驗證碼錯誤，請重新確認或再註冊";
                }
            }
            else
            {
                ValidateStr = "傳送資料錯誤，請重新確認或再註冊";
            }
            //回傳驗證訊息
            return ValidateStr;
        }
        #endregion
        #region 重設密碼
        public async Task<string> ResetPassword(string Account, string AuthCode, string NewPassword, string NewPasswordCheck)
        {
            string DateStr = string.Empty;//宣告空字串 

            if (NewPassword == NewPasswordCheck)//確認密碼相同
            {
                NewPassword = HashPassword(NewPassword);//hash新密碼
                MemberModel Data = await GetMemberByAccountAsync(Account);//根據帳號去查會員資料
                if (AuthCode == Data.AuthCode)//確認驗證碼
                {
                    try
                    {
                        var oriUser = this._DBContext.Member.Single(x => x.Account == Account);
                        oriUser.AuthCode = string.Empty;//把驗證碼欄位改為空字串(代表這個帳號沒有通過驗證)
                        oriUser.Password = NewPassword;//修改新密碼
                        await this._DBContext.SaveChangesAsync();
                        DateStr = "";
                    }
                    catch (DbUpdateException e)
                    {
                        throw new DbUpdateException(e.Message.ToString());
                    }
                }
                else
                {
                    DateStr = "驗證碼錯誤，請重新確認";
                }
            }
            else
            {
                DateStr = "密碼確認錯誤，請重新確認";
            }
            return DateStr;


        }
        #endregion
        // #region 確認帳號是否已被使用
        //  public async Task<bool> AccountCheck(string Account)
        //  {
        //      MemberModel Data = await GetMemberByAccountAsync(Account);
        //      return (Data == null);
        // }
        // #endregion

        // #region 確認密碼是否正確
        // public bool PasswordCheck(MemberModel CheckMember, string Password)
        // {
        //     bool result = CheckMember.Equals(HashPassword(Password));
        //     return result;
        // }
        // #endregion
    }
}
#region 筆記
/* WebApi筆記
    DI注入DB伺服器
    不用conn.Open、conn.Close來開關伺服器，DI注入DB已經有這個功能了。
    catch錯誤訊息Exception改成DbUpdateException
    Async 意思是 等這段執行完才會執行下段 建議有SQL的地方都要用

    資料庫要用ORM的寫法 (和LINEQ)
    新增 await this._DBContext.Member.AddAsync(newMember);
    修改 await this._DBContext.Member.Update(newMember);
    刪除 await this._DBContext.Member.Remove(newMember);
    查詢 await this._DBContext.Member.ToListAsync();

    查詢單筆 (箭頭寫法)
    await this._DBContext.Member.Where(b => b.Account == Account)
    .ToList();  (印出List資料)     .First (選取第一筆資料)

    //ORM 篩選
        Single    只會抓一筆 (抓超過或抓不到就會噴錯)
        find      (跟where差不多)
        where     會抓到很多筆(可以結合用.First 來選第一筆資料)
*/
/* 參考程式碼
    1.非ORM寫法(註冊)
    public void Register(MemberViewModel newMember)
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
    public async Task AddMember(string url)
    {
        var Member = new MemberModel { Name = url };
        await _DBContext.Member.AddAsync(Member);
        await _DBContext.SaveChangesAsync();       
    }
    3.產生ID
        user.Id = _users.Count() == 0 ? 1 : _users.Max(c => c.Id) + 1; 產生M_Id
        newUser.M_Id=_DBContext.User.Count()==0?1:_DBContext.User.Max(c=>c.M_Id)+1; 目前是字串不能用max 還有+1
        12313
*/
#endregion