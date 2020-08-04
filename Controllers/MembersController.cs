using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using project.Resources;
using project.Resources.Request;
using project.Resources.Response;
using project.Services;
namespace project.Controllers //用namespace包起來 project(檔名.現在的資料夾) using的時候方便看到
{
    [ApiController]//web api 必加 (自動啟用回傳400功能、和自動套用[FromBody]等屬性) 註1
    [Route("api/[controller]")] //URL路徑http://localhost:15175/api/Members，[controller]把MembersController取代成Members
    public class MembersController : ControllerBase //繼承自ControllerBase 基底類別 註2
    {
        //宣告全域變數
        private readonly MyContext _DBContext;//DB  
        private readonly IMapper _mapper;//AutoMap
        private readonly MembersDBService _MembersDBService;//Service

        public MembersController(IMapper mapper, MyContext DBContext) //建構子
        {
            this._mapper = mapper;
            this._DBContext = DBContext;
            //Service建議用DI注入的方式 但因為本系統架構不大所以先用new的方式 註2
            this._MembersDBService = new MembersDBService(_mapper,_DBContext);
        }

        #region 註冊
        // POST: api/Members/Register
        [HttpPost] //http協定 
        [Route("Register")] //http協定 
        public async Task<ActionResult> Register(RegisterResources RegisterData) //同步異步寫法 註3 ，Webapi裡面的ViewModel是Resources 註4
        {
            // var userDTO = this._mapper.Map<RegisterResources, UserModel>(RegisterData);//AutoMap<欲修改>(來源) 連到Profile檔的設置 註5
            // 
            // bool status = await this._MembersDBService.Register(userDTO);
            if (await this._MembersDBService.RegisterAsync(RegisterData))//呼叫function到Service並把原始資料傳過去
            {
                return Ok("註冊成功"); //200
            }
            else
            {
                return BadRequest("註冊失敗"); //400
            }
        }
        #endregion

        #region 登入
        // POST: api/Members/Login
        [HttpPost]
        [Route("Login")] //http協定 
        public async Task<ActionResult> Login(LoginResources LoginData) //同步異步寫法 註3 ，Webapi裡面的ViewModel是Resources 註4
        {
            var userDTO = this._mapper.Map<LoginResources, UserModel>(LoginData);//AutoMap<欲修改>(來源) 連到Profile檔的設置 註5
            if (await this._MembersDBService.LoginCheckAsync(userDTO))
            {
                return Ok("登入成功");
            }
            else
            {
                return BadRequest("登入失敗"); //400
            }
        }
        #endregion

        #region 顯示會員資料列
        [HttpGet]
        [Route("All")]
        public async Task<ActionResult> GetMembersAsync()
        {
            try
            {
                List<UserModel> GetMembersData = await this._MembersDBService.GetMember();
                var userDTO = this._mapper.Map<List<UserModel>, List<MembersAllResources>>(GetMembersData);
                return Ok(userDTO);
            }
            catch
            {
                return BadRequest("查詢失敗"); //400
            }
        }
        #endregion

        #region 刪除會員
        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult> DeleteMemberAsync(string Account)
        {
            if (await this._MembersDBService.DeleteMember(Account))
            {
                return Ok("刪除成功");
            }
            else
            {
                return BadRequest("發生錯誤");
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
           非同步寫法 (async、Task<>、awit...)
               public async Task<IActionResult> MembersAsync(UserViewModel RegisterData){awit...} 
    註4 ViewModel 一個頁面就會有一個viewModel 裡面放那個頁面需要用到的欄位
    註5 https://ithelp.ithome.com.tw/articles/10157130     
*/
#endregion