using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Reflection;
using GameServer.Servers;

namespace GameServer.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> conttrollerDict = new Dictionary<RequestCode, BaseController>();
        private Server server;

        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }



        void InitController()
        {

            DefaultController defaultController = new DefaultController();
            conttrollerDict.Add(defaultController.RequestCode, defaultController);



        }



        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client, Server server)
        {
            BaseController controller;
            bool isGet = conttrollerDict.TryGetValue(requestCode, out controller);
            if (!isGet)
            {
                Console.WriteLine("无法得到" + requestCode + "对应的Controller,无法处理请求！");
                return;
            }

            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo mi = controller.GetType().GetMethod(methodName);
            if (mi == null)
            {
                Console.WriteLine("[警告] 没有在Controller[" + controller.GetType() + "]中没有对应的处理方法：  [" + methodName + "]");
                return;
            }

            object[] parameters = new object[] { data, client };
            object obj = mi.Invoke(controller, parameters);
            if (obj == null || string.IsNullOrEmpty(obj as string))
            {
                return;
            }
            server.SendResponse(client, requestCode, obj as string);

        }
    }
}
