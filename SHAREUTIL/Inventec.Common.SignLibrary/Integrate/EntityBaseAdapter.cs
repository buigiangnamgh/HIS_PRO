using Inventec.Common.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.Integrate
{
    public abstract class EntityBaseAdapter
    {
        protected EntityBaseAdapter()
        {
            try
            {
                ClassName = this.GetType().Name;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        protected string ClassName { get; set; }
        protected string MethodName { get; set; }
        public static string UserName { get; set; }
        protected string ErrorFormat { get; set; }
        protected string Input { get; set; }
        protected string Output { get; set; }
        protected int FrameIndex { get; set; }

        protected void LogInOut()
        {
            try
            {
                //MethodName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
                Logging(new StringBuilder().Append("InputData: ").Append(Input).Append(Environment.NewLine).Append("____OutputData: ").Append(Output).ToString(), LogType.Info);
            }
            catch (Exception ex)
            {
                try
                {
                    LogSystem.Error("EntityBase.LogInOut.Exception.", ex);
                }
                catch (Exception)
                {
                }
            }
        }

        protected void LogInOut(string output)
        {
            FrameIndex += 1;
            LogInOut(output, LogType.Info);
        }

        protected void LogInOut(string output, LogType logType)
        {
            try
            {
                FrameIndex += 1;
                //MethodName = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
                Logging(new StringBuilder().Append("InputData: ").Append(Input).Append(Environment.NewLine).Append("____OutputData: ").Append(output).ToString(), logType);
            }
            catch (Exception ex)
            {
                try
                {
                    LogSystem.Error("EntityBase.LogInOut.Exception.", ex);
                }
                catch (Exception)
                {
                }
            }
        }

        protected void Logging(string message, LogType en)
        {
            try
            {
                FrameIndex += 1;
                try
                {
                    //LogSystem.Info(String.Join("|||", new System.Diagnostics.StackTrace().GetFrames().Select(o => o.GetMethod().Name).ToList()));
                    //if (String.IsNullOrEmpty(MethodName))
                    //{
                    MethodName = string.Format("{0}", new System.Diagnostics.StackTrace().GetFrame(FrameIndex + 2).GetMethod().Name);
                    ClassName = string.Format("{0}", new System.Diagnostics.StackTrace().GetFrame(FrameIndex + 2).GetMethod().ReflectedType.FullName);
                    //}
                }
                catch (Exception ex)
                {
                    LogSystem.Error(ex);
                }
                string threadInfo = GetThreadId();
                message = new StringBuilder().Append(ErrorFormat).Append(String.IsNullOrEmpty(ErrorFormat) ? "" : Environment.NewLine).Append("____").Append(GetInfoProcess()).Append(Environment.NewLine).Append("____").Append("UserName: [").Append(UserName).Append("]").Append(Environment.NewLine).Append("____").Append(threadInfo).Append(Environment.NewLine).Append("____").Append(message).ToString();
                switch (en)
                {
                    case LogType.Debug:
                        LogSystem.Debug(message);
                        break;
                    case LogType.Info:
                        LogSystem.Info(message);
                        break;
                    case LogType.Warn:
                        LogSystem.Warn(message);
                        break;
                    case LogType.Error:
                        LogSystem.Error(message);
                        break;
                    case LogType.Fatal:
                        LogSystem.Fatal(message);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    LogSystem.Error("EntityBase.Logging.Exception.", ex);
                }
                catch (Exception)
                {
                }
            }
        }

        protected string GetInfoProcess()
        {
            try
            {
                FrameIndex += 1;
                return new StringBuilder().Append("TraceInfo: [").Append("Class: ").Append((String.IsNullOrWhiteSpace(GetClassName()) ? "" : GetClassName() + "; ")).Append("MethodName: ").Append((String.IsNullOrWhiteSpace(GetMethodName()) ? "" : GetMethodName() + "; ")).Append("LineNumber: ").Append((String.IsNullOrWhiteSpace(GetLineNumber().ToString()) ? "" : GetLineNumber().ToString())).Append("]").ToString();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return "";
            }
        }

        protected string GetClassName()
        {
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
            return trace.GetFrame(FrameIndex + 2).GetMethod().ReflectedType.FullName;
        }

        protected string GetMethodName()
        {
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
            return trace.GetFrame(FrameIndex + 2).GetMethod().Name;
        }

        protected int GetLineNumber()
        {
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
            return trace.GetFrame(FrameIndex + 2).GetFileLineNumber();
        }

        protected static string GetThreadId()
        {
            try
            {
                return "ThreadId: " + System.Threading.Thread.CurrentThread.ManagedThreadId + "";
            }
            catch (Exception ex)
            {
                LogSystem.Error("EntityBase.GetThreadId.Exception.", ex);
                return "";
            }
        }

        /// <summary>
        /// True neu data != null.
        /// False neu nguoc lai.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool IsNotNull(Object data)
        {
            bool result = false;
            try
            {
                result = (data != null);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// True neu string != NullOrEmpty.
        /// False neu nguoc lai.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool IsNotNullOrEmpty(string data)
        {
            bool result = false;
            try
            {
                result = (!String.IsNullOrEmpty(data));
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Su dung de kiem tra list co du lieu.
        /// True neu listData != null && Count > 0.
        /// False neu nguoc lai.
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        protected bool IsNotNullOrEmpty(ICollection listData)
        {
            bool result = false;
            try
            {
                result = (listData != null && listData.Count > 0);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Su dung de kiem tra cac truong ID trong CSDL.
        /// True neu id > 0.
        /// False neu nguoc lai.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected bool IsGreaterThanZero(long id)
        {
            bool result = false;
            try
            {
                result = (id > 0);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected enum LogType
        {
            Debug,
            Info,
            Warn,
            Error,
            Fatal,
        }
    }
}
