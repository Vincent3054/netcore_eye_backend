using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyWebsite;
using project.Resources;

namespace project.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //ur;名子
    public class MembersController : ControllerBase
    {
        //連線字串 c=>s 才可以做連線
        private readonly MyContext _myContext;
        private readonly IMapper _mapper;
        private readonly MembersDBService _MembersDBService;

        public MembersController(IMapper mapper,MyContext Context) //建構子
        {
            this._mapper = mapper;
            this._myContext =  Context; //sercer也要用一個
            this._MembersDBService=new MembersDBService(_myContext);
            //orm ??!!
            //建構子 裡面的東東 new會寫在這裡 (db)
            //map
        }
        //models 和 resources view models 做轉換 map是放進去
        //註冊帳號密碼 altmap 把藥用的東西放在viewmodel 
        //map 前置作業model 做連結才可在s用  箝制作業會寫在resources 開一個資料夾 網路查
        [HttpPost] //api
        //回傳狀態瑪顯示
        //orm寫法 s要用
        //c只做接收view傳來的東西 呼叫s去做運算 (c只做接收view的需求 和回傳view要得東西)
        //加密都在s處理
        public IActionResult Members(UserViewModel RegisterData){
    		// var user = this._svc.Get();
            // UserViewModel userViewModel =_mapper.Map<UserViewModel>(user);
            
        }	     

       // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<UserModel>> PostMembersController(UserModel RegisterData)
        {
            _myContext.UserModel.Add(RegisterData);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            // return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        public async Task<ActionResult> GetUserAsync(UserResources user)
        {
            // UserModel user = new UserModel();
            var userDTO =  this._mapper.Map<UserModel>(user);
            bool a= await  this._MembersDBService.PostMember(userDTO);
            if(a)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
            // return BadRequest();400 notfu
            // var name = userDTO.Name; 
            // var email = userDTO.Email; 
            // return user;
        }    
        // private static List<UserModel> _users = new List<UserModel>();

         [HttpPost]
        // public async Task<UserModel> PostAsync([FromBody]UserModel user)
        // {
        //     var result = new UserModel();
        //     // user.Id = _users.Count() == 0 ? 1 : _users.Max(c => c.Id) + 1;
        //     _users.Add(user);
        //     result.Name = user.Name;
        //     result.Email = user.Email;
        //     result.Passsword = user.Passsword;
        //     await MembersDBService.PostMember();
        //     return result;
        // }
        public async Task<ActionResult> PostAsync([FromBody]UserModel user)
        {
            var result = new UserModel();
            // user.Id = _users.Count() == 0 ? 1 : _users.Max(c => c.Id) + 1;
            _users.Add(user);
            result.Name = user.Name;
            result.Email = user.Email;
            result.Passsword = user.Passsword;
            await MembersDBService.PostMember();
            return result;
        }

    }
}
