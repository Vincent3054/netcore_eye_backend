using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DBContext;
using Microsoft.AspNetCore.Authorization;
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


        public MembersController(IMapper mapper, MyContext DBContext, JwtHelpers jwt) //建構子
        {
            this._mapper = mapper;
            this._DBContext = DBContext;
            this._jwt = jwt;
            //Service建議用DI注入的方式 但因為本系統架構不大所以先用new的方式 註2
            this._MembersDBService = new MembersDBService(_mapper, _DBContext);
        }
        #endregion

        #region 註冊
        // POST: api/Members/Register
        [HttpPost("Register")] //http協定 
        public async Task<ActionResult> Register([FromBody] RegisterResources RegisterData) //同步異步寫法 註3，Webapi裡面的ViewModel是Resources 註4
        {
            //controller越乾淨越好，把AutoMap移到Service
            if (await this._MembersDBService.RegisterAsync(RegisterData))//呼叫function到Service並把原始資料傳過去
            {
                return Ok("註冊成功"); //200
            }
            else
            {
                return BadRequest("帳號已被註冊"); //400
            }
        }
        #endregion

        #region 登入
        // POST: api/Members/Login
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginResources LoginData) //同步異步寫法 註3 ，Webapi裡面的ViewModel是Resources 註4
        {
            if (await this._MembersDBService.LoginCheckAsync(LoginData)) //*沒有查到帳號會出現問題
            {
                return Ok(this._jwt.GenerateToken(LoginData.Account));
            }
            else
            {
                return BadRequest("登入失敗");
            }
        }
        #endregion

        #region 顯示會員資料列
        // GET: api/Members/All
        [HttpGet("All")]
        public async Task<ActionResult> GetMembers()
        {
            try
            {
                return Ok(await this._MembersDBService.GetMemberAsync());
            }
            catch
            {
                return NotFound("查詢失敗"); //400
            }
        }
        #endregion

        #region 顯示單筆會員資料
        // GET: api/Members/Single/{Account}
        [HttpGet("Single/{Account}")]
        public async Task<ActionResult> GetSingleMembers(string Account)
        {
            try
            {
                UserModel GetMembersData = await this._MembersDBService.GetMemberByAccountAsync(Account);
                var userDTO = this._mapper.Map<UserModel, MembersAllResources>(GetMembersData);
                return Ok(userDTO);
            }
            catch
            {
                return NotFound("查詢失敗"); //400
            }
        }
        #endregion

        #region 刪除會員
        // Delete: api/Members/Delete/{Account}
        [Authorize]
        [HttpDelete("Delete/{Account}")]
        public async Task<ActionResult> DeleteMember(string Account)
        {
            try
            {
                if (await this._MembersDBService.DeleteMemberAsync(Account))
                {
                    return Ok("刪除成功");
                }
                else
                {
                    return BadRequest("沒有此帳號");
                }
            }
            catch
            {
                return NotFound("發生錯誤");
            }

        }
        #endregion

        #region 修改會員
        // Delete: api/Members/Edit/{Account}
        [HttpPut("Edit/{Account}")]
        public async Task<ActionResult> EditMember(string Account, EditResources EditData)
        {
            try
            {
                if (await this._MembersDBService.EditMemberAsync(EditData, Account))
                {
                    return Ok("修改成功");
                }
                else
                {
                    return BadRequest("沒有此帳號");
                }
            }
            catch
            {
                return NotFound("發生錯誤");
            }

        }
        #endregion
        /*
        忘記密碼(信箱 帳號)
        去收信(宣告URL 寄信的時候寄出 讓他可以連到修改密碼的API)
        修改密碼
        */
        

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
*/
#endregion