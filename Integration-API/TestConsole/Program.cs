using Integration_API.DataLayer.External;
using Integration_API.Models.OpenWeatherMap;
using Integration_API.DataLayer;
using Integration_API.LogicLayer;

OpenWeatherMapCalls calls = new OpenWeatherMapCalls("");
OpenWeatherMapService service = new OpenWeatherMapService(calls);


OpenWeatherMapResponse response = await service.GetLocalWeatherForecast("jaap");


Console.WriteLine(response.tempInCelcius);