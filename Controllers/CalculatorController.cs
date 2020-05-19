using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Calculater.Controllers
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

    public static class OperationKind2
    {
        public const string Add = "+";
    }

    public class PressBody
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OperationKind Operation { get; set; }
        public string Number { get; set; }

    }

    public class Result
    {
        public string Display { get; set; }
    }

    [ApiController]
    [Route("/calculator")]
    public class CalculatorController : Controller
    {

        class State
        {
            public string Num1 { get; set; } = "";
            public string Num2 { get; set; } = "";
            public OperationKind? Operation { get; set; }
            public Result result { get; set;}
        }

        [HttpPost("press")]
        public ActionResult<Result> PostPress([FromBody] PressBody data)
        {
            //// Save
            //var bytes = JsonSerializer.SerializeToUtf8Bytes(state);
            //HttpContext.Session.Set("calculation", bytes);

            // Load
            var state = Load();
            //this.HttpContext.Session.TryGetValue("calculation", byte)


            switch (data.Operation)
            {
                case OperationKind.Add:
                case OperationKind.Subtract:
                case OperationKind.Multiply:
                case OperationKind.Divide:
                    {
                        state.Operation = data.Operation;
                        state.result = new Result { Display = data.Operation.ToString() };
                        break;
                    }
                case OperationKind.Cancel:
                    {
                        state.Num1 = "";
                        state.Num2 = "";
                        state.Operation = null;
                        state.result = new Result { Display = "" };
                        break;
                    }
                case OperationKind.Number:
                    {
                        if (state.Operation == null)
                        {
                            state.result = new Result { Display = (state.Num1 += data.Number.ToString()) };
                        }
                        else
                            state.result = new Result { Display = (state.Num2 += data.Number.ToString()) };
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

                            state.result = result;
                        }
                        break;
                    }

                default:
                    throw new NotImplementedException($"Opereation {data.Operation} not implemented yet.");
            }

            Save(state);
            return state.result;
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
