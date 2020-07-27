using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace project.Controllers
{
    [ApiController]
    [Route("[controller]")] //ur;名子
    public class MembersController : ControllerBase
    {
        //連線字串 c=>s 才可以做連線
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MembersController> _logger;

        public MembersController(ILogger<MembersController> logger) //建構子
        {
            _logger = logger;
            //orm ??!!
            //建構子 裡面的東東 new會寫在這裡 (db)
            //map
        }
        //models 和 resources view models 做轉換 map是放進去
        //註冊帳號密碼 altmap 把藥用的東西放在viewmodel 
        //map 前置作業model 做連結才可在s用  箝制作業會寫在resources 開一個資料夾 網路查
        [HttpGet] //api
        //回傳狀態瑪顯示
        //orm寫法 s要用
        //c只做接收view傳來的東西 呼叫s去做運算 (c只做接收view的需求 和回傳view要得東西)
        //加密都在s處理
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //     public UserRequestModel Create(UserRequestModel user, string password)
        //    {
        //        // validation
        //        if (string.IsNullOrWhiteSpace(password))
        //            throw new AppException("Password is required");

        //        if (_iUserRepository.Any(x => x.Email == user.Email))
        //            throw new AppException("Email \"" + user.Email + "\" is already taken");

        //        if (_iUserRepository.Any(x => x.UserName == user.UserName))
        //            throw new AppException("Username \"" + user.UserName + "\" is already taken");

        //        CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

        //        user.PasswordHash = passwordHash;
        //        user.PasswordSalt = passwordSalt;

        //        _iUserRepository.Add(user);

        //        return user;
        //    }


        

    }
}
