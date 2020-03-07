using System;
using System.Collections.Generic;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using ServiceStack.OrmLite;
using Haxiot.AccountManagement.ServiceModel;
using Haxiot.AccountManagement.ServiceInterface;
using Haxiot.Connect.ServiceModel;
using Haxiot.Connect.ServiceInterface;
using System.Configuration;
using Core.Micro;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.Messaging;
using ServiceStack.Redis;
using ServiceStack.Messaging.Redis;
using Haxiot.AccountManagement;

namespace Haxiot.Connect.Tests
{
    [TestFixture]
    public class ConnectUnitTests : BaseTest
    {
        private readonly ServiceStackHost appHostConnect;
        private readonly string ConnectServiceURL = "FIX ME" /*ConfigurationManager.AppSettings["Service.Connect.URL"]*/;

        public ConnectUnitTests()
        {
            appHostConnect = new ConnectSelfHost()
            .Init()
            .Start(ConnectServiceURL);
        }

        [OneTimeTearDown]
        public void TestFixtureTearDown()
        {
            appHostConnect.Dispose();
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetGateways(string role, bool masterAccount)
        {
            return Get<GetGateways, List<Gateway>>(ConnectServiceURL, role, masterAccount, out var getObjects);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_SaveGetAndDeleteGateway(string role, bool masterAccount)
        {
            return Test_SaveGetAndDelete<Gateway, SaveGateway, GetGateway, DeleteGateway>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_SaveGetAndDeleteApplication(string role, bool masterAccount)
        {
            return Test_SaveGetAndDelete<Application, SaveApplication, GetApplication, DeleteApplication>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_SaveGetAndDeleteDevice(string role, bool masterAccount)
        {
            return Test_SaveGetAndDelete<Device, SaveDevice, GetDevice, DeleteDevice>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_SaveGetAndDeleteConfigurationPlan(string role, bool masterAccount)
        {
            return Test_SaveGetAndDelete<ConfigurationPlan, SaveConfigurationPlan, GetConfigurationPlan, DeleteConfigurationPlan>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_SaveGetAndDeleteDeviceClass(string role, bool masterAccount)
        {
            return Test_SaveGetAndDelete<DeviceClass, SaveDeviceClass, GetDeviceClass, DeleteDeviceClass>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_SaveGetAndDeleteDeviceModel(string role, bool masterAccount)
        {
            return Test_SaveGetAndDelete<GatewayModel, SaveDeviceModel, GetDeviceModel, DeleteDeviceModel>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_SaveGetAndDeleteFrequencyPlan(string role, bool masterAccount)
        {
            return Test_SaveGetAndDelete<FrequencyPlan, SaveFrequencyPlan, GetFrequencyPlan, DeleteFrequencyPlan>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_SaveGetAndDeleteMulticast(string role, bool masterAccount)
        {
            return Test_SaveGetAndDelete<Multicast, SaveMulticast, GetMulticast, DeleteMulticast>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_SaveGetAndDeleteStream(string role, bool masterAccount)
        {
            return Test_SaveGetAndDelete<Stream, SaveStream, GetStream, DeleteStream>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetGatewayByEUI(string role, bool masterAccount)
        {
            var dic = new Dictionary<string, object>()  {  {"EUI", EntityEUI}  };
            return Test_SaveGetAndDelete<Gateway, SaveGateway, GetGatewayByEui, DeleteGateway>(ConnectServiceURL, role,
                masterAccount, out var error, dic);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetGatewaysByMulticast(string role, bool masterAccount)        //all test cases get through even if unauthorised
        {
            var dic = new Dictionary<string, object>() { { "MulticastID", EntityID } };
            return Get<GetGatewaysByMulticast, List<Gateway>> (ConnectServiceURL, role,
                masterAccount, out var error, dic);
        }

        [Test]
        public void Test_GetDeviceByEui()
        {
            var testConnectService = new TestConnectService();

            var response = testConnectService.Client.Get(new GetDeviceByEui { AccountID = Guid.Parse("00000000-0000-0000-0000-000000000000"), EUI= "0004A30B0020E89A" });

            Assert.IsNotNull(response);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetDeviceByEui(string role, bool masterAccount)        //all test cases get through even if unauthorised
        {
            var dic = new Dictionary<string, object>() { { "EUI", EntityEUI } };
            return Test_SaveGetAndDelete<Device, SaveDevice, GetDeviceByEui, DeleteDevice>(ConnectServiceURL, role,
                masterAccount, out var error, dic);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetDeviceByDevAddress(string role, bool masterAccount)        //all test cases get through even if unauthorised
        {
            var dic = new Dictionary<string, object>() { { "DevAddress", EntityDevAddress } };
            return Test_SaveGetAndDelete<Device, SaveDevice, GetDeviceByDevAddress, DeleteDevice>(ConnectServiceURL, role,
                masterAccount, out var error, dic);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetDevicesByApplication(string role, bool masterAccount)
        {
            var dic = new Dictionary<string, object>() { { "ApplicationID", EntityID } };
            return Get<GetDevicesByApplication, List<Device>>(ConnectServiceURL, role,
                masterAccount, out var error, dic);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetDevicesByMulticast(string role, bool masterAccount)
        {
            var dic = new Dictionary<string, object>() { { "MulticastID", EntityID } };
            return Get<GetDevicesByMulticast, List<Device>>(ConnectServiceURL, role,
                masterAccount, out var error, dic);
        }

        [Test]
        public void Test_MigrateDevices()
        {
            var testConnectService = new TestConnectService();

            var response = testConnectService.Client.Post(new MigrateDevices
            {
                AccountID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                DestinationAccount = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                DestinationApplication = Guid.Parse("55ed32e3-8363-433c-bfec-0f56b073861a"),
                ID = Guid.Parse("38656c49-57cf-4965-b138-6d783cff3235"),
                Level = 4
            });

            var deviceResponse = testConnectService.Client.Get(new GetDevice { AccountID = Guid.Parse("00000000-0000-0000-0000-000000000000"), ID = Guid.Parse("38656c49-57cf-4965-b138-6d783cff3235") });
            Assert.IsTrue(deviceResponse.ApplicationID == Guid.Parse("55ed32e3-8363-433c-bfec-0f56b073861a")); 

            response = testConnectService.Client.Post(new MigrateDevices
            {
                AccountID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                DestinationAccount = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                DestinationApplication = Guid.Parse("505eff7b-3eb0-4f46-8cdf-13245b5a5145"),
                ID = Guid.Parse("38656c49-57cf-4965-b138-6d783cff3235"),
                Level = 4
            });
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetApplications(string role, bool masterAccount)
        {
            return Get<GetApplications, List<Application>>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetConfigurationPlans(string role, bool masterAccount)
        {
            return Get<GetConfigurationPlans, List<ConfigurationPlan>>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetConfigurationPlanByGatewayEui(string role, bool masterAccount)///////////////////////////////
        {
            var dic = new Dictionary<string, object>() { { "EUI", EntityEUI } };
            return Test_SaveGetAndDelete<ConfigurationPlan, SaveConfigurationPlan, GetConfigurationPlanByGatewayEui, DeleteConfigurationPlan>(ConnectServiceURL, role,
                masterAccount, out var error, dic);
        }

        [Test]
        public void Test_GetDeviceClasses()
        {
            var testConnectService = new TestConnectService();

            var response = testConnectService.Client.Get(new GetDeviceClasses { AccountID = Guid.Parse("00000000-0000-0000-0000-000000000000") });

            Assert.IsNotNull(response);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetDeviceClasses(string role, bool masterAccount)
        {
            return Get<GetDeviceClasses, List<DeviceClass>>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetDeviceModels(string role, bool masterAccount)
        {
            return Get<GetDeviceModels, List<GatewayModel>>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [Test]
        public void Test_EnqueueMessage()////////////////////////////////////////////////////
        {
            var testConnectService = new TestConnectService();

            //var response = testConnectService.Client.Get(new EnqueueMessage { AccountID = Guid.Parse("00000000-0000-0000-0000-000000000000"),   });

            //Assert.IsNotNull(response);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetFrequencyPlans(string role, bool masterAccount)
        {
            return Get<GetFrequencyPlans, List<FrequencyPlan>>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetGatewayProfiles(string role, bool masterAccount)
        {
            return Get<GetGatewayProfiles, List<GatewayProfile>>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetMetrics(string role, bool masterAccount)        //all test cases get through even if unauthorised
        {
            return Get<GetMetrics, List<Metric>>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [TestCase("system", true, ExpectedResult = true)]
        [TestCase("admin", true, ExpectedResult = true)]
        [TestCase("user", true, ExpectedResult = true)]
        [TestCase("admin", false, ExpectedResult = false)]
        [TestCase("user", false, ExpectedResult = false)]
        public bool Test_GetMetricsByAccount(string role, bool masterAccount)        //all test cases get through even if unauthorised
        {
            return Get<GetMetricsByAccount, List<Metric>>(ConnectServiceURL, role,
                masterAccount, out var error);
        }

        [Test]
        public void Test_GetMulticastByGroupID()
        {
            var testConnectService = new TestConnectService();

            var response = testConnectService.Client.Get(new GetMulticastByGroupID { AccountID = Guid.Parse("00000000-0000-0000-0000-000000000000"), GroupID = "FE0A00000000" });

            Assert.IsNotNull(response);
        }

        [Test]
        public void Test_GetMulticasts()
        {
            var testConnectService = new TestConnectService();

            var response = testConnectService.Client.Get(new GetMulticasts { AccountID = Guid.Parse("00000000-0000-0000-0000-000000000000") });

            Assert.IsNotNull(response);
        }

        [Test]
        public void Test_GetNetworkStat()
        {
            var testConnectService = new TestConnectService();

            var response = testConnectService.Client.Get(new GetNetworkStat { AccountID = Guid.Parse("00000000-0000-0000-0000-000000000000") });

            Assert.IsNotNull(response);
        }
    }

    public class TestConnectService
    {
        //public string Token;

        private JsonServiceClient client;

        public JsonServiceClient Client
        {
            get { return client; }
            set { client = value; }
        }

        public TestConnectService(string role = "system")
        {
            string token = "";
            switch (role)
            {
                case "user":
                    token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiIsImtpZCI6Ikt6cCJ9.eyJpYXQiOjE1MjgyMzk4NzQsImV4cCI6MTU1OTc3NTg3NCwiZW1haWwiOiJVc2VyQGVzdC5jb20iLCJnaXZlbl9uYW1lIjoiVXNlciIsImZhbWlseV9uYW1lIjoiVXNlciIsInByZWZlcnJlZF91c2VybmFtZSI6IlVzZXIiLCJyb2xlcyI6WyJVc2VyIiwiVXNlciIsIkFkbWluaXN0cmF0b3IiLCJVc2VyIiwiVXNlciIsIkFkbWluaXN0cmF0b3IiXSwiQWNjb3VudElEIjoiMDAwMDAwMDAtMDAwMC0wMDAwLTAwMDAtMDAwMDAwMDAwMDAwIn0.SH79NTmNUAZNZLFTQzCsbVRr3Y7511CKXIQIF__GaB8";
                    break;
                case "admin":
                    token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiIsImtpZCI6Ikt6cCJ9.eyJpYXQiOjE1MjgxNzkyMjYsImV4cCI6MTU1OTcxNTIyNiwiZW1haWwiOiJBZG1pbkB0ZXN0LmNvbSIsImdpdmVuX25hbWUiOiJBZG1pbiIsImZhbWlseV9uYW1lIjoiQWRtaW4iLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJBZG1pbiIsInJvbGVzIjpbIkFkbWluaXN0cmF0b3IiXSwiQWNjb3VudElEIjoiMDAwMDAwMDAtMDAwMC0wMDAwLTAwMDAtMDAwMDAwMDAwMDAwIn0.oUdJEwiJwv4R10ReNLFiaKqgxI-mj3y_bF549V1FT0g";
                    break;
                default:
                    token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiIsImtpZCI6Ikt6cCJ9.eyJpYXQiOjE1Mjc2NjEwMTgsImV4cCI6MTU1OTE5NzAxOCwiZW1haWwiOiJzeXN0ZW1AaGF4aW90LmNvbSIsImdpdmVuX25hbWUiOiJTeXN0ZW0iLCJmYW1pbHlfbmFtZSI6IlVzZXIiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJzeXN0ZW0taGF4aW90Iiwicm9sZXMiOlsiQWRtaW5pc3RyYXRvciIsIlN5c3RlbUFkbWluIiwiVXNlciJdLCJBY2NvdW50SUQiOiIwMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAifQ.1_HrytxA6AKVa2LYev_ERv0ef-vL1GQYGO5CjYV_oU4";
                    break;
            }
            Client = new JsonServiceClient("FIX ME" /*ConfigurationManager.AppSettings["Service.Connect.URL"]*/)
            {
                BearerToken = token
            };
        }
    }
}
