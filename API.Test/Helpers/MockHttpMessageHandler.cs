using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Test.Helpers
{
    internal static class MockHttpMessageHandler<T>
    {
        internal static Mock<HttpMessageHandler> SetupBasicGetResourceList(List<T> expectedResponse)
        {
            var mockResponse=new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content=new StringContent(JsonConvert.SerializeObject(expectedResponse))
            };
            mockResponse.Content.Headers.ContentType= 
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var handlerMock=new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpResponseMessage>(),
                ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(mockResponse);
            return handlerMock;
        }
    }
}
