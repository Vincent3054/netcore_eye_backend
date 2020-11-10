using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using DBContext;
using HttpClientFactorySample.GitHub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using project.Resources;
using project.Resources.Request;
using project.Resources.Response;
using project.Services;
using Utils;

namespace project.Controllers //用namespace包起來 project(檔名.現在的資料夾) using的時候方便看到
{
    [ApiController]//web api 必加 (自動啟用回傳400功能、和自動套用[FromBody]等屬性) 註1
    [Route("api/[controller]")] //URL路徑http://localhost:15175/api/Members，[controller]把MembersController取代成Members
    public class MembersController : ControllerBase //繼承自ControllerBase 基底類別 註2
    {
        #region DI注入
        //宣告全域變數
        private readonly MyContext _DBContext;//DB  
        private readonly IMapper _mapper;//AutoMap
        private readonly JwtHelpers _jwt;
        private readonly MembersDBService _MembersDBService;//Service
        private readonly MailService _MailService;
        private readonly IWebHostEnvironment _env; //註6 取得網站根目錄功能

        private readonly IHttpClientFactory _clientFactory; //在 ASP.NET Core 中使用 IHttpClientFactory 發出 HTTP 要求

        public MembersController(IMapper mapper, MyContext DBContext, JwtHelpers jwt, IWebHostEnvironment env, IHttpClientFactory clientFactory) //建構子
        {
            this._mapper = mapper;
            this._DBContext = DBContext;
            this._jwt = jwt;
            this._env = env;
            //Service建議用DI注入的方式 但因為本系統架構不大所以先用new的方式 註2
            this._MembersDBService = new MembersDBService(_mapper, _DBContext);
            this._MailService = new MailService();
            _clientFactory = clientFactory;//在 ASP.NET Core 中使用 IHttpClientFactory 發出 HTTP 要求

        }
        #endregion

        /* call api測試
        public IEnumerable<GitHubBranch> Branches { get; private set; }

        public bool GetBranchesError { get; private set; }
        public async Task OnGet()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://api.github.com/repos/aspnet/docs/branches");
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
               Branches =await JsonSerializer.DeserializeAsync<IEnumerable<GitHubBranch>>(await response.Content.ReadAsStreamAsync());
            }
            else
            {
                GetBranchesError = true;
                Branches = Array.Empty<GitHubBranch>();
            }

        }
        */



        #region 註冊
        /// <summary>
        /// 註冊
        /// </summary>
        // POST: api/Members/Register
        [HttpPost("Register")] //http協定 
        public async Task<ActionResult> Register([FromBody] RegisterResources RegisterData) //同步異步寫法 註3，Webapi裡面的ViewModel是Resources 註4
        {
            //controller越乾淨越好，把AutoMap移到Service
            if (await this._MembersDBService.RegisterAsync(RegisterData))//呼叫function到Service並把原始資料傳過去
            {
                return Ok(new Result(true, 200, "註冊成功"));
            }
            else
            {
                return BadRequest(new Result(false, 400, "帳號已被註冊"));
            }
        }
        #endregion

        #region 登入
        /// <summary>
        /// 登入
        /// </summary>
        // POST: api/Members/Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginResources LoginData) //同步異步寫法 註3 ，Webapi裡面的ViewModel是Resources 註4
        {
            if (await this._MembersDBService.LoginCheckAsync(LoginData)) //*沒有查到帳號會出現問題
            {
                LoginData.Token = this._jwt.GenerateToken(LoginData.Account);
                return Ok(new Result<LoginResources>(true, 200, "登入成功", null, LoginData));
            }
            else
            {
                return BadRequest(new Result(false, 400, "登入失敗"));
            }
        }
        #endregion

        #region 顯示會員資料列
        /// <summary>
        /// 顯示會員資料列
        /// </summary>
        // GET: api/Members/All
        [HttpGet("All")]
        public async Task<ActionResult> GetMembers()
        {
            try
            {
                //await OnGet();//在 ASP.NET Core 中使用 IHttpClientFactory 發出 HTTP 要求
                List<MembersAllResources> Data = await this._MembersDBService.GetMemberAsync();
                return Ok(new ResultList<MembersAllResources>(true, 200, "查詢成功", null, Data));
            }
            catch
            {
                return NotFound(new Result(false, 404, "查詢失敗")); //400
            }
        }
        #endregion

        #region 顯示單筆會員資料
        /// <summary>
        /// 顯示單筆會員資料
        /// </summary>
        // GET: api/Members/Single/{Account}
        [HttpGet("Single/{Account}")]
        public async Task<ActionResult> GetSingleMembers(string Account)
        {
            try
            {
                MemberModel GetMembersData = await this._MembersDBService.GetMemberByAccountAsync(Account);
                var memberDTO = this._mapper.Map<MemberModel, MembersAllResources>(GetMembersData);
                return Ok(new Result<MembersAllResources>(true, 200, "查詢成功", null, memberDTO));
            }
            catch
            {
                return NotFound(new Result(false, 404, "查詢失敗"));
            }
        }
        #endregion
        #region 刪除會員
        /// <summary>
        /// 刪除會員
        /// </summary>
        // Delete: api/Members/Delete/{Account}
        [HttpDelete("Delete/{Account}")]
        public async Task<ActionResult> DeleteMember(string Account)
        {
            try
            {
                if (await this._MembersDBService.DeleteMemberAsync(Account))
                {
                    return Ok(new Result(true, 200, "刪除成功"));
                }
                else
                {
                    return BadRequest(new Result(false, 400, "帳號不存在"));
                }
            }
            catch
            {
                return NotFound(new Result(false, 404, "發生錯誤"));
            }

        }
        #endregion

        #region 修改會員
        /// <summary>
        /// 修改會員
        /// </summary>
        // Delete: api/Members/Edit/{Account}
        [HttpPut("Edit/{Account}")]

        public async Task<ActionResult> EditMember(string Account, EditResources EditData)
        {
            try
            {
                if (await this._MembersDBService.EditMemberAsync(EditData, Account))
                {
                    return Ok(new Result(true, 200, "修改成功"));
                }
                else
                {
                    return BadRequest(new Result(false, 400, "帳號不存在"));
                }
            }
            catch
            {
                return NotFound(new Result(false, 404, "發生錯誤"));
            }

        }
        #endregion

        #region 重設密碼系列
        /*  說明
            1.忘記密碼 ( 把驗證碼更新進去會員資料  在去寄信)
            2.接收驗證信 (去信箱收信 點擊信內容的URL POST從網址帶來的值 帳號、驗證碼 去做驗證)
            3.重設密碼 ( 驗證成功後把驗證碼的值清空 在重設密碼)
        */

        // 忘記密碼
        /// <summary>
        /// 忘記密碼
        /// </summary>
        [HttpPut("ResetPassword1")]
        // Delete: api/Members/ResetPassword1
        public async Task<ActionResult> FrogetPassword(FrogetPasswordResources FPDate)
        {
            try
            {
                //產生驗證碼
                string AuthCode = this._MailService.GetValidateCode();
                //判斷有無此帳號，並把驗證碼傳入到會員個人資料裡
                if (await this._MembersDBService.ForgetPasswordCheckAsync(FPDate.Account, AuthCode))
                {
                    //設定寄信內容範本路徑
                    string EmailUrl = "/Email/ForgetPasswordEmail.html";
                    string TempMail = System.IO.File.ReadAllText(this._env.WebRootPath + EmailUrl);//取得wwwwoor根目錄 註6
                    //宣告Email驗證用的Url
                    string P = "http://localhost:8000/?#/ResetPassword?";
                    string account = FPDate.Account;
                    string authcode = AuthCode;
                    string Path = P + "Account=" + account + "&AuthCode=" + authcode;
                    //將驗證信內容的變數填入範本中
                    string MailBody = this._MailService.GetRegisterMailBody(TempMail, FPDate.Account, Path, AuthCode);
                    //寄送驗證信
                    this._MailService.SendRegisterMail(MailBody, FPDate.Email, false);
                    return Ok(new Result(true, 200, "請去收驗證信，並重設密碼"));
                }
                else
                {
                    FPDate.Account = null;
                    FPDate.Email = null;
                    return BadRequest(new Result(false, 400, "此帳號尚未註冊"));
                }
            }
            catch
            {
                return NotFound(new Result(false, 404, "發生錯誤"));
            }
        }

        // 接收驗證信
        /// <summary>
        /// 接收驗證信
        /// </summary>
        [HttpPut("ResetPassword2")]
        // Delete: api/Members/ResetPassword2
        public async Task<ActionResult> EmailVaildate(EmailVaildateResources EVData)
        {
            try
            {
                string ValidateStr = await this._MembersDBService.EmailValidate(EVData.Account, EVData.AuthCode);
                if (String.IsNullOrWhiteSpace(ValidateStr))
                {
                    return Ok(new Result(true, 200, "驗證成功"));
                }
                else
                {
                    return BadRequest(new Result(false, 400, ValidateStr));
                }
            }
            catch
            {
                return NotFound(new Result(false, 404, "驗證失敗"));
            }
        }


        // 重設密碼
        /// <summary>
        /// 重設密碼
        /// </summary>
        [HttpPut("ResetPassword3")]
        // Delete: api/Members/ResetPassword3
        public async Task<ActionResult> ResetPassword(ResetPasswordResources RPData)
        {
            try
            {
                string DateStr = await this._MembersDBService.ResetPassword(RPData.Account, RPData.AuthCode, RPData.NewPassword, RPData.NewPasswordCheck);
                if (String.IsNullOrWhiteSpace(DateStr))
                {
                    return Ok(new Result(true, 200, "重設密碼成功"));
                }
                else
                {
                    return BadRequest(new Result(false, 400, DateStr));
                }
            }
            catch
            {
                return NotFound(new Result(false, 404, "發生錯誤"));
            }
        }

        #endregion
    }

}
#region 筆記

/*
iActionResult
ActionResult
<ActionResult> <型態>
*/

/* AutoMapper筆記
        可以讓view model 和 model 去做轉換
        以api為例 拿到的東西是viewModel但是要存進model
        這時就可以用map去做轉換
        
        前置作業要 先di注入後 在箝制作業會寫在resources建一個Profile檔並做設定 
*/
/* WebApi筆記
        繼承ControllerBase
        加上[ApiController]
        Controllers負責接受VIEW傳來的東西(或回傳view要得東西) 並呼叫Services去做運算
        Services sql的部分藥用orm的寫法
        加密那些都寫在Services

        DI注入 DB、AutoMap、Service
        [FromBody] post
        [FromQuery] put
*/
/*參考程式碼
    public Task<ActionResult> PostAsync(UserModel user)
    {
        var result = new UserModel();
            result.Name = user.Name;
            result.Email = user.Email;
            result.Passsword = user.Passsword;    
    }    
*/
/*
    註1 https://ithelp.ithome.com.tw/articles/10208987
    註2 https://ithelp.ithome.com.tw/articles/10193172
    註3 同步、非同步寫法
           同步寫法
               public IActionResult Members(UserViewModel RegisterData){}
           非同步寫法 (async、Task<>、await...)
               public async Task<IActionResult> MembersAsync(UserViewModel RegisterData){await...} 
    註4 ViewModel 一個頁面就會有一個viewModel 裡面放那個頁面需要用到的欄位
    註5 https://ithelp.ithome.com.tw/articles/10157130
    註6 https://blog.johnwu.cc/article/ironman-day16-asp-net-core-multiple-environments.html
*/
#endregion