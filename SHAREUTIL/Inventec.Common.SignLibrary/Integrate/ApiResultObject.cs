using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.Integrate
{
    public abstract class Result
    {
        public bool Success { get; set; }
        public CommonParam Param { get; set; }
    }

    public class ResultObject
    {
        public object Data { get; set; }
        public int? Total { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public ResultObject()
        {
        }

        public void SetValue(object resultData, string message, bool success, int? total)
        {
            Data = resultData;
            Message = message;
            Success = success;
            Total = total;
        }
    }

    public class ApiResultObject<T> : Result
    {
        public T Data { get; set; }
        //public bool Success { get; set; }
        //public CommonParam Param { get; set; }
        public ApiResultObject()
        {
        }

        public ApiResultObject(T data)
        {
            Data = data;
        }

        public ApiResultObject(T data, bool success)
        {
            Data = data;
            Success = success;
        }

        public void SetValue(T data, bool success, CommonParam param)
        {
            Data = data;
            Success = success;
            Param = param;
        }

        public ResultObject ConvertToResultObject()
        {
            ResultObject result = new ResultObject();
            try
            {
                result.Data = this.Data;
                result.Success = this.Success;
                result.Total = (Param != null ? Param.Count : 0);
            }
            catch (Exception ex)
            {
                //LogSystem.Error(ex);
                result = new ResultObject();
            }
            return result;
        }
    }
}
