using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Calculator.Controllers
{
    public enum OperationKind
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Cancel,
        Number,
        Equals
    }

    public class PressBody
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OperationKind Operation { get; set; }
        public string Number { get; set; }

    }

    public class Result
    {
        public string Display { get; set; } = "0";
    }

    public class State
    {
        public string Num1 { get; set; } = "";
        public string Num2 { get; set; } = "";
        public OperationKind? Operation { get; set; }
        public Result Result { get; set;} = new Result();
    }

    [ApiController]
    [Route("/calculator")]
    public class CalculatorController : Controller
    {
        [HttpGet("")]
        public ActionResult<Result> LoadState()
        {
            var state = Load();
            return state.Result;
        }

        [HttpPost("press")]
        public ActionResult<Result> PostPress([FromBody] PressBody data)
        {
            // Load existing session or set a new one
            var state = Load();

            //Find the operation to perform
            switch (data.Operation)
            {
                case OperationKind.Subtract:
                    if (state.Num1 == "")
                    {
                        state.Result = new Result { Display = (state.Num1 += '-') };
                        
                    }
                    else if (state.Operation != null && state.Num2 == "")
                    {
                        state.Result = new Result { Display = (state.Num2 += '-') };                        
                    }
                    else
                        PushOperation(data, state);
                    break;
                case OperationKind.Add:
                case OperationKind.Multiply:
                case OperationKind.Divide:
                    {
                        PushOperation(data, state);
                        break;
                    }
                case OperationKind.Cancel:
                    {
                        state.Num1 = "";
                        state.Num2 = "";
                        state.Operation = null;
                        state.Result = new Result { Display = "0" };
                        break;
                    }
                case OperationKind.Number:
                    {
                        if (state.Operation == null)
                        {
                            state.Result = new Result { Display = state.Num1 += data.Number.ToString() };
                        }
                        else
                            state.Result = new Result { Display = state.Num2 += data.Number.ToString() };
                        break;
                    }
                case OperationKind.Equals:
                    {
                        if (state.Num2 != "")
                        {
                            var result = new Result
                            {
                                Display = state.Operation.Value switch
                                {
                                    OperationKind.Add => (double.Parse(state.Num1) + double.Parse(state.Num2)).ToString(),
                                    OperationKind.Divide => (double.Parse(state.Num1) / double.Parse(state.Num2)).ToString(),
                                    OperationKind.Subtract => (double.Parse(state.Num1) - double.Parse(state.Num2)).ToString(),
                                    OperationKind.Multiply => (double.Parse(state.Num1) * double.Parse(state.Num2)).ToString(),
                                    _ => throw new System.Exception("invalid operation")
                                }
                            };

                            state.Num1 = result.Display;
                            state.Num2 = "";
                            state.Operation = null;

                            state.Result = result;
                        }
                        break;
                    }

                default:
                    throw new NotImplementedException($"Operation {data.Operation} not implemented yet.");
            }

            //Save the state to the session
            Save(state);
            return state.Result;
        }

        private static void PushOperation(PressBody data, State state)
        {
            state.Operation = data.Operation;
            state.Result = new Result { Display = data.Operation.ToString() };
        }

        private void Save(State state)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(state);
            HttpContext.Session.Set("calculation", bytes);
        }

        private State Load()
        {
            State state;
            if (HttpContext.Session.TryGetValue("calculation", out var loadedBytes))
                state = JsonSerializer.Deserialize<State>(loadedBytes);
            else
            {
                state = new State();
            }

            return state;
        }
    }
}
