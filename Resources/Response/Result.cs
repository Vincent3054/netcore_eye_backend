using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Resources.Response
{
    public class Result
    {
        public bool success { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        
        public Result(bool success, int status, string message)
        {
            this.success = success;
            this.status = status;
            this.message = message;
        }
    }

    public class Result<T>
    //T可以包含其他ViewModel 型態
    {
        public bool success { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        public T errors { get; set; }
        public T data { get; set; }

        public Result(bool success, int status, string message, T errors, T data)
        {
            this.success = success;
            this.status = status;
            this.message = message;
            this.errors = errors;
            this.data = data;
        }
    }
    public class ResultList<T>
    //T可以包含其他ViewModel 型態
    {
        public bool success { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        public List<T> errors { get; set; }
        public List<T> data { get; set; }
        public ResultList(bool success, int status, string message, List<T> errors, List<T> data)
        {
            this.success = success;
            this.status = status;
            this.message = message;
            this.errors = errors;
            this.data = data;
        }
    }
}

