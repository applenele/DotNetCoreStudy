using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomPipeline
{
    /// <summary>
    /// 本例子是用来模拟asp.net core 请求管道
    /// 通过这个例子对委托有了新的认识，     
    /// </summary>
    class Program
    {
        public static List<Func<RequestDelegate, RequestDelegate>> _list =
            new List<Func<RequestDelegate, RequestDelegate>>();


        static void Main(string[] args)
        {

            // 中间件1
            Use(next =>
            {

                return context =>
                {
                    Console.WriteLine("1");
                    return next.Invoke(context);
                };
            });

            // 中间件2
            Use(next =>
            {
                return context =>
                {
                    Console.WriteLine("2");
                    return next.Invoke(context);
                };
            });

            // 最后一个requestDelegate
            RequestDelegate end = (context) =>
                {

                    Console.WriteLine("end.....");
                    return Task.CompletedTask;
                };

            // 将集合顺序反转
            _list.Reverse();

            foreach (var middleware in _list)
            {

                end = middleware.Invoke(end); //此处的Invoke是返回一个RequestDeledate对象,并没有去执行这个RequestDelegate
            }
            end.Invoke(new RequestContext());

            // 1.通过构造遍历先把最后一个RequestDelegate传到中间件2中，返回一个RequestDelegate，里面先输出“2”，在调用next(end)的Invoke，此时返回赋值给end
            // 2.在end传到中间件1中，返回一个RequestDelegate，里面先输出“1”，在调用next(end)的Invoke，此时返回赋值给end
            // 3.end.Invoke() 
            // 实际上的执行顺序 中间件1的内容 (next)->中间件2的内容 (next)-> end





            Console.ReadLine();
        }


        public static void Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            _list.Add(middleware);
        }
    }
}
