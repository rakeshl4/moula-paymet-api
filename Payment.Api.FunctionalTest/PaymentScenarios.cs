using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Api.FunctionalTest
{
    [TestClass]
    public class PaymentScenarios
      : PaymentScenariosBase
    {
        [TestMethod]
        public async Task GetAllPayments_WithValidCustomerId_ShouldReturnOkStatus()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync("api/customer/eb07ea19-38cc-4579-892c-510da1eca613");
                response.EnsureSuccessStatusCode();
            }
        }

        [TestMethod]
        public async Task GetAllPayments_WithInvalidValidCustomerId_ShouldReturnNotFoundStatus()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync($"api/customer/{System.Guid.NewGuid()}");
                Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            }
        }

        [TestMethod]
        public async Task GetAllPayments_WithValidPayment_ShouldCompleteSuccessfully()
        {
            using (var server = CreateServer())
            {
                var client = server.CreateClient();
                var payload = "{\"customerId\":\"eb07ea19-38cc-4579-892c-510da1eca613\", \"amount\":435.6, \"transactionDate\":\"2015-01-01T15:23:42\"}";
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"api/payment/create", content);
                Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

                var paymentIdString = await response.Content.ReadAsStringAsync();
                var paymentId = JsonConvert.DeserializeObject(paymentIdString);
                payload = "{\"transactionId\":\"" + paymentId + "\"}";
                content = new StringContent(payload, Encoding.UTF8, "application/json");
                response = await client.PostAsync($"api/payment/approve", content);
                Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            }
        }

        [TestMethod]
        public async Task GetAllPayments_CancelApprovedAccount_ShouldFail()
        {
            using (var server = CreateServer())
            {
                var client = server.CreateClient();
                var payload = "{\"customerId\":\"eb07ea19-38cc-4579-892c-510da1eca613\", \"amount\":435.6, \"transactionDate\":\"2015-01-01T15:23:42\"}";
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"api/payment/create", content);
                Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

                var paymentIdString = await response.Content.ReadAsStringAsync();
                var paymentId = JsonConvert.DeserializeObject(paymentIdString);
                payload = "{\"transactionId\":\"" + paymentId + "\"}";
                content = new StringContent(payload, Encoding.UTF8, "application/json");
                response = await client.PostAsync($"api/payment/approve", content);
                Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);

                response = await client.PostAsync($"api/payment/cancel", content);
                Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
