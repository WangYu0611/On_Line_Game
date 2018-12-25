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
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        private Server server;

        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }



        void InitController()
        {

            DefaultController defaultController = new DefaultController();
            controllerDict.Add(defaultController.RequestCode, defaultController);
            controllerDict.Add(RequestCode.User, new UserController());
            controllerDict.Add(RequestCode.Room, new RoomController());


        }



        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client, Server server)
        {
            BaseController controller;
            bool isGet = controllerDict.TryGetValue(requestCode, out controller);
            if (!isGet)
            {
                Console.WriteLine("无法得到" + requestCode + "对应的Controller,无法处理请求！");
                return;
            }

            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            Console.WriteLine(methodName);
            MethodInfo mi = controller.GetType().GetMethod(methodName);
            if (mi == null)
            {
                Console.WriteLine("[警告] 没有在Controller[" + controller.GetType() + "]中没有对应的处理方法：  [" + methodName + "]");
                return;
            }

            object[] parameters = new object[] { data, client, server };
            object obj = mi.Invoke(controller, parameters);
            if (obj == null || string.IsNullOrEmpty(obj as string))
            {
                return;
            }
            server.SendResponse(client, actionCode, obj as string);

        }
    }
}
